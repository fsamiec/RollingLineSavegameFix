using Xunit;
using AutoFixture.Xunit2;
using FluentAssertions;
using RollingLineSavegameFix.Services;
using RollingLineSavegameFix.Model;
using System;
using System.IO.Abstractions.TestingHelpers;
using NSubstitute;
using System.IO.Abstractions;

namespace RollingLineSavegameFix.Tests.Services
{
    public class ReformatServiceTests
    {
        [Theory, RLAutoData]
        public void Reformat_ServiceFindsNothingToReformat_DoesNothing(
            IMainModel mainModel, 
            ReformatService sut,
            string dummyContent)
        {
            //Arrange
            var unmodified = dummyContent;
            mainModel.FileContent.Returns(dummyContent);

            //Act
            sut.Reformat();

            //Assert
            mainModel.FileContent.Should().Be(unmodified);
        }

        [Theory, RLAutoData]
        public void Reformat_ServiceFindsNoQuickMods_EliminatesAllLineBreaks(
            IMainModel mainModel,
            ReformatService sut,            
            int randomLineBreakInserter
            )
        {
            //Arrange
            var lorizzleIppsle = "Lorizzle ipsum dolor ghetto amizzle, mah nizzle adipiscing elit. You son of a bizzle ut dolizzle.Things magna ligula, dignissim sit amizzle, that's the shizzle eget, the bizzle nec, the bizzle.";
            var content = lorizzleIppsle;
            mainModel.FileContent.Returns(content);
            randomLineBreakInserter = randomLineBreakInserter % 5 + 2;

            mainModel.FileContent = lorizzleIppsle;
            var lineBreakCounter = 0;
            for (var i = randomLineBreakInserter; i < mainModel.FileContent.Length; i = i + randomLineBreakInserter + 1)
            {
                mainModel.FileContent.Insert(i, Environment.NewLine);
                lineBreakCounter++;
            }

            //Act
            sut.Reformat();
            
            //Assert
            mainModel.FileContent.Should().Be(lorizzleIppsle);
            lineBreakCounter.Should().NotBe(0);
        }
    }

    public class SavegameServiceTests
    {
        [Theory, RLAutoData]
        public void LoadSaveGame_FileCanBeReadWithoutError_ReturnsEmptyString(
            [Frozen] IMainModel model,
            [Frozen] IBackupService backupService,
            [Frozen] IReformatService reformatService,
            [Frozen] IRemoveWaggonsService removeWaggonsService,
            string fileContent,
            string filePath
            )
        {
            //Arrange

            var mockedFileSystem = new MockFileSystem();
            var mockedFileContent = new MockFileData(fileContent);
            mockedFileSystem.AddFile(filePath, mockedFileContent) ;
            model.FileName.Returns(filePath);            
            var sut = new SavegameService(model, backupService, reformatService, removeWaggonsService, mockedFileSystem);

            //Act
            var result = sut.LoadSavegame();

            result.Should().BeEmpty();
            model.FileContent.Should().Be(fileContent);
            
            backupService.ReceivedCalls().Should().BeEmpty();
            reformatService.ReceivedCalls().Should().BeEmpty();
            removeWaggonsService.ReceivedCalls().Should().BeEmpty();
        }

        [Theory, RLAutoData]
        public void LoadSaveGame_FileNotFound_ReturnsFileNotFoundErrorText(
            [Frozen] IMainModel model,
            [Frozen] IBackupService backupService,
            [Frozen] IReformatService reformatService,
            [Frozen] IRemoveWaggonsService removeWaggonsService,            
            string filePath
            )
        {
            //Arrange

            var mockedFileSystem = new MockFileSystem();                        
            model.FileName.Returns(filePath);
            var sut = new SavegameService(model, backupService, reformatService, removeWaggonsService, mockedFileSystem);

            //Act
            var result = sut.LoadSavegame();

            result.Should().Be($"Error: File {filePath} not Found.");
            model.FileContent.Should().BeEmpty();

            backupService.ReceivedCalls().Should().BeEmpty();
            reformatService.ReceivedCalls().Should().BeEmpty();
            removeWaggonsService.ReceivedCalls().Should().BeEmpty();
        }
    }

    public class BackupServiceTests
    {
        [Theory, RLAutoData]
        public void WriteBackupFile_WritesBackupFile_EveryoneIsHappy(
            [Frozen] IMainModel model,
            string fileContent)
        {
            var directory = @"C:\Temp\";
            var fileName = "temp";
            var fileExtension = ".txt";
            var filePath = directory + fileName + fileExtension;

            var mockedFileSystem = new MockFileSystem();            
            var mockedFileContent = new MockFileData(fileContent);
            mockedFileSystem.AddDirectory(directory);
            mockedFileSystem.AddFile(filePath, mockedFileContent);

            model.FileName = filePath;

            var sut = new BackupService(model, mockedFileSystem);

            sut.WriteBackupFile();

            var expectedPath = $"{directory}{fileName}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";

            mockedFileSystem.FileExists(expectedPath).Should().BeTrue();
        }
    }
}
