using InitialProject.Dto;
using InitialProject.DTO;
using InitialProject.Injector;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class LocationService
    {
        private readonly ILocationRepository locationRepository;

        public LocationService()
        {
            locationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            //locationRepository = new LocationRepository();
        }

        public Location Save(SaveNewAccommodationDTO saveNewAccommodationDTO)
        {
            Location location = new Location(locationRepository.NextId(), saveNewAccommodationDTO);

            locationRepository.Add(location);

            return location;
        }

        public Location FindById(int id)
        {
            return locationRepository.FindById(id);
        }
        
        public Location Save(LocationDto locationDto)
        {
            return locationRepository.Save(locationDto);
        }

        public List<Location>  FindAll()
        {
            return locationRepository.FindAll();
        }

        public List<int> FindIdByCountryAndCity(TopAndWorstLocationDTO worstLocationDTOToRemove)
        {
            string country = worstLocationDTOToRemove.Location.Split(", ")[0];
            string city = worstLocationDTOToRemove.Location.Split(", ")[1];

            List<Location> locations = FindAll().ToList().FindAll(x => x.Country.Equals(country) == true && x.City.Equals(city) == true);

            List<int> ids = new List<int>();

            foreach(Location temporaryLocation in locations.ToList())
            {
                ids.Add(temporaryLocation.Id);
            }

            return ids;
        }
    }
}
