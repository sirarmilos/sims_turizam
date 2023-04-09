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

        private readonly LocationRepository locationRepository;

        public AccommodationService()
        {
            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();
        }

        public bool IsAccommodationNameExist(string accommodationName)
        {
            // poziva se vise puta i ima istu vrednost, tj. uzima sve smestaje, mozda da bude kao private polje i property get i set da ima
            List<Accommodation> allAccommodations = accommodationRepository.FindAllAccommodations();
            return allAccommodations.Exists(x => x.AccommodationName.Equals(accommodationName, StringComparison.OrdinalIgnoreCase));
        }

        public void SaveNewAccommodation(SaveNewAccommodationDTO saveNewAccommodationDTO)
        {
            SaveAccommodation(saveNewAccommodationDTO, SaveLocation(saveNewAccommodationDTO));
        }

        public Location SaveLocation(SaveNewAccommodationDTO saveNewAccommodationDTO)
        {
            Location location = new Location(NextIdLocation(), saveNewAccommodationDTO);

            List<Location> allLocations = locationRepository.FindAllLocations();

            allLocations.Add(location);

            locationRepository.SaveLocations(allLocations);

            return location;
        }

        public void SaveAccommodation(SaveNewAccommodationDTO saveNewAccommodationDTO, Location location)
        {
            Accommodation accommodation = new Accommodation(NextIdAccommodation(), saveNewAccommodationDTO, location);

            List<Accommodation> allAccommodations = accommodationRepository.FindAllAccommodations();

            allAccommodations.Add(accommodation);

            accommodationRepository.SaveAccommodations(allAccommodations);
        }

        public int NextIdLocation()
        {
            List<Location> allLocations = locationRepository.FindAllLocations();

            if (allLocations.Count < 1)
            {
                return 1;
            }

            return allLocations.Max(x => x.Id) + 1;
        }

        public int NextIdAccommodation()
        {
            List<Accommodation> allAccommodations = accommodationRepository.FindAllAccommodations();

            if (allAccommodations.Count < 1)
            {
                return 1;
            }

            return allAccommodations.Max(x => x.Id) + 1;
        }
    }
}
