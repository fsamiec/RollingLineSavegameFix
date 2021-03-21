using RollingLineSavegameFix.Model;
using System.IO;
using System.Windows;

namespace RollingLineSavegameFix.Services
{
    public class SavegameService : ISavegameService
    {
        private readonly MainModel _model;
        private readonly IBackupService _backupService;

        public SavegameService(MainModel model, IBackupService backupService)
        {
            _model = model;
            _backupService = backupService;
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
            var content = _model.FileContent;
            MessageBox.Show("Palim Palim " + content);            
        }

       
    }
}
