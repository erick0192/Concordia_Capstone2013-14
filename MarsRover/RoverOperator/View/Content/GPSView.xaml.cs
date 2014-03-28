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
            gpsVM = new GPSViewViewModel(this.Dispatcher);
            gpsVM.map = myMap;
            this.DataContext = gpsVM;            
        }

        //private void RefreshMap()
        //{
        //    myMap.UpdateLayout();
        //    var c = myMap.Center;
        //    c.Latitude += 0.00001;
        //    myMap.SetView(c, myMap.ZoomLevel);
        //}
    }
}
