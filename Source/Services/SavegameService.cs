using RollingLineSavegameFix.Model;
using System.IO;

namespace RollingLineSavegameFix.Services
{
    /// <summary>
    /// Manipulates Savegame as a fancy Service
    /// </summary>
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
            
            if (_model.ShouldRemoveFaultyWaggons)
            {
                _removeWaggonsService.RemoveFaultyQuickmodWaggons();
            }

            File.WriteAllText(_model.FileName, _model.FileContent);         
        }              
    }
}
