﻿using InitialProject.DTO;
using InitialProject.Injector;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private readonly RateGuestsService rateGuestsService;

        private readonly UserService userService;

        private readonly ReservationService reservationService;

        private readonly CanceledReservationService cancelReservationService;

        private readonly RenovationRecommedationService renovationRecommedationService;

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
            accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>(); // new AccommodationRepository();
            locationService = new LocationService();
            reservationReschedulingRequestService = new ReservationReschedulingRequestService();
        }

        public AccommodationService(string username)
        {
            accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            locationService = new LocationService();
            reservationReschedulingRequestService = new ReservationReschedulingRequestService();
            rateGuestsService = new RateGuestsService(username);
            userService = new UserService();
            reservationService = new ReservationService(username);
            cancelReservationService = new CanceledReservationService();
            renovationRecommedationService = new RenovationRecommedationService();

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

        public Accommodation FindAccommodationByAccommodationName(string accommodationName)
        {
            return accommodationRepository.FindByAccommodationName(accommodationName);
        }

        public void CheckRecentlyRenovated()
        {
            List<Accommodation> allAccommodations = accommodationRepository.FindAll();

            List<Renovation> allRenovations = accommodationRepository.FindAllRenovations();

            foreach(Accommodation temporaryAccommodation in allAccommodations.ToList())
            {
                List<Renovation> temporaryRenovations = allRenovations.FindAll(x => x.Accommodation.Id == temporaryAccommodation.Id && DateTime.Compare(x.EndDate, DateTime.Now) < 0);

                DateTime dateTimeNew = DateTime.MinValue;

                foreach(Renovation temporaryRenovation in temporaryRenovations.ToList())
                {
                    if(DateTime.Compare(temporaryRenovation.EndDate, dateTimeNew) > 0)
                    {
                        dateTimeNew = temporaryRenovation.EndDate;
                    }
                }

                temporaryAccommodation.RecentlyRenovated = false;

                if(DateTime.Now.Subtract(dateTimeNew).Days < 365)
                {
                    temporaryAccommodation.RecentlyRenovated = true;
                }
            }

            accommodationRepository.Save(allAccommodations);
        }

        public int FindNumberOfUnratedGuests(string ownerUsername)
        {
            return rateGuestsService.FindNumberOfUnratedGuests(ownerUsername);
        }

        public void SaveViewedCancelledReservation(CancelledReservationsNotificationDTO cancelledReservationsNotificationDTO)
        {
            userService.SaveViewedCancelledReservation(cancelledReservationsNotificationDTO);
        }

        public List<string> FindUnreadCancelledReservations(string ownerUsername)
        {
            return userService.FindUnreadCancelledReservations(ownerUsername);
        }

        public ObservableCollection<ShowAccommodationDTO> FindOwnerAccommodations(string ownerUsername)
        {
            ObservableCollection<ShowAccommodationDTO> showAccommodationDTOs = new ObservableCollection<ShowAccommodationDTO>();

            List<Accommodation> ownerAccommodations = accommodationRepository.FindByOwnerUsername(ownerUsername);

            foreach(Accommodation temporaryAccommodation in ownerAccommodations.ToList())
            {
                showAccommodationDTOs.Add(new ShowAccommodationDTO(temporaryAccommodation));
            }

            return showAccommodationDTOs;
        }

        public string FindSuperTypeByOwnerName(string ownerName)
        {
            return userService.FindSuperTypeByOwnerName(ownerName);
        }

        public ShowStatisticsAccommodationDTO FindSelectedAccommodation(int accommodationId)
        {
            Accommodation accommodation = accommodationRepository.FindById(accommodationId);

            ShowStatisticsAccommodationDTO showStatisticsAccommodationDTO = new ShowStatisticsAccommodationDTO(accommodation);

            return showStatisticsAccommodationDTO;
        }

        public List<int> FindAccommodationReservationsYears(int accommodationId)
        {
            return reservationService.FindAccommodationReservationsYears(accommodationId);
        }

        public List<int> FindAccommodationCanceledReservationsYears(int accommodationId)
        {
            return cancelReservationService.FindAccommodationCanceledReservationsYears(accommodationId);
        }

        public List<int> FindAccommodationRescheduledReservationsYears(int accommodationId)
        {
            return reservationReschedulingRequestService.FindAccommodationRescheduledReservationsYears(accommodationId);
        }

        /* public List<int> FindAccommodationRenovationRecommedationsYears(int accommodationId)
        {
            return renovationRecommedationService.FindAccommodationRenovationRecommedationsYears(accommodationId);
        }*/
    }
}
