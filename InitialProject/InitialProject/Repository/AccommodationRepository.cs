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

        public void Save(string accommodationName, string country, string city, string address, decimal latitude, decimal longitude, string type, int maxGuests, int minDaysReservation, int leftCancelationDays, List<string> images)
        {
            accommodations = accommodationSerializer.FromCSV(FilePathAccommodation);

            if(CheckErrorAccommodationName(accommodationName, accommodations) == true)
            {
                Location location = new Location(NextIdLocation(), country, city, address, latitude, longitude);
                Accommodation accommodation = new Accommodation(NextIdAccommodation(), accommodationName, location, type, maxGuests, minDaysReservation, leftCancelationDays, images);

                locations = locationSerializer.FromCSV(FilePathLocation);
                locations.Add(location);
                locationSerializer.ToCSV(FilePathLocation, locations);

                accommodations.Add(accommodation);
                accommodationSerializer.ToCSV(FilePathAccommodation, accommodations);

                MessageBox.Show("New accommodation has been successfully added.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Accommodation with this name already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // fokus na textbox
            }
        }

        private bool CheckErrorAccommodationName(string accommodationName, List<Accommodation> accommodations)
        {
            foreach (Accommodation temporaryAccommodation in accommodations)
            {
                if (temporaryAccommodation.AccommodationName.Equals(accommodationName) == true)
                {
                    return false;
                }
            }

            return true;
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


        // null,blank and white-spaced arguments will be ignored
        public List<Accommodation> FindAll(string accommodationName, string country, string city, string type, int maxGuests, int minDaysReservation)
        {
            List<Accommodation> accommodationNameResults, countryResults, cityResults, typeResults, maxGuestsResults, minDaysReservationResults;

            if (!string.IsNullOrWhiteSpace(accommodationName))
            {
                accommodationName = accommodationName.Trim();
                accommodationNameResults = FindAllByAccommodation(accommodationName);
                if (accommodationNameResults.Count == 0)
                {
                    return null;
                }
            }
            else
            {
                accommodationNameResults = accommodations;
            }


            // todo: proveriti ovo
            if (!string.IsNullOrWhiteSpace(country))
            {
                country = country.Trim();
                countryResults = FindAllByCountry(country);
                if (countryResults.Count == 0)
                {
                    return null;
                }
            }
            else
            {
                countryResults = accommodations;
            }


            // todo: proveriti ovo
            if (!string.IsNullOrWhiteSpace(city))
            {
                city = city.Trim();
                cityResults = FindAllByCity(city);
                if (cityResults.Count == 0)
                {
                    return null;
                }
            }
            else
            {
                cityResults = accommodations;
            }



            if (!string.IsNullOrWhiteSpace(type))
            {
                type = type.Trim();
                typeResults = FindAllByType(type);
                if (typeResults.Count == 0)
                {
                    return null;
                }
            }
            else
            {
                typeResults = accommodations;
            }



            if (maxGuests > 0)
            {
                maxGuestsResults = FindAllByMaxGuestsNumber(maxGuests);
                if (maxGuestsResults.Count == 0)
                {
                    return null;
                }
            }
            else
            {
                maxGuestsResults = accommodations;
            }


            if (minDaysReservation > 0)
            {
                minDaysReservationResults = FindAllAboveMinReservationDays(minDaysReservation);
                if (minDaysReservationResults.Count == 0)
                {
                    return null;
                }
            }
            else
            {
                minDaysReservationResults = accommodations;
            }

            return accommodationNameResults.Intersect(cityResults).Intersect(countryResults).Intersect(typeResults).Intersect(maxGuestsResults).Intersect(minDaysReservationResults).ToList();
        }

        public List<Accommodation> FindAllByAccommodation(string name)
        {
            List<Accommodation> searchResult = new List<Accommodation>();

            foreach (Accommodation accommodation in accommodations)
            {
                if (accommodation.AccommodationName.ToLower().StartsWith(name.ToLower()))
                    searchResult.Add(accommodation);
            }

            return searchResult;
        }

        public List<Accommodation> FindAllByCountry(string name)
        {
            List<Accommodation> searchResult = new List<Accommodation>();

            foreach (Accommodation accommodation in accommodations)
            {
                if (accommodation.Location.Country.ToLower().StartsWith(name.ToLower()))
                    searchResult.Add(accommodation);
            }

            return searchResult;
        }

        public List<Accommodation> FindAllByCity(string name)
        {
            List<Accommodation> searchResult = new List<Accommodation>();

            foreach (Accommodation accommodation in accommodations)
            {
                if (accommodation.Location.City.ToLower().StartsWith(name.ToLower()))
                    searchResult.Add(accommodation);
            }

            return searchResult;
        }

        public List<Accommodation> FindAllByMaxGuestsNumber(int quantity)
        {
            List<Accommodation> searchResult = new List<Accommodation>();

            foreach (Accommodation accommodation in accommodations)
            {
                if (accommodation.MaxGuests >= quantity)
                    searchResult.Add(accommodation);
            }

            return searchResult;
        }

        public List<Accommodation> FindAllByType(string name)
        {
            List<Accommodation> searchResult = new List<Accommodation>();

            foreach (Accommodation accommodation in accommodations)
            {
                if (accommodation.Type.ToLower().Equals(name.ToLower()))
                    searchResult.Add(accommodation);
            }

            return searchResult;
        }

        public List<Accommodation> FindAllAboveMinReservationDays(int minDaysReservation)
        {
            List<Accommodation> searchResult = new List<Accommodation>();

            foreach (Accommodation accommodation in accommodations)
            {
                if (accommodation.MinDaysReservation <= minDaysReservation)
                    searchResult.Add(accommodation);
            }

            return searchResult;
        }



    }
}
