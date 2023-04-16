using InitialProject.DTO;
using InitialProject.IRepository;
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
        private readonly IAccommodationRepository accommodationRepository;

        private readonly LocationService locationService;

        public AccommodationService()
        {
            accommodationRepository = new AccommodationRepository();
            locationService = new LocationService();
        }

        public bool IsAccommodationNameExist(string accommodationName)
        {
            return accommodationRepository.IsAccommodationExist(accommodationName);
        }

        public void SaveNewAccommodation(SaveNewAccommodationDTO saveNewAccommodationDTO)
        {
            SaveAccommodation(saveNewAccommodationDTO, locationService.Save(saveNewAccommodationDTO));
        }

        public void SaveAccommodation(SaveNewAccommodationDTO saveNewAccommodationDTO, Location location)
        {
            Accommodation accommodation = new Accommodation(accommodationRepository.NextId(), saveNewAccommodationDTO, location);

            accommodationRepository.Add(accommodation);
        }
    }
}
