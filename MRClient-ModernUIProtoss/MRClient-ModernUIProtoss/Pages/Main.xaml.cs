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
            CameraViewModel cvm = this.camFront.DataContext as CameraViewModel;
            cvm.CameraName = "Front";
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HandleFrontExpanded);
            ((MainViewModel)DataContext).UpperLeftCameraVM = cvm;

            cvm = this.camBack.DataContext as CameraViewModel;
            cvm.CameraName = "Back";
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HandleBackExpanded);
            ((MainViewModel)DataContext).UpperRightCameraVM =cvm;

            cvm = this.camLeft.DataContext as CameraViewModel;
            cvm.CameraName = "Left";
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HandleLeftExpanded);            
            ((MainViewModel)DataContext).LowerLeftCameraVM = cvm;

            cvm = this.camRight.DataContext as CameraViewModel;
            cvm.CameraName = "Right";
            cvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HandleRightExpanded);
            ((MainViewModel)DataContext).LowerRightCameraVM = cvm;

            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(((MainViewModel)DataContext).MainIsVisibleChanged);
            this.FocusVisualStyle = new Style();//Get rid of dotted rectangle that indicates its focused
        }

        private void HandleFrontExpanded(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsExpanded")
            {
                if (((MainViewModel)DataContext).UpperLeftCameraVM.IsExpanded)
                {
                    this.camBack.Visibility = System.Windows.Visibility.Collapsed;
                    this.camLeft.Visibility = System.Windows.Visibility.Collapsed;
                    this.camRight.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    this.camBack.Visibility = System.Windows.Visibility.Visible;
                    this.camLeft.Visibility = System.Windows.Visibility.Visible;
                    this.camRight.Visibility = System.Windows.Visibility.Visible;
                }
            }
           
        }

        private void HandleBackExpanded(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsExpanded")
            {
                if (((MainViewModel)DataContext).UpperRightCameraVM.IsExpanded)
                {
                    this.camFront.Visibility = System.Windows.Visibility.Collapsed;
                    this.camLeft.Visibility = System.Windows.Visibility.Collapsed;
                    this.camRight.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    this.camFront.Visibility = System.Windows.Visibility.Visible;
                    this.camLeft.Visibility = System.Windows.Visibility.Visible;
                    this.camRight.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void HandleLeftExpanded(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsExpanded")
            {
                if (((MainViewModel)DataContext).LowerLeftCameraVM.IsExpanded)
                {
                    this.camBack.Visibility = System.Windows.Visibility.Collapsed;
                    this.camFront.Visibility = System.Windows.Visibility.Collapsed;
                    this.camRight.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    this.camBack.Visibility = System.Windows.Visibility.Visible;
                    this.camFront.Visibility = System.Windows.Visibility.Visible;
                    this.camRight.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void HandleRightExpanded(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsExpanded")
            {
                if (((MainViewModel)DataContext).LowerRightCameraVM.IsExpanded)
                {
                    this.camBack.Visibility = System.Windows.Visibility.Collapsed;
                    this.camLeft.Visibility = System.Windows.Visibility.Collapsed;
                    this.camFront.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    this.camBack.Visibility = System.Windows.Visibility.Visible;
                    this.camLeft.Visibility = System.Windows.Visibility.Visible;
                    this.camFront.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }
    }
}
