using Microsoft.Win32;
using RollingLineSavegameFix.ViewModel;
using SharpMik;
using SharpMik.Drivers;
using SharpMik.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RollingLineSavegameFix.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        Module m_Mod = null;
        MikMod m_Player;


        public MainView()
        {
            InitializeComponent();            
            InitializeBackgroundMusic();
            DataContext = new MainViewModel();
        }

        /// <summary>
        /// Fucking MusicPlayer must be in MainView
        /// </summary>
        private void InitializeBackgroundMusic()
        {

            m_Player = new MikMod();
            ModDriver.Mode = (ushort)(ModDriver.Mode | SharpMikCommon.DMODE_NOISEREDUCTION);            
            m_Player.Init<NaudioDriver>("");

            //TODO: Modify to stream
            m_Mod = ModuleLoader.Load("Resources\\music.xm");

            //m_Player.Play(m_Mod);
        }

        /// <summary>
        /// Fucking Workaround for OpenFileDialog in WPF
        /// </summary>        
        private void OpenFileBrowser_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;            
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                ((MainViewModel)DataContext).FileName = openFileDialog.FileName;
            }
        }
    }
}
