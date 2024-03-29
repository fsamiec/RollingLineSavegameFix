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
        private readonly IRemoveWaggonsService _removeWaggonsService;
        private readonly IMoveObjectsService _moveObjectsService;
        private readonly IMoveTracksService _moveTracksService;
        private readonly IMoveWaggonsService _moveWaggonsService;
        private readonly IFileSystem _fileSystem;

        public SavegameService(
            IMainModel model, 
            IBackupService backupService,
            IRemoveWaggonsService removeWaggonsService,
            IMoveObjectsService moveObjectsService,
            IMoveTracksService moveTracksService,
            IMoveWaggonsService moveWaggonsService,
            IFileSystem fileSystem) 
        {
            _model = model;
            _backupService = backupService;
            _removeWaggonsService = removeWaggonsService;
            _moveObjectsService = moveObjectsService;
            _moveTracksService = moveTracksService;
            _moveWaggonsService = moveWaggonsService;
            _fileSystem = fileSystem;
        }

        public SavegameService(
            IMainModel model,
            IBackupService backupService,
            IRemoveWaggonsService removeWaggonsService,
            IMoveObjectsService moveObjectsService,
            IMoveTracksService moveTracksService,
            IMoveWaggonsService moveWaggonsService)
            : this(model, backupService, removeWaggonsService, moveObjectsService, moveTracksService, moveWaggonsService,new FileSystem())
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

            if (_model.ShouldRemoveAllWaggons)
            {
                _removeWaggonsService.RemoveAllWaggons();
            } 
            
            if (_model.ShouldRemoveFaultyWaggons)
            {
                _removeWaggonsService.RemoveFaultyQuickmodWaggons();
            }

            if (_model.ShouldMoveObjects)
            {
                _moveObjectsService.Move();
                _moveTracksService.Move();
                _moveWaggonsService.Move();
            }

            _fileSystem.File.WriteAllText(_model.FileName, _model.FileContent);


            System.Windows.Clipboard.SetText(_model.FileContent); 
        }
    }
}
