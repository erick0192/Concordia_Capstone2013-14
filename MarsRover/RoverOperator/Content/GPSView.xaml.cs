using System.Windows.Controls;

namespace RoverOperator.Content
{
    /// <summary>
    /// Interaction logic for GPSView.xaml
    /// </summary>
    public partial class GPSView : UserControl
    {
       
        GPSViewViewModel gpsVM;

        public GPSView()
        {
            InitializeComponent();

            //instantiate viewmodel and set datacontext
            gpsVM = new GPSViewViewModel();
            this.DataContext = gpsVM;
        }
    }
}
