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
using MarsRoverClient.Content;
using Xceed.Wpf.AvalonDock.Layout;

namespace MarsRoverClient.Pages
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
            CameraViewModel cvm = this.camFront.DataContext as CameraViewModel;
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "Front";
            cvm.VideoSource = WebCamStreamManager.Instance.GetFrontCameraStream();
            
            ((MainViewModel)DataContext).VMFrontCamera = cvm;
            
            cvm = this.camBack.DataContext as CameraViewModel;
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "Back";
            cvm.VideoSource = WebCamStreamManager.Instance.GetBackCameraStream();
            ((MainViewModel)DataContext).VMBackCamera = cvm;
            
            cvm = this.camLeft.DataContext as CameraViewModel;
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "Left";
            cvm.VideoSource = WebCamStreamManager.Instance.GetLeftCameraStream();       
            ((MainViewModel)DataContext).VMLeftCamera = cvm;
            
            cvm = this.camRight.DataContext as CameraViewModel;
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "Right";
            cvm.VideoSource = WebCamStreamManager.Instance.GetRightCameraStream();
            ((MainViewModel)DataContext).VMRightCamera = cvm;

            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(((MainViewModel)DataContext).MainIsVisibleChanged);
            this.FocusVisualStyle = new Style();//Get rid of dotted rectangle that indicates its focused    

            //Start or hide the cameras depending on configuration
            //((MainViewModel)DataContext).VMFrontCamera.VideoSource.Start();
            this.layoutFrontCam.Hide();
            ((MainViewModel)DataContext).VMBackCamera.VideoSource.Start();
            //this.layoutBackCam.Hide();
            this.layoutLeftCam.Hide();
            this.layoutRightCam.Hide();            
        }

        private void ShowHideCamera(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!this.Dispatcher.HasShutdownStarted && !this.Dispatcher.HasShutdownFinished)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    var cvm = sender as CameraViewModel;
                    if (cvm == ((MainViewModel)DataContext).VMFrontCamera)
                    {
                        ShowHideCamera(this.layoutFrontCam, ((MainViewModel)DataContext).VMFrontCamera, e);
                    }
                    else if (cvm == ((MainViewModel)DataContext).VMBackCamera)
                    {
                        ShowHideCamera(this.layoutBackCam, ((MainViewModel)DataContext).VMBackCamera, e);
                    }
                    else if (cvm == ((MainViewModel)DataContext).VMLeftCamera)
                    {
                        ShowHideCamera(this.layoutLeftCam, ((MainViewModel)DataContext).VMLeftCamera, e);
                    }
                    else if (cvm == ((MainViewModel)DataContext).VMRightCamera)
                    {
                        ShowHideCamera(this.layoutRightCam, ((MainViewModel)DataContext).VMRightCamera, e);
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
