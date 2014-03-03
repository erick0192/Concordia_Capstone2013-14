using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RoverOperator.Content
{
    public class MotorsViewModel : INotifyPropertyChanged
    {
        #region Properties

        private bool enablePowerModifiers = false;
        public bool EnablePowerModifiers
        {
            get { return enablePowerModifiers; }
            set
            {
                enablePowerModifiers = value;
                EnablePowerModifier(enablePowerModifiers);
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("EnablePowerModifiers"));
            }
        }

        public MotorStatusViewModel FrontLeftMotorVM { get; set; }
        public MotorStatusViewModel FrontRightMotorVM { get; set; }
        public MotorStatusViewModel MiddleLeftMotorVM { get; set; }
        public MotorStatusViewModel MiddleRightMotorVM { get; set; }
        public MotorStatusViewModel BackLeftMotorVM { get; set; }
        public MotorStatusViewModel BackRightMotorVM { get; set; }

        #endregion 

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor

        public MotorsViewModel()
        {
            
        }

        #endregion

        #region Methods

        private void EnablePowerModifier(bool enable)
        {
            FrontLeftMotorVM.CanModifyPower = enable;
            FrontRightMotorVM.CanModifyPower = enable;
            MiddleLeftMotorVM.CanModifyPower = enable;
            MiddleRightMotorVM.CanModifyPower = enable;
            BackLeftMotorVM.CanModifyPower = enable;
            BackRightMotorVM.CanModifyPower = enable;
        }

        #endregion
    }
}
