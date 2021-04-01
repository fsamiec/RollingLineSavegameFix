using System.Windows.Input;
using RollingLineSavegameFix.Services;

namespace RollingLineSavegameFix.ViewModel
{
    public class StatusBarViewModel : ViewModelBase
    {
        private readonly IWindowService _windowService;
        public StatusBarViewModel()
        {
            _windowService = new WindowService();
        }

        private ICommand _nfoCommand;
        public ICommand NfoCommand
        {
            get
            {
                if (_nfoCommand != null)
                {
                    return _nfoCommand;
                }

                _nfoCommand = new RelayCommand(
                    p => true,
                    p => _windowService.ShowNfoDialogWindow());

                return _nfoCommand;
            }
        }
    }
}
