using RollingLineSavegameFix.View;

namespace RollingLineSavegameFix.Services.Misc
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
