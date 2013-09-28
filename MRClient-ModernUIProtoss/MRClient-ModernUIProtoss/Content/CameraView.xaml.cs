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
using System.Xml.Linq;

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

            CollapsedWidth = 300;
            CollapsedHeight = 300;

            ExpandedWidth = CollapsedWidth * 2;
            ExpandedHeight = CollapsedHeight * 2;

            ControlExtraHeight = 30;
            ControlExtraWidth = 10;
            
            InitializeComponent();
            DataContext = new CameraViewModel("Camera");
            ((CameraViewModel)DataContext).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HandleViewExpanded);
        }

        private void HandleViewExpanded(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsExpanded")
            {
                if (((CameraViewModel)DataContext).IsExpanded)
                {
                    ExpandView();
                }
                else
                {
                    CollapseView();
                }

            }
        }

        private void btnCollapse_Click(object sender, RoutedEventArgs e)
        {
           
            if (((CameraViewModel)DataContext).CollapseViewCommand.CanExecute(null))
            {
                ((CameraViewModel)DataContext).CollapseViewCommand.Execute(null);
                CollapseView();
            }
           
        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
           
            if (((CameraViewModel)DataContext).ExpandViewCommand.CanExecute(null))
            {
                ((CameraViewModel)DataContext).ExpandViewCommand.Execute(null);
                ExpandView();
            }
            
        }

        private void ExpandView()
        {
            this.btnExpand.Visibility = System.Windows.Visibility.Collapsed;
            this.btnCollapse.Visibility = System.Windows.Visibility.Visible;

            this.Width = ExpandedWidth + ControlExtraWidth * 2;
            this.Height = ExpandedHeight + ControlExtraHeight;

            this.panelStream.Width = ExpandedWidth;
            this.panelStream.Height = ExpandedHeight;
        }

        private void CollapseView()
        {
            this.btnCollapse.Visibility = System.Windows.Visibility.Collapsed;
            this.btnExpand.Visibility = System.Windows.Visibility.Visible;

            this.Width = CollapsedWidth + ControlExtraWidth;
            this.Height = CollapsedHeight + ControlExtraHeight;

            this.panelStream.Width = CollapsedWidth;
            this.panelStream.Height = CollapsedHeight;
        }
          
    }
}
