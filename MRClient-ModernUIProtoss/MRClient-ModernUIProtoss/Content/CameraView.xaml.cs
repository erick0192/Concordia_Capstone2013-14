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

namespace MRClient_ModernUIProtoss.Content
{    
    /// <summary>
    /// Interaction logic for CameraView.xaml
    /// </summary>
    public partial class CameraView : UserControl
    {
        //public static readonly DependencyProperty CameraNameProperty = DependencyProperty.Register
        //    (
        //         "Title",
        //         typeof(string),
        //         typeof(CameraView),
        //         new PropertyMetadata(string.Empty)
        //    );

        //public string Title
        //{
        //    get { return (string)GetValue(CameraNameProperty); }
        //    set { SetValue(CameraNameProperty, value); }
        //}

        public CameraView() 
        {
            InitializeComponent();
            DataContext = new CameraViewModel("Camera");            
        }             
    }
}
