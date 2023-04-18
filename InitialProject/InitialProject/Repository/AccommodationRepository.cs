using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Repository
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private LocationRepository locationRepository;

        private const string FilePathAccommodation = "../../../Resources/Data/accommodations.csv";

        private const string FilePathLocation = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Accommodation> accommodationSerializer;

        private readonly Serializer<Location> locationSerializer;

        private List<Accommodation> accommodations;

        private List<Location> locations;

        public AccommodationRepository()
        {
            accommodationSerializer = new Serializer<Accommodation>();
            accommodations = accommodationSerializer.FromCSV(FilePathAccommodation);
            locationSerializer = new Serializer<Location>();
            locations = locationSerializer.FromCSV(FilePathLocation);

            foreach (Accommodation accommodation in accommodations)
            {
                if (locations == null)
                    break;
                foreach (Location location in locations)
                {
                    if (location.Id == accommodation.Location.Id)
                    {
                        accommodation.Location = location;
                        break;
                    }
                }
            }
        }

        public List<Accommodation> FindAll()
        {
            locationRepository = new LocationRepository();

            accommodations = accommodationSerializer.FromCSV(FilePathAccommodation);

            foreach(Accommodation temporaryAccommodation in accommodations.ToList())
            {
                temporaryAccommodation.Location = locationRepository.FindById(temporaryAccommodation.Location.Id);
            }

            return accommodations;
        }

        public void Save(List<Accommodation> allAccommodations)
        {
            accommodationSerializer.ToCSV(FilePathAccommodation, allAccommodations);
        }

        public void Add(Accommodation accommodation)
        {
            List<Accommodation> allAccommodations = FindAll();
            allAccommodations.Add(accommodation);
            Save(allAccommodations);
        }

        public bool IsAccommodationExist(string accommodationName)
        {
            return FindAll().Exists(x => x.AccommodationName.Equals(accommodationName) == true);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }

            return FindAll().Max(x => x.Id) + 1;
        }

        public Accommodation FindById(int accommodationId)
        {
            return FindAll().ToList().Find(x => x.Id == accommodationId);
        }




        // moraju da dele allAccommodations, nez da l u rep ili servisu da ostavim
        public List<Accommodation> FindAllByAccommodationName(List<Accommodation> allAccommodations, string name) 
        {
            return allAccommodations.FindAll(x => x.AccommodationName.ToLower().StartsWith(name.ToLower()) == true);
        }

        public List<Accommodation> FindAllByCountry(List<Accommodation> allAccommodations, string name)
        {
            return allAccommodations.FindAll(x => x.Location.Country.ToLower().StartsWith(name.ToLower()) == true);
        }

        public List<Accommodation> FindAllByCity(List<Accommodation> allAccommodations, string name)
        {
            return allAccommodations.FindAll(x => x.Location.City.ToLower().StartsWith(name.ToLower()) == true);
        }

        public List<Accommodation> FindAllByMaxGuestsNumber(List<Accommodation> allAccommodations, int? quantity)
        {
            return allAccommodations.FindAll(x => x.MaxGuests >= quantity);
        }

        public List<Accommodation> FindAllByType(List<Accommodation> allAccommodations, string name)
        {
            return allAccommodations.FindAll(x => x.Type.ToLower().Equals(name.ToLower()) == true);
        }

        public List<Accommodation> FindAllAboveMinReservationDays(List<Accommodation> allAccommodations, int? minDaysReservation)
        {
            return allAccommodations.FindAll(x => x.MinDaysReservation <= minDaysReservation);
        }
    }
}
