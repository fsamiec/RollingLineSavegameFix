using Xunit;
using AutoFixture.Xunit2;
using FluentAssertions;
using RollingLineSavegameFix.Services;
using RollingLineSavegameFix.Model;
using System.IO.Abstractions.TestingHelpers;
using NSubstitute;

namespace RollingLineSavegameFix.Tests.Services
{
    public class SavegameServiceTests
    {
        [Theory, RLAutoData]
        public void LoadSaveGame_FileCanBeReadWithoutError_ReturnsEmptyString(
            [Frozen] IMainModel model,
            [Frozen] IBackupService backupService,
            [Frozen] IRemoveWaggonsService removeWaggonsService,
            [Frozen] IMoveObjectsService moveObjectsService,
            [Frozen] IMoveTracksService moveTracksService,
            [Frozen] IMoveWaggonsService moveWaggonsService,
            string fileContent,
            string filePath
            )
        {
            //Arrange

            var mockedFileSystem = new MockFileSystem();
            var mockedFileContent = new MockFileData(fileContent);
            mockedFileSystem.AddFile(filePath, mockedFileContent) ;
            model.FileName.Returns(filePath);            
            var sut = new SavegameService(model, backupService, removeWaggonsService, moveObjectsService, moveTracksService, moveWaggonsService, mockedFileSystem);            

            //Act
            var result = sut.LoadSavegame();

            result.Should().BeEmpty();
            model.FileContent.Should().Be(fileContent);
            
            backupService.ReceivedCalls().Should().BeEmpty();
            removeWaggonsService.ReceivedCalls().Should().BeEmpty();
        }

        [Theory, RLAutoData]
        public void LoadSaveGame_FileNotFound_ReturnsFileNotFoundErrorText(
            [Frozen] IMainModel model,
            [Frozen] IBackupService backupService,
            [Frozen] IRemoveWaggonsService removeWaggonsService,            
            [Frozen] IMoveObjectsService moveObjectsService,
            [Frozen] IMoveTracksService moveTracksService,
            [Frozen] IMoveWaggonsService moveWaggonsService,
            string filePath
            )
        {
            //Arrange

            var mockedFileSystem = new MockFileSystem();                        
            model.FileName.Returns(filePath);            
            var sut = new SavegameService(model, backupService, removeWaggonsService, moveObjectsService, moveTracksService, moveWaggonsService, mockedFileSystem);

            //Act
            var result = sut.LoadSavegame();

            result.Should().Be($"Error: File {filePath} not Found.");
            model.FileContent.Should().BeEmpty();

            backupService.ReceivedCalls().Should().BeEmpty();
            removeWaggonsService.ReceivedCalls().Should().BeEmpty();
        }
    }
}
