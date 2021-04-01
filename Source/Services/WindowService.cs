using RollingLineSavegameFix.View;

namespace RollingLineSavegameFix.Services
{
    public class WindowService : IWindowService
    {        
        public void ShowNfoDialogWindow()
        {            
            var nfoWindow = new NfoWindowView();
            nfoWindow.ShowDialog();
        }
    }
}
