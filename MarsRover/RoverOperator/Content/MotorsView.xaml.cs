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

namespace RoverOperator.Content
{
    /// <summary>
    /// Interaction logic for MotorsView.xaml
    /// </summary>
    public partial class MotorsView : UserControl
    {
        public MotorsView()
        {
            InitializeComponent();

            var mvm = new MotorsViewModel();
            FrontLeftMotor.DataContext = mvm.FrontLeftMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.FrontLeft);
            FrontRightMotor.DataContext = mvm.FrontRightMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.FrontRight);
            MiddleLeftMotor.DataContext = mvm.MiddleLeftMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.MiddleLeft);
            MiddleRightMotor.DataContext = mvm.MiddleRightMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.MiddleRight);
            BackLeftMotor.DataContext = mvm.BackLeftMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.BackLeft);
            BackRightMotor.DataContext = mvm.BackRightMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.BackRight);

            DataContext = mvm;
        }
    }
}
