using System.Windows;
using System.Windows.Input;
using RollingLineSavegameFix.Model;
using RollingLineSavegameFix.Services;

namespace RollingLineSavegameFix.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ISavegameService _savegameService;
        private IBackupService _backupService;
        private IReformatService _reformatService;
        private IRemoveWaggonsService _removeWaggonsService;
        private MainModel _model;

        public MainViewModel()
        {
            _model = new MainModel();

            InitializeServices();
        }

        private void InitializeServices()
        {
            _backupService = new BackupService(_model);
            _reformatService = new ReformatService(_model);
            _removeWaggonsService = new RemoveWaggonsService(_model);
            _savegameService = new SavegameService(_model, _backupService, _reformatService, _removeWaggonsService);
            
        }

        public string FileName
        {
            get => _model.FileName;
            set
            {
                if (_model.FileName == value)
                    return;

                _model.FileName = value;
                LoadFileContent();
                OnPropertyChanged(nameof(FileName));
            }
        }

        /// <summary>
        /// Enables GUI after Filename is set
        /// </summary>
        public bool AreOptionsAvaiable
        {
            get => !string.IsNullOrWhiteSpace(FileName);
        }

        private bool shouldNotRemoveWaggons = true;
        /// <summary>
        /// Remove no Waggons
        /// </summary>
        public bool ShouldNotRemoveWaggons
        {
            get => shouldNotRemoveWaggons;
            set
            {
                if (shouldNotRemoveWaggons == value)
                {
                    return;
                }

                shouldNotRemoveWaggons = value;
                OnPropertyChanged(nameof(ShouldNotRemoveWaggons));
            }
        }
        
        /// <summary>
        ///     Remove faulty Waggons
        /// </summary>
        public bool ShouldRemoveFaultyWaggons
        {
            get => _model.ShouldRemoveFaultyWaggons; set
            {
                if (_model.ShouldRemoveFaultyWaggons == value)
                    return;

                _model.ShouldRemoveFaultyWaggons = value;
                OnPropertyChanged(nameof(ShouldRemoveFaultyWaggons));
            }
        }

        /// <summary>
        ///     Remove all Waggons
        /// </summary>
        public bool ShouldRemoveAllWaggons
        {
            get => _model.ShouldRemoveAllWaggons; set
            {
                if (_model.ShouldRemoveAllWaggons == value)
                    return;

                _model.ShouldRemoveAllWaggons = value;
                OnPropertyChanged(nameof(ShouldRemoveAllWaggons));
            }
        }

        private ICommand _doItNowCommand;
        public ICommand DoItNowCommand
        {
            get
            {
                if (_doItNowCommand != null)
                {
                    return _doItNowCommand;
                }

                _doItNowCommand = new RelayCommand(
                    p => AreOptionsAvaiable,
                    p => _savegameService.WriteNewSaveGame());

                return _doItNowCommand;
            }
        }

        private void LoadFileContent()
        {
            var message = _savegameService.LoadSavegame();

            if (!string.IsNullOrEmpty(message))            
            {
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _model.FileContent = null;
                _model.FileName = null;
            }

            OnPropertyChanged(nameof(AreOptionsAvaiable));
            OnPropertyChanged(nameof(FileName));
        }
    }
}
