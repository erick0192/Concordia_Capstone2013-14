using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MRClient_ModernUIProtoss.Content
{
    class CameraViewModel : INotifyPropertyChanged
    {
        private ICommand mToggleCamera;
        public ICommand ToggleCamera
        {
            get
            {
                if (mToggleCamera == null)
                {
                    mToggleCamera = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.ToggleCam(),
                        p => this.CanToggleCamera());
                }
                return mToggleCamera;
            }
        }

        public string CameraName { get; set; }
        private bool asdf;
        public bool IsActive
        {
            get { return asdf; }
            set { asdf = value; OnPropertyChanged(""); }
        }

        public CameraViewModel()
        {
            IsActive = true;
        }

        public bool CanToggleCamera() { return true; }

        public void ToggleCam()
        {
            //System.Windows.MessageBox.Show("Toggling Camera");
            IsActive = !IsActive;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {            
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
