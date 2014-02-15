using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RoverOperator.Content;
using System.IO;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using System.ComponentModel;
using System.Windows;
using System.Net.NetworkInformation;
using System.Threading;

namespace RoverOperator.Pages
{
    public class MainViewModel : Window, INotifyPropertyChanged
    {
        #region Delegates

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Attributes

        private string connectedToRover;
        public string ConnectedToRover
        {
            get { return connectedToRover; }
            set
            {
                if (!value.Equals(connectedToRover))
                {
                    connectedToRover = value;
                    OnPropertyChanged("ConnectedToRover");
                }
            }
        }

        private string pingRTT;
        public string PingRTT
        {
            get { return pingRTT; }
            set
            {
                if (!value.Equals(pingRTT))
                {
                    pingRTT = value;
                    OnPropertyChanged("PingRTT");
                }
            }
        }     

        private bool isCamActive2;
        private bool isCamActive3;


        #endregion

        #region Properties

        public Xceed.Wpf.AvalonDock.DockingManager DockingManager { get; set; }
        public CameraViewModel VMCamera1 { get; set; }
        public CameraViewModel VMCamera2 { get; set; }
        public CameraViewModel VMCamera3 { get; set; }

        #endregion

        #region Commands

        //Command to hide and turn off / show and turn on camera
        private ICommand mToggleCameraCommand;        
        public ICommand ToggleCameraCommand
        {
            get
            {
                if (mToggleCameraCommand == null)
                {
                    mToggleCameraCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.ToggleCamera(p),
                        p => this.CanToggleCamera(p));
                }
                return mToggleCameraCommand;
            }
        }

        private ICommand mLoadLayoutCommand;
        public ICommand LoadLayoutCommand
        {
            get
            {
                if (mLoadLayoutCommand == null)
                {
                    mLoadLayoutCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => LoadLayout(p),
                        p => CanLoadLayout(p));
                }
                return mLoadLayoutCommand;
            }
        }

        private ICommand mSaveLayoutCommand;
        public ICommand SaveLayoutCommand
        {
            get
            {
                if (mSaveLayoutCommand == null)
                {
                    mSaveLayoutCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => SaveLayout(p),
                        p => CanSaveLayout(p));
                }
                return mSaveLayoutCommand;
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            StartPinging(Properties.NetworkSettings.Default.RoverIPAddress);
        }

        #endregion

        #region Command Methods

        protected bool CanToggleCamera(object iParam)
        {
            CameraViewModel cvm = null;
            var camNum = iParam as string;

            if ("1" == camNum)
            {                
                cvm = VMCamera1;
            }
            else if ("2" == camNum)
            {
                cvm = VMCamera2;
            }
            else if ("3" == camNum)
            {
                cvm = VMCamera3;
            }

            return cvm.ToggleCamera.CanExecute(null) ;
        }

        protected void ToggleCamera(object iParam)
        {
            CameraViewModel cvm = null;
            var camNum = iParam as string;

            if ("1" == camNum)
            {
                cvm = VMCamera1;
            }
            else if ("2" == camNum)
            {
                cvm = VMCamera2;
            }
            else if ("3" == camNum)
            {
                cvm = VMCamera3;
            }

            cvm.ToggleCamera.Execute(null);
        }

        protected bool CanLoadLayout(object iParam)
        {
            return File.Exists(@".\AvalonDock." + iParam + ".Layout.config");
        }

        protected void LoadLayout(object iParam)
        {
            var layoutSerializer = new XmlLayoutSerializer(DockingManager);
            layoutSerializer.Deserialize(@".\AvalonDock." + iParam + ".Layout.config");
        }

        protected bool CanSaveLayout(object iParam)
        {
            return true;
        }

        protected void SaveLayout(object iParam)
        {
            var layoutSerializer = new XmlLayoutSerializer(DockingManager);
            layoutSerializer.Serialize(@".\AvalonDock." + iParam + ".Layout.config");
        }

        #endregion 

        #region Methods

        private void StartPinging(string host)
        {
            Thread t = new Thread(() => Ping(host));
            t.IsBackground = true;
            t.Start();
        }

        private void Ping(string host)
        {
            bool pingable = false;
            Ping pinger = new Ping();

            try
            {
                PingReply reply = pinger.Send(host);
                pingable = reply.Status == IPStatus.Success;
                PingRTT = "Ping: " + reply.RoundtripTime + " ms";
            }
            catch (PingException) { }

            if (pingable)
            {
                ConnectedToRover = "Connected";
            }
            else
            {
                ConnectedToRover = "Unable to connect";
            }
            Thread.Sleep(200);
        }

        #endregion

        #region Event Handlers

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
