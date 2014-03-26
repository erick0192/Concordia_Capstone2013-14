using System.ComponentModel;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace RoverOperator.Content
{
    public class NetworkSettingsViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Properties
        
        private string roverIPAddress;
        public string RoverIPAddress
        {
            get
            {
                return roverIPAddress;
            }
            set
            {
                roverIPAddress = value;              
            }
        }

        public int CameraPort1 { get; set; }
        public int CameraPort2 { get; set; }
        public int CameraPort3 { get; set; }
        public int StatusUpdatePort { get; set; }
        public int CommandsPort { get; set; }

        //Validation for Network Properties
        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string property]
        {
            get
            {
                string result = null;
                
                if (property == "RoverIPAddress") {
                    if (!IPAddressIsValid(roverIPAddress))
                    {
                        result = "Invalid IP Address";
                    }
                }

                return result;
            }
        }

        #endregion

        #region Commands

        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(p => this.Save());
                }
                return saveCommand;
            }
        }

        private ICommand undoCommand;
        public ICommand UndoCommand
        {
            get
            {
                if(undoCommand == null)
                {
                    undoCommand = new RelayCommand(p => this.Undo());
                }
                return undoCommand;
            }           
        }

        private ICommand defaultCommand;
        public ICommand DefaultCommand
        {
            get
            {
                if(defaultCommand == null)
                {
                    defaultCommand = new RelayCommand(p => this.Default());
                }
                return defaultCommand;
            }
        }

        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public NetworkSettingsViewModel()
        {
            Undo();
        }

        #endregion

        #region Command Methods

        private void Save()
        {
            Properties.NetworkSettings.Default.RoverIPAddress = RoverIPAddress;
            Properties.NetworkSettings.Default.CameraPort1 = CameraPort1;
            Properties.NetworkSettings.Default.CameraPort2 = CameraPort2;
            Properties.NetworkSettings.Default.CameraPort3 = CameraPort3;
            Properties.NetworkSettings.Default.StatusUpdatePort = StatusUpdatePort;
            Properties.NetworkSettings.Default.CommandsPort = CommandsPort;

            Properties.NetworkSettings.Default.Save();
        }

        private void Undo()
        {
            RoverIPAddress = Properties.NetworkSettings.Default.RoverIPAddress;
            CameraPort1 = Properties.NetworkSettings.Default.CameraPort1;
            CameraPort2 = Properties.NetworkSettings.Default.CameraPort2;
            CameraPort3 = Properties.NetworkSettings.Default.CameraPort3;
            StatusUpdatePort = Properties.NetworkSettings.Default.StatusUpdatePort;
            CommandsPort = Properties.NetworkSettings.Default.CommandsPort;

            FirePropertiesChanged();
        }

        private void Default()
        {
            Properties.NetworkSettings.Default.Reset();
            Properties.NetworkSettings.Default.Save();
            Undo();
        }

        private void FirePropertiesChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("RoverIPAddress"));
                PropertyChanged(this, new PropertyChangedEventArgs("CameraPort1"));
                PropertyChanged(this, new PropertyChangedEventArgs("CameraPort2"));
                PropertyChanged(this, new PropertyChangedEventArgs("CameraPort3"));
                PropertyChanged(this, new PropertyChangedEventArgs("StatusUpdatePort"));
                PropertyChanged(this, new PropertyChangedEventArgs("CommandsPort"));
            }
        }

        private bool IPAddressIsValid(string IPAddress)
        {
            Match match = Regex.Match(roverIPAddress, @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
            if (match.Success) return true;
            return false;
        }

        #endregion
    

    }
}
