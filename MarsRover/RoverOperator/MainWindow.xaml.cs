using FirstFloor.ModernUI.Windows.Controls;
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
using RoverOperator.Content;

namespace RoverOperator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private ICommand goToPageCommand;
        public ICommand GoToPageCommand
        {
            get
            {
                if(goToPageCommand == null)
                {
                    goToPageCommand = new FirstFloor.ModernUI.Presentation.RelayCommand(
                        p => this.GoToPage(p));
                }

                return goToPageCommand;
            }
        }

        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            FirstFloor.ModernUI.Presentation.AppearanceManager.Current.AccentColor = Color.FromRgb(0xa2, 0x00, 0x25);
            AddKeyBoardShortcuts();
            var mwVM = new MainWindowViewModel();
            ViewModelManager.Instance.MainWindowVM = mwVM;
            DataContext = mwVM;
           
        }

        private void AddKeyBoardShortcuts()
        {
            //Settings
            var kb = new KeyBinding(GoToPageCommand, Key.F12, ModifierKeys.None);
            kb.CommandParameter = @"/View/Pages/Help.xaml";
            this.InputBindings.Add(kb);

            //Help
            kb = new KeyBinding(GoToPageCommand, Key.F11, ModifierKeys.None);
            kb.CommandParameter = @"/View/Pages/Settings.xaml";
            this.InputBindings.Add(kb);

            //Home
            kb = new KeyBinding(GoToPageCommand, Key.H, ModifierKeys.Control);
            kb.CommandParameter = @"/View/Pages/Main.xaml";
            this.InputBindings.Add(kb);            

            //Log
            kb = new KeyBinding(GoToPageCommand, Key.L, ModifierKeys.Control);
            kb.CommandParameter = @"/View/Pages/Log.xaml";
            this.InputBindings.Add(kb);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);            
            Application.Current.Shutdown();
        }

        private void GoToPage(object relativeURI)
        {
            var targetURI = relativeURI as String;
            this.ContentSource = new Uri(targetURI, UriKind.Relative);
        }
    }
}
