using RollingLineSavegameFix.Services;

namespace RollingLineSavegameFix.ViewModel
{
    public class NfoWindowViewModel : ViewModelBase
    {

        private readonly INfoReaderService _nfoReaderService;

        public NfoWindowViewModel()
        {
            _nfoReaderService = new NfoReaderService();
        }

        public string NfoText
        {
            get => _nfoReaderService.ReadNfo();
        }


        public string NfoTitle => return "-:-We Code Together -We Die Together -:-";
    }
}
