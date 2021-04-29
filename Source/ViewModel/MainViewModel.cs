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
        private IFindWaggonsRegExService _findWaggonsRegExService;
        private IFindObjectsRegExService _findObjectsRegExService;
        private IFindTracksRegExService _findTracksRegExService;
        private IRemoveWaggonsService _removeWaggonsService;
        private IParseAndAddFloatValue _parseAndAddFloatValue;
        private IMoveCoWireObjectService _moveCoWireObjectService;
        private IMoveObjectsService _moveObjectsService;
        private IMoveTracksService _moveTracksService;
        private IMoveWaggonsService _moveWaggonsService;
        private IMainModel _model;

        public MainViewModel()
        {
            _model = new MainModel();

            InitializeServices();
        }

        private void InitializeServices()
        {
            _backupService = new BackupService(_model);
            _reformatService = new ReformatService(_model);
            _findWaggonsRegExService = new FindWaggonsRegExService();
            _removeWaggonsService = new RemoveWaggonsService(_model, _reformatService, _findWaggonsRegExService);
            
            _findObjectsRegExService = new FindObjectsRegExService();
            _findTracksRegExService = new FindTracksRegExService();

            _parseAndAddFloatValue = new ParseAndAddFloatValue();
            _moveCoWireObjectService = new MoveCoWireObjectService(_model, _parseAndAddFloatValue);
            _moveObjectsService = new MoveObjectsService(_model, _findObjectsRegExService, _moveCoWireObjectService, _parseAndAddFloatValue);            
            _moveTracksService = new MoveTracksService(_model, _findTracksRegExService, _parseAndAddFloatValue);
            _moveWaggonsService = new MoveWaggonsService(_model, _findWaggonsRegExService, _parseAndAddFloatValue);

            _savegameService = new SavegameService(_model, _backupService, _removeWaggonsService, _moveObjectsService, _moveTracksService, _moveWaggonsService);            
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
            get => _model.ShouldRemoveAllWaggons; 
            set
            {
                if (_model.ShouldRemoveAllWaggons == value)
                    return;

                _model.ShouldRemoveAllWaggons = value;
                OnPropertyChanged(nameof(ShouldRemoveAllWaggons));
            }
        }


        /// <summary>
        ///     Triggers object moving
        /// </summary>
        public bool ShouldMoveObjects 
        { 
            get => _model.ShouldMoveObjects;
            set
            {
                if (_model.ShouldMoveObjects == value)
                    return;

                _model.ShouldMoveObjects = value;
                OnPropertyChanged(nameof(ShouldMoveObjects));
            }
        }

        /// <summary>
        ///     Moves objects on X Axis
        /// </summary>
        public float MoveXAxisValue
        {
            get => _model.MoveXAxisValue;
            set
            {
                if (_model.MoveXAxisValue == value)
                    return;

                _model.MoveXAxisValue = value;
                OnPropertyChanged(nameof(MoveXAxisValue));
            }
        }

        /// <summary>
        ///     Moves objects on Y Axis
        /// </summary>
        public float MoveYAxisValue
        {
            get => _model.MoveYAxisValue;
            set
            {                
                if (_model.MoveYAxisValue == value)
                    return;

                _model.MoveYAxisValue = value;
                OnPropertyChanged(nameof(MoveYAxisValue));
            }
        }

        /// <summary>
        ///     Moves objects on Z Axis
        /// </summary>
        public float MoveZAxisValue
        {
            get => _model.MoveZAxisValue;
            set
            {
                if (_model.MoveZAxisValue == value)
                    return;

                _model.MoveZAxisValue = value;
                OnPropertyChanged(nameof(MoveZAxisValue));
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
