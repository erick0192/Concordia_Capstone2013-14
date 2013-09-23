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
        public string Heading { get; set; }
        private bool mIsActive;
        public bool IsActive 
        { 
            get { return mIsActive; }
            set
            {
                mIsActive = value;
                if (mIsActive)
                {
                    
                }
            }
        }

        public CameraView() : this("View")
        {
            
        }

        public CameraView(string iHeading)
        {
            InitializeComponent();
            DataContext = this;
            Heading = iHeading;
            IsActive = true;
        }
    }
}
