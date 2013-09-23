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
using MRClient_ModernUIProtoss.Content;

namespace MRClient_ModernUIProtoss.Pages
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        public Main()
        {
            InitializeComponent();
            InitKeyBindings();
        }

        public void InitKeyBindings()
        {
            KeyBinding OpenCmdKeyBinding = new KeyBinding(
            ApplicationCommands.Open,
            Key.R,
            ModifierKeys.Control);

            this.InputBindings.Add(OpenCmdKeyBinding);
        }
    }
}
