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
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
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
            DataContext = new MainViewModel();
            ((MainViewModel)DataContext).UpperLeftCameraVM = (CameraViewModel)this.camFront.DataContext;
            ((MainViewModel)DataContext).UpperRightCameraVM = (CameraViewModel)this.camBack.DataContext;
            ((MainViewModel)DataContext).LowerLeftCameraVM = (CameraViewModel)this.camLeft.DataContext;
            ((MainViewModel)DataContext).LowerRightCameraVM = (CameraViewModel)this.camRight.DataContext;
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(((MainViewModel)DataContext).MainIsVisibleChanged);
        }
    }
}
