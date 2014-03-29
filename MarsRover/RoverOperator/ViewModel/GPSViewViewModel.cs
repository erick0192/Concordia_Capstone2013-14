using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;
using MarsRover;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RoverOperator.Content
{
    class GPSViewViewModel : INotifyPropertyChanged
    {
        #region Properties

        public String Title
        {
            get
            {
                return "Location"; //Temporary
            }

        }

        public Map map;

        private Dispatcher _dispatcher;

        private Pushpin _roverPin;
        public Pushpin roverPin 
        {
            get
            {               
                return _roverPin;
            }
            set
            {
                _roverPin = value;
                if (roverPin != null)
                {
                    _roverPin.Visibility = System.Windows.Visibility.Visible;

                    //If the rover pin is not already in the pushpin collection, add it
                    if (!pushPinCollection.Contains(roverPin))
                    {
                        pushPinCollection.Add(roverPin);
                    }
                }
                else
                {
                    _roverPin.Visibility = System.Windows.Visibility.Hidden;
                }
                OnPropertyChanged("roverPin");
                //notify that the collection has updates
                OnPropertyChanged("pushPinCollection");
            }
        }

        private string _detailsString;
        public string detailsString
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_detailsString))
                {
                    return _detailsString;
                }
                else
                {
                    return "---";
                }
                
            }
            set
            {
                _detailsString = value;                
                OnPropertyChanged("detailsString");
            }
        }

        private GPSCoordinates _roverCoordinates;
        public GPSCoordinates roverCoordinates
        {
            get
            {
                return _roverCoordinates;
            }
            set
            {
                _roverCoordinates = value;
                updateRoverLocation();
                updateTargetDetails();
                OnPropertyChanged("roverCoordinates");
            }
        }        

        private string _roverCoordinateString;
        public string roverCoordinateString
        {
            get
            {
                return _roverCoordinateString;
            }
            set
            {
                _roverCoordinateString = value;                
                OnPropertyChanged("roverCoordinateString");
            }
        }

        //keep track of targets on map
        private List<Pushpin> targetPins;
        private ObservableCollection<Pushpin> _pushPinCollection;
        public ObservableCollection<Pushpin> pushPinCollection
        {
            get
            {
                return _pushPinCollection;
            }
            set
            {
                _pushPinCollection = value;
                OnPropertyChanged("pushPinCollection");
            }
        }


        //for input validation of longitude and latitude
        private string oldLongitudeString = "";
        private string oldLatitudeString = "";

        /// <summary>
        /// Coordinate values of the new target (populated by longitude and latitude text boxes)
        /// </summary>
        private string _longitudeString;
        public string longitudeString
        {
            get
            {
                return _longitudeString;
            }
            set
            {
                _longitudeString = value;
                //If the text entered by the user is accepted, store it, else reset it to the stored value
                if (validateCoordinateString(_longitudeString, out longitude))
                {                    
                    oldLongitudeString = _longitudeString;
                }
                else
                {
                    _longitudeString = oldLongitudeString;
                }

                determineCanAddTarget();
                
                OnPropertyChanged("longitudeString");
            }
        }
        private string _latitudeString;
        public string latitudeString
        {
            get
            {
                return _latitudeString;
            }
            set
            {
                _latitudeString = value;
                //If the text entered by the user is accepted, store it, else reset it to the stored value
                if (validateCoordinateString(_latitudeString, out latitude))
                {                    
                    oldLatitudeString = _latitudeString;
                }
                else
                {
                    _latitudeString = oldLatitudeString;
                }

                determineCanAddTarget();

                OnPropertyChanged("latitudeString");
            }
        }

        //double values used for adding targets
        private double longitude = 0;
        private double latitude = 0;

        //A name that the user assigns to a target
        private string _targetTitleString;
        public string targetTitleString
        {
            get
            {
                return _targetTitleString;
            }
            set
            {
                //Make sure first character of name is a letter, also allow the string to be empty
                if (Char.IsLetter(value.FirstOrDefault()) || string.IsNullOrWhiteSpace(value)) 
                {
                    _targetTitleString = value;
                }
                OnPropertyChanged("targetTitleString");
            }
        }

        #endregion

        #region Delegates and Events

        public event PropertyChangedEventHandler PropertyChanged;

        //Updated coordinates from status update
        private void UpdateGPS(GPSCoordinates gpsCoordinates)
        {
            roverCoordinates = gpsCoordinates;            
            roverCoordinateString = "Rover: " + gpsCoordinates.Location.ToString();           
        }
      

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Right click event to remove target (best way to do this even though using commands seems to be more elegant)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void targetPin_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            removeTarget((Pushpin)sender);
        }

        #endregion

        #region Constructor

        public GPSViewViewModel(Dispatcher d)
        {
            _dispatcher = d;
            StatusUpdater.Instance.GPSCoordinatesUpdated += new StatusUpdater.GPSCoordinatesUpdatedDelegate(UpdateGPS);
            //Make sure list and rover pin are instantiated
            if (pushPinCollection == null)
            {
                pushPinCollection = new ObservableCollection<Pushpin>();
            }                 
        }

        

        #endregion

        #region Methods

        /// <summary>
        /// (DEPRECATED) Binds the rover location data being sent by StatusUpdater to PushPin on the Map
        /// </summary>
        private void setRoverPinLocation()
        {
            bool messageReceived = false;
            var startTime = DateTime.UtcNow;

            roverCoordinates = new GPSCoordinates();
            roverPin = new Pushpin();

            //Wait for the StatusUpdater to send the location of the rover BEFORE the pushpin is set, or else DataBinding will not occur
            while (roverCoordinates.Location == null && (DateTime.UtcNow - startTime < TimeSpan.FromSeconds(2)))
            {
                //loop until the rover coordinates have been received, timeout after 2 seconds
                messageReceived = true;
            }

            //if the message was successfully received, bind the location
            if (messageReceived)
            {
                roverPin.Location = roverCoordinates.Location;

                formatRoverPin();
            }
        }

        /// <summary>
        /// Force map refresh when the rover pin gets updated.
        /// </summary>
        private void RefreshMap()
        {
            if (map != null)
            {
                map.UpdateLayout();
                var c = map.Center;
                c.Latitude += 0.00001;
                map.SetView(c, map.ZoomLevel);
            }
        }

        /// <summary>
        /// Update location of rover pin (triggered by the update of rovercoordinates
        /// </summary>
        private void updateRoverLocation()
        {
            //MIGHT NEED TO USE THIS CODE IN CASE IT CRASHES SAYING ITS TRYING TO ACCES FROM ANOTHER THREAD
            if (!_dispatcher.HasShutdownStarted && !_dispatcher.HasShutdownFinished)
            {
                _dispatcher.Invoke((Action)(() =>
                {
                    if (roverPin == null)
                    {
                        roverPin = new Pushpin();
                        formatRoverPin();                        
                    }
                    roverPin.Location = roverCoordinates.Location;
                    RefreshMap();
                }));
            }
        }

        /// <summary>
        /// Change the style of the rover pin
        /// </summary>
        private void formatRoverPin()
        {
            ToolTip tt = new ToolTip();
            tt.Content = "Rover";
            roverPin.ToolTip = tt;

            roverPin.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 255));
        }

        /// <summary>
        /// Change the style of the target pins
        /// </summary>
        private void formatTargetPin(Pushpin targetPin)
        {
            ToolTip tt = new ToolTip();

            //Title is not mandatory
            if (!string.IsNullOrWhiteSpace(targetTitleString))
            {
                //to enforce unique Name
                int counter = 0;

                while (String.IsNullOrWhiteSpace(targetPin.Name) && !String.IsNullOrWhiteSpace(targetTitleString))
                {
                    //Start counter at 1, stop incrementing when looping finishes
                    counter++;
                    try
                    {
                        targetPin.Name = targetTitleString.Replace(" ", "");
                    }
                    catch
                    {
                        //targetPin.Name = targetTitleString.Replace(" ", "") + "_" + counter;
                        targetPin.Name = "";
                        break;
                    }
                }

                tt.Content = targetTitleString + ": ";
            }
            else
            {
                //avoids null tooltip content.
                tt.Content = "";
            }

            tt.Content += targetPin.Location.Latitude + ", " + targetPin.Location.Longitude;
            targetPin.ToolTip = tt;

            targetPin.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));
        }

        /// <summary>
        /// Decide whether or not to allow the command to execute
        /// </summary>
        private void determineCanAddTarget()
        {
            if (!string.IsNullOrWhiteSpace(longitudeString) && !string.IsNullOrWhiteSpace(latitudeString))
            {
                canAddTarget = true;
            }
            else
            {
                canAddTarget = false;
            }
        }


        /// <summary>
        /// Updates the information in the details view
        /// </summary>
        public void updateTargetDetails()
        {
            //MIGHT NEED TO USE THIS CODE IN CASE IT CRASHES SAYING ITS TRYING TO ACCES FROM ANOTHER THREAD
            if (!_dispatcher.HasShutdownStarted && !_dispatcher.HasShutdownFinished)
            {
                _dispatcher.Invoke((Action)(() =>
                {
                    string detail = "";

                    //only attempt to recalculate if there are targets
                    if (targetPins != null && targetPins.Count > 0 && roverPin != null && roverPin.Location != null)
                    {
                        foreach (var target in targetPins)
                        {
                            double targetLongitude = target.Location.Longitude;
                            double targetLatitude = target.Location.Latitude;

                            ToolTip tt = (ToolTip)(target.ToolTip);
                            string targetName = tt.Content.ToString();
                            //string targetLongitudeString = targetLongitude.ToString();
                            //string targetLatitudeString = targetLatitude.ToString();

                            double distance = getDistance(roverPin.Location, target.Location);

                            //Show the name if it has one
                            if (!string.IsNullOrWhiteSpace(targetName))
                            {
                                detail += targetName + ", ";
                            }
                            //else //else show the coordinates to identify it
                            //{
                            //    detail += "(" + targetLongitudeString + ", " + targetLatitudeString + "), ";
                            //}

                            detail += "distance: " + distance +"km";
                            detail += "\n";
                        }                        
                    }

                    //Only update this once all calculations are done (or set to empty string if no targets are found), so that the property update doesnt trigger more often than needed (it would lower performance)
                    detailsString = detail;
                    
                }));
            }
        }

        /// <summary>
        /// Get distance in kilometers between two locations
        /// </summary>
        /// <param name="latitude1"></param>
        /// <param name="longitude1"></param>
        /// <param name="latitude2"></param>
        /// <param name="longitude2"></param>
        /// <returns></returns>
        private double getDistance(Location location1, Location location2)
        {
            double latitude1 = location1.Latitude;
            double longitude1 = location1.Longitude;

            double latitude2 = location2.Latitude;
            double longitude2 = location2.Longitude;

            double radiusOfEarth = 6371;
            double lateralDistanceRadians = degreesToRadians(latitude2 - latitude1);
            double longitudinalDistanceRadians = degreesToRadians(longitude2 - longitude1);
            double a = Math.Sin(lateralDistanceRadians / 2) * Math.Sin(lateralDistanceRadians / 2) + Math.Cos(degreesToRadians(latitude1)) * Math.Cos(degreesToRadians(latitude2)) * Math.Sin(longitudinalDistanceRadians / 2) * Math.Sin(longitudinalDistanceRadians / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = radiusOfEarth * c;
            distance = Math.Round(distance, 4);
            return distance;
        }

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        private double degreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        /// <summary>
        /// Add a target to the map (triggered by Add button)
        /// </summary>
        /// <param name="targetLocation"></param>
        private void addTarget(Location targetLocation)
        {
            //latitude ranges from -90 to 90 
            targetLocation.Latitude = latitude % 90;

            //longitude ranges from -180 to 180
            targetLocation.Longitude = longitude % 180;

            //Initialize target list
            if (targetPins == null)
            {
                targetPins = new List<Pushpin>();
            }

            // The pushpin to add to the map.
            Pushpin targetPin = new Pushpin();
            targetPin.Location = targetLocation;
            
            //Style the target
            formatTargetPin(targetPin);

            targetPin.MouseRightButtonUp += targetPin_MouseRightButtonUp;

            //Initialize target list
            if (targetPins == null)
            {
                targetPins = new List<Pushpin>();
            }
            targetPins.Add(targetPin);

            //Initialize the pushpin list
            if (pushPinCollection == null)
            {
                pushPinCollection = new ObservableCollection<Pushpin>();
            }
            pushPinCollection.Add(targetPin);

            updateTargetDetails();
            
            //reset values of the textboxes
            longitudeString = "";
            latitudeString = "";
            targetTitleString = "";

            //reset the data values
            longitude = 0;
            latitude = 0;      
        }
       

        /// <summary>
        /// Remove target from map (triggered by right clocking on a pushpin)
        /// </summary>
        private void removeTarget(Pushpin targetToRemove)
        {
            //check that pushpin is actually a target and is in the map's collection of pins
            if (pushPinCollection.Contains(targetToRemove) && targetPins.Contains(targetToRemove))
            {
                pushPinCollection.Remove(targetToRemove);
                targetPins.Remove(targetToRemove);

                updateTargetDetails();
            }
        }

        /// <summary>
        /// Data validation on user entered coordinates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool validateCoordinateString(string coordinateString, out double coordinate)
        {
            bool result = (Double.TryParse(coordinateString, out coordinate));

            //Allowed text = double format or empty string
            if (!result && !(coordinateString == "") && !(coordinateString == "-"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        
        #endregion

        #region Commands

        public bool canAddTarget;
        public RelayCommand _addTargetCommand;
        /// <summary>
        /// Add new target to the map
        /// </summary>
        public ICommand AddTargetCommand
        {
            get
            {
                if (_addTargetCommand == null)
                {
                    _addTargetCommand = new RelayCommand(param => this.ExecuteAddTargetCommand(), param => this.canAddTarget);
                }
                return _addTargetCommand;
            }
        }

        /// <summary>
        /// Add the target pin to target list
        /// </summary>
        private void ExecuteAddTargetCommand()
        {           
            Location targetLocation = new Location();            

            //add the target to the map
            addTarget(targetLocation);                           
        }

        public RelayCommand _removeTargetCommand;
        /// <summary>
        /// (DEPRECATED) Remove target pin from map
        /// </summary>
        public ICommand RemoveTargetCommand
        {
            get
            {
                if (_removeTargetCommand == null)
                {
                    _removeTargetCommand = new RelayCommand(param => this.ExecuteRemoveTargetCommand(), param => targetPins.Count > 0);
                }
                return _removeTargetCommand;
            }
        }

        /// <summary>
        /// (DEPRECATED) Remove the target pin to target list
        /// </summary>
        private void ExecuteRemoveTargetCommand()
        {
            //removeTarget();
        }

        #endregion
    }
}
