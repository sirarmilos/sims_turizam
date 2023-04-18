using InitialProject.Dto;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private const string FilePathLocation = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Location> locationSerializer;

        private List<Location> locations;

        public LocationRepository()
        {
            locationSerializer = new Serializer<Location>();
            locations = locationSerializer.FromCSV(FilePathLocation);
        }

        public List<Location> FindAll()
        {
            return locationSerializer.FromCSV(FilePathLocation);
        }

        public void Save(List<Location> allLocations)
        {
            locationSerializer.ToCSV(FilePathLocation, allLocations);
        }

        public void Add(Location location)
        {
            List<Location> allLocations = FindAll();
            allLocations.Add(location);
            Save(allLocations);
        }

        public Location Save(LocationDto locationDto)
        {
            Location location = new(NextId(), locationDto.Country, locationDto.City, locationDto.Address, locationDto.Latitude, locationDto.Longitude);
            locations.Add(location);
            locationSerializer.ToCSV(FilePathLocation, locations);
            return location;
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }
            return FindAll().Max(c => c.Id) + 1;
        }

        public Location FindById(int locationId)
        {
            return FindAll().ToList().Find(x => x.Id == locationId);
        }
    }
}
