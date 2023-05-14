using InitialProject.Dto;
using InitialProject.DTO;
using InitialProject.Injector;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Location Save(LocationDto locationDto)
        {
            return locationRepository.Save(locationDto);
        }
    }
}
