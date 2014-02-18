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
using Microsoft.Maps.MapControl.WPF;

namespace RoverOperator.Content
{
    /// <summary>
    /// Interaction logic for GPSView.xaml
    /// </summary>
    public partial class GPSView : UserControl
    {
        GPSViewViewModel gpsVM;

        //keep track of targets on map
        private List<Pushpin> targetPins;

        public GPSView()
        {
            InitializeComponent();

            gpsVM = new GPSViewViewModel();

            //bind events
            myMap.MouseDoubleClick += map_MouseDoubleClick;

            DataContext = this;
        }

        //handle mouse click to add targets
        private void map_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location targetLocation = myMap.ViewportPointToLocation(mousePosition);
            
            addTarget(targetLocation);           
        }

        //Add a target to the map from the text boxes
        private void addTarget(Location location)
        {
            //Initialize target list
            if (targetPins == null)
            {
                targetPins = new List<Pushpin>();
            }

            Location targetLocation = location;

            // The pushpin to add to the map.
            Pushpin targetPin = new Pushpin();
            targetPin.Location = targetLocation;
            
            //Add a tooltip to the pin
            ToolTip tt = new ToolTip();
            tt.Content = targetPin.Location.Longitude + ", " + targetPin.Location.Latitude;
            targetPin.ToolTip = tt;

            //remove target on rightclick event handler
            targetPin.MouseRightButtonUp += targetPin_MouseRightButtonUp;

            // Adds the pushpin to the map.
            myMap.Children.Add(targetPin);

            //Initialize target list
            if (targetPins == null)
            {
                targetPins = new List<Pushpin>();
            }
            targetPins.Add(targetPin);
        }

        /// <summary>
        /// Remove target on right click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void targetPin_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            myMap.Children.Remove((UIElement) sender);
            targetPins.Remove((Pushpin)sender);
        }

        //when you click the 'Remove' button
        private void removeTarget()
        {
            //check both but they should be the same number all the time
            if (myMap.Children.Count > 0 && targetPins.Count > 0)
            {
                myMap.Children.Remove(targetPins.Last());
                targetPins.Remove(targetPins.Last());
            }
        }

        //when you click the 'Remove All' button
        private void removeAllTargets()
        {
            //check both but they should be the same number all the time
            if (myMap.Children.Count > 0 && targetPins.Count > 0)
            {
                foreach (var t in targetPins)
                {
                    myMap.Children.Remove(t);
                    targetPins.Remove(t);
                }
            }
        }

        //Add a marker to the map
        private Ellipse placeGPSPoint(Location location, string name, Color color)
        {
            Ellipse target = new Ellipse();

            target.Fill = new SolidColorBrush(color);
            double radius = 12.0;
            target.Width = radius * 2;
            target.Height = radius * 2;
            ToolTip tt = new ToolTip();
            tt.Content = name + ": " + location;
            target.ToolTip = tt;
            Point p0 = myMap.LocationToViewportPoint(location);
            Point p1 = new Point(p0.X - radius, p0.Y - radius);
            Location loc = myMap.ViewportPointToLocation(p1);
            MapLayer.SetPosition(target, loc);
            myMap.Children.Add(target);

            return target;
        }

        private void removeTargetButton_Click(object sender, RoutedEventArgs e)
        {
            removeTarget();
        }

        private void removeAllTargetsButton_Click(object sender, RoutedEventArgs e)
        {
            removeAllTargets();
        }

        private void addTargetButton_Click(object sender, RoutedEventArgs e)
        {
            Location targetLocation = new Location();

            double longitude;
            double latitude;

            bool resultLong = (Double.TryParse(longitudeBox.Text, out longitude));
            bool resultLat = (Double.TryParse(latitudeBox.Text, out latitude));

            if (resultLong && resultLat)
            {
                
                //latitude ranges from -90 to 90 
                targetLocation.Latitude = latitude % 90;

                //longitude ranges from -180 to 180
                targetLocation.Longitude = longitude % 180;

                addTarget(targetLocation);
            }
            else
            {
                if (!resultLong)
                {
                    Console.Out.WriteLine("Invalid longitude entered.");
                    longitudeBox.Clear();
                }
                if (!resultLat)
                {
                    Console.Out.WriteLine("Invalid latitude entered.");
                    latitudeBox.Clear();
                }
            }
        }
    }
}
