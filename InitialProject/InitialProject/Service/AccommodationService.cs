using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Service
{
    public class AccommodationService
    {
        private readonly AccommodationRepository accommodationRepository;

        private readonly LocationService locationService;

        public AccommodationService()
        {
            accommodationRepository = new AccommodationRepository();
            locationService = new LocationService();
        }

        public bool IsAccommodationNameExist(string accommodationName)
        {
            return accommodationRepository.IsAccommodationWithAccommodationNameExist(accommodationName);
        }

        public void SaveNewAccommodation(SaveNewAccommodationDTO saveNewAccommodationDTO)
        {
            SaveAccommodation(saveNewAccommodationDTO, locationService.SaveLocation(saveNewAccommodationDTO));
        }

        public void SaveAccommodation(SaveNewAccommodationDTO saveNewAccommodationDTO, Location location)
        {
            Accommodation accommodation = new Accommodation(accommodationRepository.NextIdAccommodation(), saveNewAccommodationDTO, location);

            accommodationRepository.UpdateAccommodations(accommodation);
        }
    }
}
