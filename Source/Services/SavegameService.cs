using RollingLineSavegameFix.Model;
using System;
using System.IO;
using System.Windows;

namespace RollingLineSavegameFix.Services
{
    public class SavegameService : ISavegameService
    {
        private readonly MainModel _model;
        private readonly IBackupService _backupService;
        private readonly IReformatService _reformatService;
        private readonly IRemoveWaggonsService _removeWaggonsService;

        public SavegameService(
            MainModel model, 
            IBackupService backupService,
            IReformatService reformatService,
            IRemoveWaggonsService removeWaggonsService)
        {
            _model = model;
            _backupService = backupService;
            _reformatService = reformatService;
            _removeWaggonsService = removeWaggonsService;
        }

        public string LoadSavegame()
        {
            try
            {
                _model.FileContent = File.ReadAllText(_model.FileName);
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

            File.WriteAllText(_model.FileName+"_", _model.FileContent);

            Console.WriteLine("Fertsch");
        }
              
    }
}
