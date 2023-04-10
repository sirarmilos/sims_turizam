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

        public List<Accommodation> AllAccommodations
        {
            get;
            set;
        }

        public List<Location> AllLocations
        {
            get;
            set;
        }

        public AccommodationService()
        {
            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();

            ListInitialization();
        }

        private void ListInitialization()
        {
            AllAccommodations = new List<Accommodation>();
            AllLocations = new List<Location>();
        }

        public bool IsAccommodationNameExist(string accommodationName)
        {
            // poziva se vise puta i ima istu vrednost, tj. uzima sve smestaje, mozda da bude kao private polje i property get i set da ima
            AllAccommodations = accommodationRepository.FindAllAccommodations();
            return AllAccommodations.Exists(x => x.AccommodationName.Equals(accommodationName, StringComparison.OrdinalIgnoreCase));
        }

        public void SaveNewAccommodation(SaveNewAccommodationDTO saveNewAccommodationDTO)
        {
            SaveAccommodation(saveNewAccommodationDTO, SaveLocation(saveNewAccommodationDTO));
        }

        public Location SaveLocation(SaveNewAccommodationDTO saveNewAccommodationDTO)
        {
            AllLocations = locationRepository.FindAllLocations();

            Location location = new Location(NextIdLocation(), saveNewAccommodationDTO);

            AllLocations.Add(location);

            locationRepository.SaveLocations(AllLocations);

            return location;
        }

        public void SaveAccommodation(SaveNewAccommodationDTO saveNewAccommodationDTO, Location location)
        {
            Accommodation accommodation = new Accommodation(NextIdAccommodation(), saveNewAccommodationDTO, location);

            AllAccommodations.Add(accommodation);

            accommodationRepository.SaveAccommodations(AllAccommodations);
        }

        public int NextIdLocation()
        {
            if (AllLocations.Count < 1)
            {
                return 1;
            }

            return AllLocations.Max(x => x.Id) + 1;
        }

        public int NextIdAccommodation()
        {
            if (AllAccommodations.Count < 1)
            {
                return 1;
            }

            return AllAccommodations.Max(x => x.Id) + 1;
        }
    }
}
