using RollingLineSavegameFix.Services.Misc;

namespace RollingLineSavegameFix.ViewModel
{
    public class NfoWindowViewModel : ViewModelBase
    {
        private readonly INfoReaderService _nfoReaderService;

        public NfoWindowViewModel()
        {
            _nfoReaderService = new NfoReaderService();
        }

        public string NfoText => _nfoReaderService.ReadNfo();        

        public string NfoTitle => "-:- We Code Together - We Die Together -:-";
    }
}
