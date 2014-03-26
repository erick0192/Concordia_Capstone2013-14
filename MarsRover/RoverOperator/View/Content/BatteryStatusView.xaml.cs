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
using MarsRover;

namespace RoverOperator.Content
{
    /// <summary>
    /// Interaction logic for BatteryStatusView.xaml
    /// </summary>
    public partial class BatteryStatusView : UserControl
    {
        public BatteryStatusView()
        {
            InitializeComponent();
            DataContext = new BatteryStatusViewModel();
            RegisterForBatteryCellsStatusChanges();
        }

        private void RegisterForBatteryCellsStatusChanges()
        {
            var vm = DataContext as BatteryStatusViewModel;
            vm.Battery.Cells.ForEach(cell =>
            {
                cell.NormalVoltageDetected += new BatteryCell.NormalVoltageDetectedDelegate(HandleBatteryCellNormalVoltage);
                cell.OverVoltageDetected += new BatteryCell.OverVoltageDetectedDelegate(HandleBatteryCellOverVoltage);
                cell.UnderVoltageDetected += new BatteryCell.UnderVoltageDetectedDelegate(HandleBatteryCellUnderVoltage);
                cell.WarningVoltageDetected += new BatteryCell.WarningVoltageDetectedDelegate(HandleBatteryCellWarningVoltage);
            });
        }   
  
        private void HandleBatteryCellNormalVoltage(BatteryCell cell)
        {
            if (!this.Dispatcher.HasShutdownStarted && !this.Dispatcher.HasShutdownFinished)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    var cellBar = this.FindName("cellBar" + cell.CellID) as ProgressBar;
                    cellBar.Foreground = Brushes.Lime;
                    var cellPanel = this.FindName("cellPanel" + cell.CellID) as StackPanel;
                    cellPanel.ToolTip = null;
                }));
            }
        }

        private void HandleBatteryCellOverVoltage(BatteryCell cell)
        {
            if (!this.Dispatcher.HasShutdownStarted && !this.Dispatcher.HasShutdownFinished)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    var cellBar = this.FindName("cellBar" + cell.CellID) as ProgressBar;
                    cellBar.Foreground = Brushes.Red;
                    var cellPanel = this.FindName("cellPanel" + cell.CellID) as StackPanel;
                    cellPanel.ToolTip = "This battery cell is currently experiencing over voltage.";
                }));
            }
        }

        private void HandleBatteryCellUnderVoltage(BatteryCell cell)
        {
            if (!this.Dispatcher.HasShutdownStarted && !this.Dispatcher.HasShutdownFinished)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    var cellBar = this.FindName("cellBar" + cell.CellID) as ProgressBar;
                    cellBar.Foreground = Brushes.DarkRed;
                    var cellPanel = this.FindName("cellPanel" + cell.CellID) as StackPanel;
                    cellPanel.ToolTip = "This battery cell is currently experiencing under voltage.";
                }));
            }
        }

        private void HandleBatteryCellWarningVoltage(BatteryCell cell)
        {
            if (!this.Dispatcher.HasShutdownStarted && !this.Dispatcher.HasShutdownFinished)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    var cellBar = this.FindName("cellBar" + cell.CellID) as ProgressBar;
                    cellBar.Foreground = Brushes.OrangeRed;
                    var cellPanel = this.FindName("cellPanel" + cell.CellID) as StackPanel;
                    cellPanel.ToolTip = "This battery cell is currently operating at a non-ideal voltage.";
                }));
            }
        }
    }
}
