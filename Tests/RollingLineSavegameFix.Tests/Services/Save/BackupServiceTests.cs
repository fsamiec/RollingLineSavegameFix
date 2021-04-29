using Xunit;
using AutoFixture.Xunit2;
using FluentAssertions;
using RollingLineSavegameFix.Services;
using RollingLineSavegameFix.Model;
using System;
using System.IO.Abstractions.TestingHelpers;

namespace RollingLineSavegameFix.Tests.Services
{
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

            var expectedPath = $"{directory}{fileName}_backup_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";

            mockedFileSystem.FileExists(expectedPath).Should().BeTrue();
        }
    }
}
