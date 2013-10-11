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
            cvm.CameraName = "Front";
            cvm.VideoSource = WebCamStreamManager.Instance.GetFrontCameraStream();
            ((MainViewModel)DataContext).UpperLeftCameraVM = cvm;
            
            cvm = this.camBack.DataContext as CameraViewModel;
            cvm.CameraName = "Back";
            cvm.VideoSource = WebCamStreamManager.Instance.GetBackCameraStream();
            ((MainViewModel)DataContext).UpperRightCameraVM =cvm;
            
            cvm = this.camLeft.DataContext as CameraViewModel;
            cvm.CameraName = "Left";
            cvm.VideoSource = WebCamStreamManager.Instance.GetLeftCameraStream();       
            ((MainViewModel)DataContext).LowerLeftCameraVM = cvm;
            
            cvm = this.camRight.DataContext as CameraViewModel;
            cvm.CameraName = "Right";
            cvm.VideoSource = WebCamStreamManager.Instance.GetRightCameraStream();
            ((MainViewModel)DataContext).LowerRightCameraVM = cvm;

            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(((MainViewModel)DataContext).MainIsVisibleChanged);
            this.FocusVisualStyle = new Style();//Get rid of dotted rectangle that indicates its focused    

            this.layoutRightCam.Hide();
            this.layoutLeftCam.Hide();
        }       
    }
}
