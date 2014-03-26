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
            FrontLeftMotor.DataContext = MotorsViewModel.FrontLeftMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.FrontLeft);
            FrontRightMotor.DataContext = MotorsViewModel.FrontRightMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.FrontRight);
            MiddleLeftMotor.DataContext = MotorsViewModel.MiddleLeftMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.MiddleLeft);
            MiddleRightMotor.DataContext = MotorsViewModel.MiddleRightMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.MiddleRight);
            BackLeftMotor.DataContext = MotorsViewModel.BackLeftMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.BackLeft);
            BackRightMotor.DataContext = MotorsViewModel.BackRightMotorVM = new MotorStatusViewModel(MarsRover.Motor.Location.BackRight);

            MotorsViewModel.MotorVMActive = true;

            DataContext = mvm;
        }
    }
}
