using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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

        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;

        private string guest1;
        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public AccommodationService()
        {
            accommodationRepository = new AccommodationRepository();
            locationService = new LocationService();
            reservationReschedulingRequestService = new ReservationReschedulingRequestService();
        }

        public AccommodationService(string username)
        {
            accommodationRepository = new AccommodationRepository();
            locationService = new LocationService();
            reservationReschedulingRequestService = new ReservationReschedulingRequestService();
            Guest1 = username;
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


        public List<Accommodation> FindAll(SearchAndShowAccommodationDTO searchShowAndAccommodationDTO)
        {
            List<Accommodation> accommodationNameResults, countryResults, cityResults, typeResults, maxGuestsResults, minDaysReservationResults;
            List<Accommodation> allAccommodations = accommodationRepository.FindAll();

            if (!IsNameContained(allAccommodations, searchShowAndAccommodationDTO.AccommodationName, out accommodationNameResults)) return null;
            if (!IsCountryContained(allAccommodations, searchShowAndAccommodationDTO.Country, out countryResults)) return null;
            if (!IsCityContained(allAccommodations, searchShowAndAccommodationDTO.City, out cityResults)) return null;
            if (!IsTypeContained(allAccommodations, searchShowAndAccommodationDTO.Type, out typeResults)) return null;
            if (!IsGuestsNumberContained(allAccommodations, searchShowAndAccommodationDTO.MaxGuests, out maxGuestsResults)) return null;
            if (!AreReservationDaysContained(allAccommodations, searchShowAndAccommodationDTO.MinDaysReservation, out minDaysReservationResults)) return null;

            allAccommodations = accommodationNameResults.Intersect(cityResults).Intersect(countryResults).Intersect(typeResults).Intersect(maxGuestsResults).Intersect(minDaysReservationResults).ToList();

            return allAccommodations;
        }

        private bool IsCountryContained(List<Accommodation> allAccommodations, string country, out List<Accommodation> countryResults)
        {
            if (!string.IsNullOrWhiteSpace(country))
            {
                country = country.Trim();
                countryResults = accommodationRepository.FindAllByCountry(allAccommodations, country);
                if (countryResults.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                countryResults = allAccommodations;
            }

            return true;
        }

        private bool IsNameContained(List<Accommodation> allAccommodations, string accommodationName, out List<Accommodation> accommodationNameResults)
        {
            if (!string.IsNullOrWhiteSpace(accommodationName))
            {
                accommodationName = accommodationName.Trim();
                accommodationNameResults = accommodationRepository.FindAllByAccommodationName(allAccommodations, accommodationName);
                if (accommodationNameResults.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                accommodationNameResults = allAccommodations;
            }

            return true;
        }

        private bool AreReservationDaysContained(List<Accommodation> allAccommodations, int? minDaysReservation, out List<Accommodation> minDaysReservationResults)
        {
            if ((minDaysReservation != null) && (minDaysReservation >= 0))
            {
                minDaysReservationResults = accommodationRepository.FindAllAboveMinReservationDays(allAccommodations, minDaysReservation);
                if (minDaysReservationResults.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                minDaysReservationResults = allAccommodations;
            }

            return true;
        }

        private bool IsGuestsNumberContained(List<Accommodation> allAccommodations, int? maxGuests, out List<Accommodation> maxGuestsResults)
        {
            if ((maxGuests != null) && (maxGuests > 0))
            {
                maxGuestsResults = accommodationRepository.FindAllByMaxGuestsNumber(allAccommodations, maxGuests);
                if (maxGuestsResults.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                maxGuestsResults = allAccommodations;
            }

            return true;
        }

        private bool IsTypeContained(List<Accommodation> allAccommodations, string type, out List<Accommodation> typeResults)
        {
            if (!string.IsNullOrWhiteSpace(type))
            {
                type = type.Trim();
                typeResults = accommodationRepository.FindAllByType(allAccommodations, type);
                if (typeResults.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                typeResults = allAccommodations;
            }

            return true;
        }

        private bool IsCityContained(List<Accommodation> allAccommodations, string city, out List<Accommodation> cityResults)
        {
            if (!string.IsNullOrWhiteSpace(city))
            {
                city = city.Trim();
                cityResults = accommodationRepository.FindAllByCity(allAccommodations, city);
                if (cityResults.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                cityResults = allAccommodations;
            }

            return true;
        }


        public bool Guest1HasNotification()
        {
            return reservationReschedulingRequestService.Guest1HasNotification(Guest1);
        }

    }
}
