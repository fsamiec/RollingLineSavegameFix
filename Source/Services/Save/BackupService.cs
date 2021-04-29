using RollingLineSavegameFix.Model;
using System;
using System.IO.Abstractions;

namespace RollingLineSavegameFix.Services
{
    public class BackupService : IBackupService
    {
        private readonly IMainModel _model;
        private readonly IFileSystem _fileSystem;
        public BackupService(IMainModel model) : this (model, new FileSystem())
        { }

        public BackupService(IMainModel model, IFileSystem fileSystem)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public void WriteBackupFile()
        {            
            var directory = _fileSystem.Path.GetDirectoryName(_model.FileName);
            var filenameWithoutExtension = _fileSystem.Path.GetFileNameWithoutExtension(_model.FileName);
            var extension = _fileSystem.Path.GetExtension(_model.FileName);

            var backupFileName = $"{directory}\\{filenameWithoutExtension}_backup_{DateTime.Now:yyyyMMddHHmmss}{extension}";
            _fileSystem.File.WriteAllText(backupFileName, _model.FileContent);                        
        }
    }   
}
