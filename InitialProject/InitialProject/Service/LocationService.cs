using InitialProject.DTO;
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
        private readonly LocationRepository locationRepository;

        public LocationService()
        {
            locationRepository = new LocationRepository();
        }

        public Location Save(SaveNewAccommodationDTO saveNewAccommodationDTO)
        {
            Location location = new Location(locationRepository.NextIdLocation(), saveNewAccommodationDTO);

            locationRepository.Add(location);

            return location;
        }
    }
}
