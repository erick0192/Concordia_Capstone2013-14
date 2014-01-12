using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml.Linq;
using AForge.Video;
using MarsRover.Streams;

namespace MarsRoverClient.Content
{    
    /// <summary>
    /// Interaction logic for CameraView.xaml
    /// </summary>
    public partial class CameraView : UserControl
    {
        public int ControlExtraWidth {get; set;}
        public int ControlExtraHeight { get; set; }
        public int CollapsedWidth { get; set; }
        public int CollapsedHeight { get; set; }
        public int ExpandedWidth { get; set; }
        public int ExpandedHeight { get; set; }

        public CameraView() 
        {
            InitializeComponent();
            DataContext = new CameraViewModel();   
        }   
    }
}
