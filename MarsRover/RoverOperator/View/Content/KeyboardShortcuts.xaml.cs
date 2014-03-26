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
    /// Interaction logic for KeyboardShortcuts.xaml
    /// </summary>
    public partial class KeyboardShortcuts : UserControl
    {
        public KeyboardShortcuts()
        {
            InitializeComponent();
            var ksvm = new KeyboardShortcutsViewModel();
            DataContext = ksvm;

            this.ApplicationListView.ItemsSource = ksvm.AppShortcuts;
            this.MainListView.ItemsSource = ksvm.MainPageShortcuts;
        }
    }
}
