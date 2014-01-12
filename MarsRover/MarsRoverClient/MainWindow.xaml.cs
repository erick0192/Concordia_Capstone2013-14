using FirstFloor.ModernUI.Windows.Controls;
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
using MarsRoverClient.Content;

namespace MarsRoverClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            FirstFloor.ModernUI.Presentation.AppearanceManager.Current.AccentColor = Color.FromRgb(0xa2, 0x00, 0x25);

            DataContext = new MainWindowViewModel();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);            
            Application.Current.Shutdown();
        }
    }
}
