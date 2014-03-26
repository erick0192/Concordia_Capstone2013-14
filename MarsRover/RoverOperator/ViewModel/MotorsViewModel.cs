using System.ComponentModel;

namespace RoverOperator.Content
{
    public class MotorsViewModel : INotifyPropertyChanged
    {
        #region Properties

        public static bool MotorVMActive { get; set; }

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

        public static MotorStatusViewModel FrontLeftMotorVM { get; set; }
        public static MotorStatusViewModel FrontRightMotorVM { get; set; }
        public static MotorStatusViewModel MiddleLeftMotorVM { get; set; }
        public static MotorStatusViewModel MiddleRightMotorVM { get; set; }
        public static MotorStatusViewModel BackLeftMotorVM { get; set; }
        public static MotorStatusViewModel BackRightMotorVM { get; set; }

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
