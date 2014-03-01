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

namespace RoverOperator.Content
{
    /// <summary>
    /// Interaction logic for NetworkSettings.xaml
    /// </summary>
    public partial class NetworkSettings : UserControl
    {
        public NetworkSettings()
        {
            InitializeComponent();
            DataContext = new NetworkSettingsViewModel();
        }

        private void roverIPAddressLostFocus(object sender, EventArgs e)
        {
            var textbox = (DependencyObject)sender;
            if (Validation.GetHasError(textbox)) this.SaveButton.IsEnabled = false;
            else this.SaveButton.IsEnabled = true;
        }

    }
}
