using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class AccommodationRepository
    {
        private const string FilePathAccommodation = "../../../Resources/Data/accommodation.csv";

        private const string FilePathLocation = "../../../Resources/Data/location.csv";

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
        }

        public void Save(string accommodationName, string country, string city, string address, decimal latitude, decimal longitude, string type, int maxGuests, int minDaysReservation, int leftCancelationDays)
        {

            Location location = new Location(NextIdLocation(), country, city, address, latitude, longitude);
            Accommodation accommodation = new Accommodation(NextIdAccommodation(), accommodationName, location, type, maxGuests, minDaysReservation, leftCancelationDays);
            
            locations = locationSerializer.FromCSV(FilePathLocation);
            locations.Add(location);
            locationSerializer.ToCSV(FilePathLocation, locations);
            
            accommodations = accommodationSerializer.FromCSV(FilePathAccommodation);
            accommodations.Add(accommodation);
            accommodationSerializer.ToCSV(FilePathAccommodation, accommodations);
        }
            

        public int NextIdAccommodation()
        {
            accommodations = accommodationSerializer.FromCSV(FilePathAccommodation);
            if(accommodations.Count < 1)
            {
                return 1;
            }
            return accommodations.Max(c => c.Id) + 1;
        }

        public int NextIdLocation()
        {
            locations = locationSerializer.FromCSV(FilePathLocation);
            if (locations.Count < 1)
            {
                return 1;
            }
            return locations.Max(c => c.Id) + 1;
        }

        //Accommodation registration
    }
}
