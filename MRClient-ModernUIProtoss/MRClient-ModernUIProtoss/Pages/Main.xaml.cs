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

            //Instantiate VM for camera views
            CameraViewModel cvm = new CameraViewModel("Front");
            this.camFront.DataContext = cvm;
            ((MainViewModel)DataContext).UpperLeftCameraVM = cvm;

            cvm = new CameraViewModel("Back");
            this.camBack.DataContext = cvm;
            ((MainViewModel)DataContext).UpperRightCameraVM =cvm;

            cvm = new CameraViewModel("Left");
            this.camLeft.DataContext = cvm;
            ((MainViewModel)DataContext).LowerLeftCameraVM = cvm;

            cvm = new CameraViewModel("Right");
            this.camRight.DataContext = cvm;
            ((MainViewModel)DataContext).LowerRightCameraVM = cvm;

            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(((MainViewModel)DataContext).MainIsVisibleChanged);
            this.FocusVisualStyle = new Style();//Get rid of dotted rectangle that indicates its focused
        }
    }
}
