﻿using System;
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
            FrontLeftMotor.DataContext = new MotorStatusViewModel(MarsRover.Motor.Location.FrontLeft);
            FrontRightMotor.DataContext = new MotorStatusViewModel(MarsRover.Motor.Location.FrontRight);
            MiddleLeftMotor.DataContext = new MotorStatusViewModel(MarsRover.Motor.Location.MiddleLeft);
            MiddleRightMotor.DataContext = new MotorStatusViewModel(MarsRover.Motor.Location.MiddleRight);
            BackLeftMotor.DataContext = new MotorStatusViewModel(MarsRover.Motor.Location.BackLeft);
            BackRightMotor.DataContext = new MotorStatusViewModel(MarsRover.Motor.Location.BackRight);
        }
    }
}
