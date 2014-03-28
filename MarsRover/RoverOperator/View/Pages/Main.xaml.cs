using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RoverOperator.Content;
using Xceed.Wpf.AvalonDock.Layout;

namespace RoverOperator.Pages
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        private bool isAlreadyHidingCamLayout = false;

        private ICommand toggleGPSCommand;
        public ICommand ToggleGPSCommand
        {
            get
            {
                if (toggleGPSCommand == null)
                {
                    toggleGPSCommand = new RelayCommand(
                        p => this.ToggleGPS());
                }
                return toggleGPSCommand;
            }
        }

        public Main()
        {
            InitializeComponent();            

            var mainVM = new MainViewModel();
            ViewModelManager.Instance.MainWindowVM.MainVM = mainVM;
            DataContext = mainVM;           

            mainVM.DockingManager = dockingManager;           

            //Instantiate VM for camera views            
            CameraViewModel cvm = this.Cam1.DataContext as CameraViewModel;
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "1";
            cvm.VideoSource = VideoStreamReceiverManager.Instance.Camera1;
            mainVM.VMCamera1 = cvm;
            
            cvm = this.Cam2.DataContext as CameraViewModel;
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "2";
            cvm.VideoSource = VideoStreamReceiverManager.Instance.Camera2;
            mainVM.VMCamera2 = cvm;
            
            cvm = this.Cam3.DataContext as CameraViewModel;
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ShowHideCamera);
            cvm.CameraName = "3";
            cvm.VideoSource = VideoStreamReceiverManager.Instance.Camera3;
            mainVM.VMCamera3 = cvm;          

            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(MainIsVisibleChanged);
            
            this.FocusVisualStyle = new Style();//Get rid of dotted rectangle that indicates its focused    
           
            this.LayoutCam1.Hide();
            this.LayoutCam2.Hide();
            this.LayoutCam3.Hide();

            this.LayoutCam1.Hiding += new EventHandler<CancelEventArgs>(HandleHiding);
            this.LayoutCam2.Hiding += new EventHandler<CancelEventArgs>(HandleHiding);
            this.LayoutCam3.Hiding += new EventHandler<CancelEventArgs>(HandleHiding);

            AddKeyBoardShortcuts();
        }

        private void AddKeyBoardShortcuts()
        {
            var mainVM = DataContext as MainViewModel;
            //Camera 1
            var kb = new KeyBinding(mainVM.VMCamera1.ToggleCamera, Key.D1, ModifierKeys.Control);
            kb.CommandParameter = "1";
            this.InputBindings.Add(kb);

            //Camera 2
            kb = new KeyBinding(mainVM.VMCamera2.ToggleCamera, Key.D2, ModifierKeys.Control);
            kb.CommandParameter = "2";
            this.InputBindings.Add(kb);

            //Camera 3
            kb = new KeyBinding(mainVM.VMCamera3.ToggleCamera, Key.D3, ModifierKeys.Control);
            kb.CommandParameter = "3";
            this.InputBindings.Add(kb);

            //GPS
            kb = new KeyBinding(ToggleGPSCommand, Key.G, ModifierKeys.Control);
            this.InputBindings.Add(kb);
        }

        private void ShowHideCamera(object sender, PropertyChangedEventArgs e)
        {
            //MIGHT NEED TO USE THIS CODE IN CASE IT CRASHES SAYING ITS TRYING TO ACCES FROM ANOTHER THREAD
            if (!this.Dispatcher.HasShutdownStarted && !this.Dispatcher.HasShutdownFinished)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (e.PropertyName == "IsActive")
                    {
                        LayoutAnchorable camLayoutAnch = null;
                        var cvm = sender as CameraViewModel;
                        var mainVM = DataContext as MainViewModel;

                        if (cvm == mainVM.VMCamera1)
                        {
                            camLayoutAnch = this.LayoutCam1;
                        }
                        else if (cvm == mainVM.VMCamera2)
                        {
                            camLayoutAnch = this.LayoutCam2;
                        }
                        else if (cvm == mainVM.VMCamera3)
                        {
                            camLayoutAnch = this.LayoutCam3;
                        }

                        if (cvm.IsActive)
                        {
                            camLayoutAnch.Show();
                        }
                        else if(!isAlreadyHidingCamLayout)
                        {
                            //We dont want to try and hide if it was already hidden through the GUI
                            camLayoutAnch.Hide();
                        }
                        else
                        {
                            //Reset for next time
                            isAlreadyHidingCamLayout = false;
                        }
                    }
                }));
            }
        }

        private void HandleHiding(object sender, EventArgs e)
        {
            var camLayoutAnch = sender as LayoutAnchorable;
            var mainVM = DataContext as MainViewModel;
            CameraViewModel cvm = null;

            if (camLayoutAnch == this.LayoutCam1)
            {
                cvm = mainVM.VMCamera1;
            }
            else if (camLayoutAnch == this.LayoutCam2)
            {
                cvm = mainVM.VMCamera2;
            }
            else if (camLayoutAnch == this.LayoutCam3)
            {
                cvm = mainVM.VMCamera3;
            }

            if (cvm.IsActive && camLayoutAnch.IsVisible && cvm.ToggleCamera.CanExecute(null))
            {
                isAlreadyHidingCamLayout = true;
                cvm.ToggleCamera.Execute(null);
            }               
        }

        private void ToggleGPS()
        {
            if (LayoutGPS.IsVisible)
            {
                LayoutGPS.Hide();
            }
            else
            {
                LayoutGPS.Show();
            }
        }

        public void MainIsVisibleChanged(object iSender, System.Windows.DependencyPropertyChangedEventArgs iEventArgs)
        {
            if ((bool)iEventArgs.NewValue == true)
            {
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.ContextIdle,
                    new Action(delegate()
                    {
                        ((Main)iSender).Focus();
                    })
                );
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Setting width in xaml does not work cause Avalondock resizes all layouts after initialization
            //So we must set the width after everything is loaded
            //leftLayoutPanel.DockWidth = new GridLength(360, GridUnitType.Pixel);
        }
    }
}
