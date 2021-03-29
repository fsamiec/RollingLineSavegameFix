﻿using RollingLineSavegameFix.Model;
using System.IO;
using System.IO.Abstractions;

namespace RollingLineSavegameFix.Services
{
    /// <summary>
    /// Manipulates Savegame as a fancy Service
    /// </summary>
    public class SavegameService : ISavegameService
    {
        private readonly IMainModel _model;
        private readonly IBackupService _backupService;
        private readonly IReformatService _reformatService;
        private readonly IRemoveWaggonsService _removeWaggonsService;
        private readonly IFileSystem _fileSystem;

        public SavegameService(
            IMainModel model, 
            IBackupService backupService,
            IReformatService reformatService,
            IRemoveWaggonsService removeWaggonsService,
            IFileSystem fileSystem) 
        {
            _model = model;
            _backupService = backupService;
            _reformatService = reformatService;
            _removeWaggonsService = removeWaggonsService;
            _fileSystem = fileSystem;
        }

        public SavegameService(
            IMainModel model,
            IBackupService backupService,
            IReformatService reformatService,
            IRemoveWaggonsService removeWaggonsService)
            : this(model, backupService, reformatService, removeWaggonsService, new FileSystem())
        {            
        }

        public string LoadSavegame()
        {
            try
            {
                _model.FileContent = _fileSystem.File.ReadAllText(_model.FileName);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                return $"Error: File {fileNotFoundException.FileName} not Found.";
            }
            catch (IOException ioException)
            {
                return $"Error: IOEception caught while opening File {_model.FileName}. {ioException.Message}";
            }
            return string.Empty;
        }

        public void WriteNewSaveGame()
        {
            _backupService.WriteBackupFile();
            _reformatService.Reformat();

            if (_model.ShouldRemoveAllWaggons)
            {
                _removeWaggonsService.RemoveAllWaggons();
            } 
            
            if (_model.ShouldRemoveFaultyWaggons)
            {
                _removeWaggonsService.RemoveFaultyQuickmodWaggons();
            }

            _fileSystem.File.WriteAllText(_model.FileName, _model.FileContent);         
        }              
    }
}
