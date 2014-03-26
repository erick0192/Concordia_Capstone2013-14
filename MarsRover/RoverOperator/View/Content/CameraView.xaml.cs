using System.Windows.Controls;

namespace RoverOperator.Content
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
