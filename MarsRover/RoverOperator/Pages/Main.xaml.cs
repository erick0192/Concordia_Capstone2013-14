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

        private ICommand toggleCameraCommand;
        public ICommand ToggleCameraCommand
        {
            get
            {
                if (toggleCameraCommand == null)
                {
                    toggleCameraCommand = new RelayCommand(
                        p => this.ToggleCamera(p));
                }

                return toggleCameraCommand;
            }
        }

        public Main()
        {
            InitializeComponent();
            AddKeyBoardShortcuts();
            DataContext = new MainViewModel();

            ((MainViewModel)DataContext).DockingManager = dockingManager;
            

            //Instantiate VM for camera views            
            CameraViewModel cvm = this.Cam1.DataContext as CameraViewModel;
            //cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "1";
            cvm.VideoSource = VideoStreamReceiverManager.Instance.Camera1;
            
            ((MainViewModel)DataContext).VMCamera1 = cvm;
            
            cvm = this.Cam2.DataContext as CameraViewModel;
            //cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "2";
            cvm.VideoSource = VideoStreamReceiverManager.Instance.Camera2;
            ((MainViewModel)DataContext).VMCamera2 = cvm;
            
            cvm = this.Cam3.DataContext as CameraViewModel;
            //cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "3";
            cvm.VideoSource = VideoStreamReceiverManager.Instance.Camera3;       
            ((MainViewModel)DataContext).VMCamera3 = cvm;          

            //Need to refactor this!
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(((MainViewModel)DataContext).MainIsVisibleChanged);
            
            this.FocusVisualStyle = new Style();//Get rid of dotted rectangle that indicates its focused    
           
            this.LayoutCam1.Hide();
            this.LayoutCam2.Hide();
            this.LayoutCam3.Hide();
            
            this.LayoutCam1.IsVisibleChanged += new EventHandler(HandleUIHide);
            this.LayoutCam2.IsVisibleChanged += new EventHandler(HandleUIHide);
            this.LayoutCam3.IsVisibleChanged += new EventHandler(HandleUIHide);            
        }

        private void AddKeyBoardShortcuts()
        {
            //Camera 1
            var kb = new KeyBinding(ToggleCameraCommand, Key.D1, ModifierKeys.Control);
            kb.CommandParameter = "1";
            this.InputBindings.Add(kb);

            //Camera 2
            kb = new KeyBinding(ToggleCameraCommand, Key.D2, ModifierKeys.Control);
            kb.CommandParameter = "2";
            this.InputBindings.Add(kb);
            
            //Camera 3
            kb = new KeyBinding(ToggleCameraCommand, Key.D3, ModifierKeys.Control);
            kb.CommandParameter = "3";
            this.InputBindings.Add(kb);
          
        }

        private void ToggleCamera(object cameraID)
        {
            var camID = cameraID as String;
            var layoutCam = this.FindName("LayoutCam" + camID) as LayoutAnchorable;
            
            if(layoutCam.IsVisible)
            {
                layoutCam.Hide();
            }
            else
            {
                layoutCam.Show();
            }
        }

        private void HandleUIHide(object sender, EventArgs e)
        {
            var layoutCam = sender as LayoutAnchorable;
            CameraViewModel cvm = null;
            if(layoutCam == LayoutCam1)
            {
                cvm = ((MainViewModel)DataContext).VMCamera1;
            }
            else if (layoutCam == LayoutCam2)
            {
                cvm = ((MainViewModel)DataContext).VMCamera2;
            }
            else if (layoutCam == LayoutCam3)
            {
                cvm = ((MainViewModel)DataContext).VMCamera3;
            }
            
            if (cvm.ToggleCamera.CanExecute(null))
                cvm.ToggleCamera.Execute(null);
                       
        }

        //private void ShowHideCamera(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (!this.Dispatcher.HasShutdownStarted && !this.Dispatcher.HasShutdownFinished)
        //    {
        //        this.Dispatcher.Invoke((Action)(() =>
        //        {
        //            var cvm = sender as CameraViewModel;
        //            if (cvm == ((MainViewModel)DataContext).VMCamera1)
        //            {
        //                ShowHideCamera(this.LayoutCam1, ((MainViewModel)DataContext).VMCamera1, e);
        //            }
        //            else if (cvm == ((MainViewModel)DataContext).VMCamera2)
        //            {
        //                ShowHideCamera(this.LayoutCam2, ((MainViewModel)DataContext).VMCamera2, e);
        //            }
        //            else if (cvm == ((MainViewModel)DataContext).VMCamera3)
        //            {
        //                ShowHideCamera(this.LayoutCam3, ((MainViewModel)DataContext).VMCamera3, e);
        //            }
        //        }));
        //    }
        //}

        //private void ShowHideCamera(LayoutAnchorable iCameraLayoutControl, CameraViewModel iCVM, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "IsActive")
        //    {
        //        if (iCVM.IsActive)
        //        {
        //            iCameraLayoutControl.Show();
        //        }
        //        else
        //        {                    
        //            iCameraLayoutControl.Hide();
        //        }
        //    }
        //}

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
