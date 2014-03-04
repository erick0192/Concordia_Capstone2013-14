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

        //for input validation of longitude and latitude
        private string oldEnteredCoordinateText = "";

        //Coordinate values of the new target (populated by longitude and latitude text boxes)
        private double longitude = 0;
        private double latitude = 0;

        public GPSView()
        {
            InitializeComponent();

            gpsVM = new GPSViewViewModel();
            this.DataContext = gpsVM;            

            //bind events
            myMap.MouseDoubleClick += map_MouseDoubleClick;            
        }

        /// <summary>
        /// Handle mouse double click to add targets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void map_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determine the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location targetLocation = myMap.ViewportPointToLocation(mousePosition);
            
            addTarget(targetLocation);           
        }

        /// <summary>
        /// Add a target to the map (triggered by Add button)
        /// </summary>
        /// <param name="location"></param>
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
            tt.Content = targetPin.Location.Latitude + ", " + targetPin.Location.Longitude;
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

        /// <summary>
        /// (DEPRECATED): Remove all targets from map (triggered by 'Remove' button)
        /// </summary>
        private void removeTarget()
        {
            //check both but they should be the same number all the time
            if (myMap.Children.Count > 0 && targetPins.Count > 0)
            {
                myMap.Children.Remove(targetPins.Last());
                targetPins.Remove(targetPins.Last());
            }
        }

        /// <summary>
        /// (DEPRECATED): Remove all targets from map (triggered by 'Remove All' button)
        /// </summary>
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

        /// <summary>
        /// Add a marker to the map
        /// </summary>
        /// <param name="location"></param>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <returns></returns>
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

        /// <summary>
        /// (DEPRECATED): Remove a target when clicking the remove button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeTargetButton_Click(object sender, RoutedEventArgs e)
        {
            removeTarget();
        }

        /// <summary>
        /// (DEPRECATED): Remove a target when clicking the remove all button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeAllTargetsButton_Click(object sender, RoutedEventArgs e)
        {
            removeAllTargets();
        }

        /// <summary>
        /// Add a target to the map when the Add button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addTargetButton_Click(object sender, RoutedEventArgs e)
        {   
            //Make sure the text boxes have text in them to avoid empty fields generating (0,0) targets
            if (!String.IsNullOrWhiteSpace(latitudeBox.Text) && !String.IsNullOrWhiteSpace(longitudeBox.Text))
            {
                Location targetLocation = new Location();

                //latitude ranges from -90 to 90 
                targetLocation.Latitude = latitude % 90;

                //longitude ranges from -180 to 180
                targetLocation.Longitude = longitude % 180;

                //add the target to the map
                addTarget(targetLocation);

                //reset values of the textboxes
                longitudeBox.Clear();
                latitudeBox.Clear();

                //reset the data values
                longitude = 0;
                latitude = 0;
            }            
        }

        /// <summary>
        /// Save old value of the text (assumed to be a valid double value or empty)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void coordinateBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            oldEnteredCoordinateText = ((TextBox)sender).Text;
        }

        /// <summary>
        /// Data validation on text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void coordinateBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double coordinate;

            bool result = (Double.TryParse(((TextBox)sender).Text, out coordinate));

            //Allowed text = double format or empty string
            if (!result && !(((TextBox)sender).Text == ""))
            {
                ((TextBox)sender).Text = oldEnteredCoordinateText;
            }
            else
            {
                switch (((TextBox)sender).Name)
                {
                    case "longitudeBox":
                        longitude = coordinate;
                        break;
                    case "latitudeBox":
                        latitude = coordinate;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
