using System;
using System.Windows;
using System.Windows.Controls;

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
