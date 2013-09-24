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
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using MRClient_ModernUIProtoss.Content;

namespace MRClient_ModernUIProtoss.Pages
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl, IContent
    {
        public Main()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            ((MainViewModel)DataContext).UpperLeftCameraVM = (CameraViewModel)this.camFront.DataContext;
            ((MainViewModel)DataContext).UpperRightCameraVM = (CameraViewModel)this.camBack.DataContext;
            ((MainViewModel)DataContext).LowerLeftCameraVM = (CameraViewModel)this.camLeft.DataContext;
            ((MainViewModel)DataContext).LowerRightCameraVM = (CameraViewModel)this.camRight.DataContext; 
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(LoginControl_IsVisibleChanged);
        }

        void LoginControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.ContextIdle,
                new Action(delegate()
                {
                    this.Focus();
                    this.camFront.Focus();
                }));
            }
        }  

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            // ask user if navigating away is ok
            MessageBox.Show("Fragmenting Navigating");
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            this.Focus();
            this.camFront.Focus();
            // ask user if navigating away is ok
            MessageBox.Show("NavigatedFrom");
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            this.Focus();
            this.camFront.Focus();
            // ask user if navigating away is ok
            MessageBox.Show("navigating to");
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            // ask user if navigating away is ok
            MessageBox.Show("navigating from");
        }
    }
}
