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
using MarsRover.Streams;
using RoverOperator.Content;
using Xceed.Wpf.AvalonDock.Layout;

namespace RoverOperator.Pages
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        public Main()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            ((MainViewModel)DataContext).DockingManager = dockingManager;

            //Instantiate VM for camera views            
            CameraViewModel cvm = this.Cam1.DataContext as CameraViewModel;
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "Front";
            cvm.VideoSource = VideoStreamReceiverManager.Instance.Camera1;
            
            ((MainViewModel)DataContext).VMCamera1 = cvm;
            
            cvm = this.Cam2.DataContext as CameraViewModel;
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "Back";
            cvm.VideoSource = VideoStreamReceiverManager.Instance.Camera2;
            ((MainViewModel)DataContext).VMCamera2 = cvm;
            
            cvm = this.Cam3.DataContext as CameraViewModel;
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "Left";
            cvm.VideoSource = VideoStreamReceiverManager.Instance.Camera3;       
            ((MainViewModel)DataContext).VMCamera3 = cvm;          

            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(((MainViewModel)DataContext).MainIsVisibleChanged);
            this.FocusVisualStyle = new Style();//Get rid of dotted rectangle that indicates its focused    

            //Start or hide the cameras depending on configuration
            //((MainViewModel)DataContext).VMFrontCamera.VideoSource.Start();
            this.LayoutCam1.Hide();
            //((MainViewModel)DataContext).VMBackCamera.VideoSource.Start();
            this.LayoutCam2.Hide();
            this.LayoutCam3.Hide();
        }

        private void ShowHideCamera(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!this.Dispatcher.HasShutdownStarted && !this.Dispatcher.HasShutdownFinished)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    var cvm = sender as CameraViewModel;
                    if (cvm == ((MainViewModel)DataContext).VMCamera1)
                    {
                        ShowHideCamera(this.LayoutCam1, ((MainViewModel)DataContext).VMCamera1, e);
                    }
                    else if (cvm == ((MainViewModel)DataContext).VMCamera2)
                    {
                        ShowHideCamera(this.LayoutCam2, ((MainViewModel)DataContext).VMCamera2, e);
                    }
                    else if (cvm == ((MainViewModel)DataContext).VMCamera3)
                    {
                        ShowHideCamera(this.LayoutCam3, ((MainViewModel)DataContext).VMCamera3, e);
                    }
                }));
            }
        }

        private void ShowHideCamera(LayoutAnchorable iCameraLayoutControl, CameraViewModel iCVM, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsActive")
            {
                if (iCVM.IsActive)
                {
                    iCameraLayoutControl.Show();
                }
                else
                {
                    iCameraLayoutControl.Hide();
                }
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
