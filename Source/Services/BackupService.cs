using RollingLineSavegameFix.Model;
using System;
using System.IO;

namespace RollingLineSavegameFix.Services
{
    public class BackupService : IBackupService
    {
        private readonly MainModel _model;
        public BackupService(MainModel model)
        {
            _model = model;
        }

        public void WriteBackupFile()
        {
            var directory = Path.GetDirectoryName(_model.FileName);
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(_model.FileName);
            var extension = Path.GetExtension(_model.FileName);

            var backupFileName = $"{directory}\\{filenameWithoutExtension}_{DateTime.Now:yyyyMMddHHmmss}{extension}";            
            File.WriteAllText(backupFileName, _model.FileContent);                        
        }
    }   
}
