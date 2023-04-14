using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class LocationRepository
    {
        private const string FilePathLocation = "../../../Resources/Data/location.csv";

        private readonly Serializer<Location> locationSerializer;

        private List<Location> locations;

        public LocationRepository()
        {
            locationSerializer = new Serializer<Location>();
            locations = locationSerializer.FromCSV(FilePathLocation);
        }

        public List<Location> FindAllLocations()
        {
            return locationSerializer.FromCSV(FilePathLocation);
        }

        public void SaveLocations(List<Location> allLocations)
        {
            locationSerializer.ToCSV(FilePathLocation, allLocations);
        }

        public void UpdateLocations(Location location)
        {
            List<Location> allLocations = FindAllLocations();
            allLocations.Add(location);
            SaveLocations(allLocations);
        }

        public Location Save(LocationDto locationDto)
        {
            Location location = new(NextIdLocation(), locationDto.Country, locationDto.City, locationDto.Address, locationDto.Latitude, locationDto.Longitude);
            locations.Add(location);
            locationSerializer.ToCSV(FilePathLocation, locations);
            return location;
        }

        public int NextIdLocation()
        {
            if (FindAllLocations().Count < 1)
            {
                return 1;
            }
            return FindAllLocations().Max(c => c.Id) + 1;
        }

        public Location FindLocationByLocationId(int locationId)
        {
            return FindAllLocations().ToList().Find(x => x.Id == locationId);
        }

        public Location GetById(int id)
        {
            foreach(Location location in  locations)
            {
                if(location.Id == id)
                    return location;
            }
            return null;

        }


    }
}
