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
using FirstFloor.ModernUI.Windows;
using MarsRoverClient.Content;

namespace MarsRoverClient.Pages
{
    /// <summary>
    /// Interaction logic for Log.xaml
    /// </summary>
    public partial class Log : UserControl
    {
        public Log()
        {
            InitializeComponent();
            LogViewModel lvm = new LogViewModel();            
            DataContext = lvm;

            lvm.LogListVM = new LogListViewModel();
            this.logList.DataContext = lvm.LogListVM;
        }
    }
}
