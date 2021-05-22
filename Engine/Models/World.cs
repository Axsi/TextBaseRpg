using System.Collections.Generic;

namespace Engine.Models
{
    //public cause we have to have it visible in another project
    public class World
    {
        //store the location objects in the world class, we will have a list that holds our location objects
        private List<Location> _locations = new List<Location>(); //private variable so only accessible inside the world object

        //our add location function
        internal void AddLocation (int xCoordinate, int yCoordinate, string name, string description, string imageName)
        {
            // Location loc = new Location();
            // loc.XCoordinate = xCoordinate;
            // loc.YCoordinate = yCoordinate;
            // loc.Name = name;
            // loc.Description = description;
            // loc.ImageName = $"/Engine;component/Images/Locations/{imageName}";
            
            //we use the newly created Location constructor below instead of doing it individually in the above commented code
            _locations.Add(new Location(xCoordinate, yCoordinate, name, description, $"/Engine;component/Images/Locations/{imageName}"));
        }

        //public because we will need to call it from the UI project
        //this function will return a location object for the given x and y coordinate
        public Location LocationAt(int xCoordinate, int yCoordinate)
        {
            foreach (Location loc in _locations)
            {
                if (loc.XCoordinate == xCoordinate && loc.YCoordinate == yCoordinate)
                {
                    return loc;
                }
            }

            return null; //if no location found, just return null
        }
    }
}