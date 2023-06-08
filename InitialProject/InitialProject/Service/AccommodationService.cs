using InitialProject.DTO;
using InitialProject.Injector;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using InitialProject.View;
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

        private readonly CanceledReservationService canceledReservationService;

        private readonly RenovationRecommendationService renovationRecommendationService;

        private readonly ForumNotificationsToOwnerService forumNotificationsToOwnerService;

        private readonly RenovationService renovationService;

        public Dictionary<int, string> months = new Dictionary<int, string>()
        {
            { 1, "January" },
            { 2, "February" },
            { 3, "March" },
            { 4, "April" },
            { 5, "May" },
            { 6, "June" },
            { 7, "July" },
            { 8, "August" },
            { 9, "September" },
            { 10, "October" },
            { 11, "November" },
            { 12, "December" },
        };

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
            //accommodationRepository = new AccommodationRepository();
            locationService = new LocationService();
            reservationReschedulingRequestService = new ReservationReschedulingRequestService();
            canceledReservationService = new CanceledReservationService();
            forumNotificationsToOwnerService = new ForumNotificationsToOwnerService();
            renovationService = new RenovationService();
        }

        public AccommodationService(string username)
        {
            accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            //accommodationRepository = new AccommodationRepository();
            locationService = new LocationService();
            reservationReschedulingRequestService = new ReservationReschedulingRequestService();
            rateGuestsService = new RateGuestsService(username);
            userService = new UserService();
            reservationService = new ReservationService(username);
            canceledReservationService = new CanceledReservationService();
            renovationRecommendationService = new RenovationRecommendationService();
            forumNotificationsToOwnerService = new ForumNotificationsToOwnerService();
            renovationService = new RenovationService(username);

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




        public bool IsSuperGuest(string guest1Username)
        {
            return userService.IsSuperGuest(guest1Username);
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

        public List<Accommodation> FindAll()
        {
            return accommodationRepository.FindAll();
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

            if (type != null  && type.Equals("All Types")) type = "";

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

        public List<CancelledReservationsNotificationDTO> FindUnreadCancelledReservations(string ownerUsername)
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

        public List<string> FindAccommodationYears(int accommodationId)
        {
            List<string> years = new List<string>();

            List<int> intYears = reservationService.FindAccommodationReservationsYears(accommodationId);

            years.Add("all year");

            foreach(int year in intYears.ToList())
            {
                years.Add(year.ToString());
            }

            return years;
        }

        public List<AccommodationStatisticsDataDTO> FindAccommodationYearStatistics(int accommodationId, List<string> stringYears)
        {
            List<AccommodationStatisticsDataDTO> accommodationStatisticsDataDTOs = new List<AccommodationStatisticsDataDTO>();

            List<int> years = ConvertYearsToInt(stringYears);

            foreach(int year in years.ToList())
            {
                int reservationCount = reservationService.FindAccommodationReservationCountByYear(accommodationId, year);
                int canceledReservationCount = canceledReservationService.FindAccommodationCanceledReservationCountByYear(accommodationId, year);
                int rescheduledReservationCount = reservationReschedulingRequestService.FindAccommodationRescheduledReservationCountByYear(accommodationId, year);
                int renovationRecommedationCount = renovationRecommendationService.FindAccommodationRenovationRecommedationCountByYear(accommodationId, year);

                accommodationStatisticsDataDTOs.Add(new AccommodationStatisticsDataDTO(year, reservationCount, canceledReservationCount, rescheduledReservationCount, renovationRecommedationCount));
            }

            return accommodationStatisticsDataDTOs;
        }

        public List<int> FindAccommodationReservationCount(List<AccommodationStatisticsDataDTO> accommodationStatisticsDataDTOs)
        {
            List<int> reservationCount = new List<int>();

            foreach (AccommodationStatisticsDataDTO temporaryAccommodationStatisticsDataDTO in accommodationStatisticsDataDTOs.ToList())
            {
                reservationCount.Add(temporaryAccommodationStatisticsDataDTO.ReservationsCount);
            }

            return reservationCount;
        }

        public List<int> FindAccommodationCanceledReservationCount(List<AccommodationStatisticsDataDTO> accommodationStatisticsDataDTOs)
        {
            List<int> canceledReservationCount = new List<int>();

            foreach (AccommodationStatisticsDataDTO temporaryAccommodationStatisticsDataDTO in accommodationStatisticsDataDTOs.ToList())
            {
                canceledReservationCount.Add(temporaryAccommodationStatisticsDataDTO.CanceledReservationsCount);
            }

            return canceledReservationCount;
        }

        public List<int> FindAccommodationRescheduledReservationCount(List<AccommodationStatisticsDataDTO> accommodationStatisticsDataDTOs)
        {
            List<int> rescheduledReservationCount = new List<int>();

            foreach (AccommodationStatisticsDataDTO temporaryAccommodationStatisticsDataDTO in accommodationStatisticsDataDTOs.ToList())
            {
                rescheduledReservationCount.Add(temporaryAccommodationStatisticsDataDTO.RescheduledReservationCount);
            }

            return rescheduledReservationCount;
        }

        public List<int> FindAccommodationRenovationRecommendationsCount(List<AccommodationStatisticsDataDTO> accommodationStatisticsDataDTOs)
        {
            List<int> renovationRecommendationsCount = new List<int>();

            foreach (AccommodationStatisticsDataDTO temporaryAccommodationStatisticsDataDTO in accommodationStatisticsDataDTOs.ToList())
            {
                renovationRecommendationsCount.Add(temporaryAccommodationStatisticsDataDTO.RenovationRecommedationsCount);
            }

            return renovationRecommendationsCount;
        }

        public List<int> ConvertYearsToInt(List<string> stringYears)
        {
            List<int> years = new List<int>();

            foreach (string temporaryStringYear in stringYears.ToList())
            {
                if (temporaryStringYear.Equals("all year") == false)
                {
                    years.Add(Convert.ToInt32(temporaryStringYear));
                }
            }

            return years;
        }

        public int FindMostBusyYear(int accommodationId, List<string> stringYears)
        {
            List<Reservation> accommodationReservations = reservationService.FindByAccommodationId(accommodationId);

            Dictionary<int, decimal> busyYears = new Dictionary<int, decimal>();

            List<int> years = ConvertYearsToInt(stringYears);

            busyYears = FindBusyYears(accommodationReservations, years);

            return MostBusyYearCheck(busyYears);
        }

        public Dictionary<int, decimal> FindBusyYears(List<Reservation> reservations, List<int> years)
        {
            Dictionary<int, decimal> busyYears = new Dictionary<int, decimal>();

            foreach (int year in years.ToList())
            {
                busyYears[year] = 0;
            }

            foreach (Reservation temporaryAccommodationReservation in reservations.ToList())
            {
                while (temporaryAccommodationReservation.StartDate.Year != temporaryAccommodationReservation.EndDate.Year)
                {
                    DateTime temporaryStartDate = new DateTime(temporaryAccommodationReservation.EndDate.Year, 1, 1);
                    busyYears[temporaryAccommodationReservation.EndDate.Year] += (temporaryAccommodationReservation.EndDate.Subtract(temporaryStartDate).Days + 1); //

                    temporaryAccommodationReservation.EndDate = temporaryAccommodationReservation.EndDate.AddYears(-1);
                    temporaryAccommodationReservation.EndDate = new DateTime(temporaryAccommodationReservation.EndDate.Year, 12, 31);
                }

                busyYears[temporaryAccommodationReservation.StartDate.Year] += (temporaryAccommodationReservation.EndDate.Subtract(temporaryAccommodationReservation.StartDate).Days + 1); //
            }

            busyYears = CalculateBusyYearsPercentage(busyYears, years);

            return busyYears;
        }

        public Dictionary<int, decimal> CalculateBusyYearsPercentage(Dictionary<int, decimal> busyYears, List<int> years)
        {
            foreach (int year in years.ToList())
            {
                DateTime yearStartDate = new DateTime(year, 1, 1);
                DateTime yearEndDate = new DateTime(year, 12, 31);
                int yearDays = yearEndDate.Subtract(yearStartDate).Days + 1;
                busyYears[year] = busyYears[year] / yearDays;
            }

            return busyYears;
        }

        public int MostBusyYearCheck(Dictionary<int, decimal> busyYears)
        {
            if(busyYears.Count == 0)
            {
                return -1;
            }

            return busyYears.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        }

        public List<AccommodationStatisticsDataDTO> FindAccommodationMonthStatistics(int accommodationId, int year)
        {
            List<AccommodationStatisticsDataDTO> accommodationStatisticsDataDTOs = new List<AccommodationStatisticsDataDTO>();

            List<int> reservationCount = reservationService.FindAccommodationReservationCountByMonth(accommodationId, year);
            List<int> canceledReservationCount = canceledReservationService.FindAccommodationCanceledReservationCountByMonth(accommodationId, year);
            List<int> rescheduledReservationCount = reservationReschedulingRequestService.FindAccommodationRescheduledReservationCountByMonth(accommodationId, year);
            List<int> renovationRecommedationCount = renovationRecommendationService.FindAccommodationRenovationRecommedationCountByMonth(accommodationId, year);

            for(int i = 0; i <= 11; i++)
            {
                accommodationStatisticsDataDTOs.Add(new AccommodationStatisticsDataDTO(i + 1, reservationCount[i], canceledReservationCount[i], rescheduledReservationCount[i], renovationRecommedationCount[i]));
            }

            return accommodationStatisticsDataDTOs;
        }

        public List<int> FindAccommodationReservationMonthCount(List<AccommodationStatisticsDataDTO> accommodationStatisticsDataDTOs)
        {
            List<int> reservationCount = new List<int>();

            foreach(AccommodationStatisticsDataDTO temporaryAccommodationStatisticsDataDTO in accommodationStatisticsDataDTOs.ToList())
            {
                reservationCount.Add(temporaryAccommodationStatisticsDataDTO.ReservationsCount);
            }

            return reservationCount;
        }

        public List<int> FindAccommodationCanceledReservationMonthCount(List<AccommodationStatisticsDataDTO> accommodationStatisticsDataDTOs)
        {
            List<int> canceledReservationCount = new List<int>();

            foreach(AccommodationStatisticsDataDTO temporaryAccommodationStatisticsDataDTO in accommodationStatisticsDataDTOs.ToList())
            {
                canceledReservationCount.Add(temporaryAccommodationStatisticsDataDTO.CanceledReservationsCount);
            }

            return canceledReservationCount;
        }

        public List<int> FindAccommodationRescheduledReservationMonthCount(List<AccommodationStatisticsDataDTO> accommodationStatisticsDataDTOs)
        {
            List<int> rescheduledReservationCount = new List<int>();

            foreach(AccommodationStatisticsDataDTO temporaryAccommodationStatisticsDataDTO in accommodationStatisticsDataDTOs.ToList())
            {
                rescheduledReservationCount.Add(temporaryAccommodationStatisticsDataDTO.RescheduledReservationCount);
            }

            return rescheduledReservationCount;
        }

        public List<int> FindAccommodationRenovationRecommendationsMonthCount(List<AccommodationStatisticsDataDTO> accommodationStatisticsDataDTOs)
        {
            List<int> renovationRecommendationsCount = new List<int>();

            foreach(AccommodationStatisticsDataDTO temporaryAccommodationStatisticsDataDTO in accommodationStatisticsDataDTOs.ToList())
            {
                renovationRecommendationsCount.Add(temporaryAccommodationStatisticsDataDTO.RenovationRecommedationsCount);
            }

            return renovationRecommendationsCount;
        }

        public string FindMostBusyMonth(int accommodationId, int year)
        {
            List<Reservation> accommodationReservations = reservationService.FindAccommodationReservationsByYear(accommodationId, year);

            Dictionary<int, decimal> busyMonths = new Dictionary<int, decimal>();

            busyMonths = FindBusyMonths(accommodationReservations, year);

            int maxBusyMonth = busyMonths.Aggregate((x, y) => x.Value > y.Value ? x : y).Key + 1;

            return months[maxBusyMonth];
        }

        public Dictionary<int, decimal> FindBusyMonths(List<Reservation> reservations, int year)
        {
            Dictionary<int, decimal> busyMonths = new Dictionary<int, decimal>();

            for(int i = 0; i <= 11; i++)
            {
                busyMonths[i] = 0;
            }

            foreach(Reservation temporaryReservation in reservations.ToList())
            {
                if(temporaryReservation.StartDate.Year != year)
                {
                    temporaryReservation.StartDate = new DateTime(year, 1, 1);
                }

                if(temporaryReservation.EndDate.Year != year)
                {
                    temporaryReservation.EndDate = new DateTime(year, 12, 31);
                }

                while(temporaryReservation.EndDate.Month != temporaryReservation.StartDate.Month && temporaryReservation.StartDate.Month != 12)
                {
                    int maximumMonthDay = DateTime.DaysInMonth(year, temporaryReservation.StartDate.Month);
                    busyMonths[temporaryReservation.StartDate.Month - 1] += maximumMonthDay;

                    temporaryReservation.StartDate = new DateTime(year, (temporaryReservation.StartDate.Month + 1), 1);
                }

                busyMonths[temporaryReservation.StartDate.Month - 1] += (temporaryReservation.EndDate.Subtract(temporaryReservation.StartDate).Days + 1); //
            }

            busyMonths = CalculateBusyMonthsPercentage(busyMonths, year);

            return busyMonths;
        }

        public Dictionary<int, decimal> CalculateBusyMonthsPercentage(Dictionary<int, decimal> busyMonths, int year)
        {
            for(int i = 0; i <= 11; i++)
            {
                int monthDays = DateTime.DaysInMonth(year, i + 1);
                busyMonths[i] = busyMonths[i] / monthDays;
            }

            return busyMonths;
        }

        public List<string> FindOwnerAccommodationNames(string ownerUsername)
        {
            List<Accommodation> ownerAccommodations = accommodationRepository.FindByOwnerUsername(ownerUsername);

            List<string> accommodationNames = new List<string>();

            foreach(Accommodation temporaryAccommodation in ownerAccommodations.ToList())
            {
                accommodationNames.Add(temporaryAccommodation.AccommodationName);
            }

            return accommodationNames;
        }

        public List<Accommodation> FindOwnerAccommodationsToPDFReport(string ownerUsername)
        {
            return accommodationRepository.FindByOwnerUsername(ownerUsername);
        }

        public void MarkAsReadNotificationsCancelledReservations(List<CancelledReservationsNotificationDTO> unreadCancelledReservations)
        {
            canceledReservationService.MarkAsReadNotificationsCancelledReservations(unreadCancelledReservations);
        }

        public string FindTopLocation()
        {
            List<Accommodation> allAccommodations = accommodationRepository.FindAll();

            List<TopAndWorstLocationDTO> topLocationDTOs = FindAllLocationsToTopLocation(allAccommodations);

            return TopLocationCheck(topLocationDTOs);
        }

        public List<TopAndWorstLocationDTO> FindAllLocationsToTopLocation(List<Accommodation> allAccommodations)
        {
            List<TopAndWorstLocationDTO> topLocationDTOs = new List<TopAndWorstLocationDTO>();

            foreach(Accommodation temporaryAccommodation in allAccommodations.ToList())
            {
                if(temporaryAccommodation.Removed == false)
                {
                    string location = temporaryAccommodation.Location.Country + ", " + temporaryAccommodation.Location.City;

                    bool isAlreadyAdded = topLocationDTOs.ToList().Exists(x => x.Location.Equals(location) == true);

                    if (isAlreadyAdded == false)
                    {
                        decimal totalBusyPercentage = FindLocationTotalBusyPercentage(temporaryAccommodation.Id, 2022, 2024);
                        TopAndWorstLocationDTO topLocationDTO = new TopAndWorstLocationDTO(temporaryAccommodation.Location, totalBusyPercentage); //
                        topLocationDTOs.Add(topLocationDTO);
                    }
                }
            }

            return topLocationDTOs;
        }

        public decimal FindLocationTotalBusyPercentage(int accommodationId, int startYear, int endYear)
        {
            List<int> years = new List<int>();

            for(int year = startYear; year <= endYear; year++)
            {
                years.Add(year);
            }

            List<Reservation> accommodationReservations = reservationService.FindByAccommodationId(accommodationId);

            return FindLocationBusyYears(accommodationReservations, years, startYear, endYear);
        }

        public decimal FindLocationBusyYears(List<Reservation> reservations, List<int> years, int startYear, int endYear)
        {
            Dictionary<int, decimal> busyYears = new Dictionary<int, decimal>();

            foreach (int year in years.ToList())
            {
                busyYears[year] = 0;
            }

            foreach (Reservation temporaryAccommodationReservation in reservations.ToList())
            {
                bool dateConditions = temporaryAccommodationReservation.StartDate.Year >= startYear && temporaryAccommodationReservation.EndDate.Year <= endYear;

                while(temporaryAccommodationReservation.StartDate.Year != temporaryAccommodationReservation.EndDate.Year)
                {
                    DateTime temporaryStartDate = new DateTime(temporaryAccommodationReservation.EndDate.Year, 1, 1);
                    busyYears[temporaryAccommodationReservation.EndDate.Year] += (temporaryAccommodationReservation.EndDate.Subtract(temporaryStartDate).Days + 1); //

                    temporaryAccommodationReservation.EndDate = temporaryAccommodationReservation.EndDate.AddYears(-1);
                    temporaryAccommodationReservation.EndDate = new DateTime(temporaryAccommodationReservation.EndDate.Year, 12, 31);
                }

                if (dateConditions == true)
                {
                    busyYears[temporaryAccommodationReservation.StartDate.Year] += (temporaryAccommodationReservation.EndDate.Subtract(temporaryAccommodationReservation.StartDate).Days + 1); //
                }
            }

            DateTime yearStartDate = new DateTime(startYear, 1, 1);
            DateTime yearEndDate = new DateTime(endYear, 12, 31);
            int totalDays = yearEndDate.Subtract(yearStartDate).Days + 1;

            return busyYears.Sum(x => x.Value) / totalDays;
        }

        public string TopLocationCheck(List<TopAndWorstLocationDTO> topLocationDTOs)
        {
            if (topLocationDTOs.Count == 0)
            {
                return "-";
            }

            TopAndWorstLocationDTO topLocation = topLocationDTOs.MaxBy(x => x.TotalBusyPercentage);

            if(topLocation.TotalBusyPercentage == 0)
            {
                return "-";
            }

            return topLocation.Location.ToString();
        }

        public string FindWorstLocation(string ownerUsername)
        {
            List<Accommodation> allAccommodations = accommodationRepository.FindAll();

            List<TopAndWorstLocationDTO> worstLocationDTOs = FindAllLocationsToWorstLocation(allAccommodations, ownerUsername);

            if (worstLocationDTOs.Count > 0)
            {
                TopAndWorstLocationDTO worstLocation = worstLocationDTOs.MinBy(x => x.TotalBusyPercentage);

                return worstLocation.Location.ToString();
            }

            return "-";
        }

        public List<TopAndWorstLocationDTO> FindAllLocationsToWorstLocation(List<Accommodation> allAccommodations, string ownerUsername)
        {
            List<TopAndWorstLocationDTO> worstLocationDTOs = new List<TopAndWorstLocationDTO>();

            foreach (Accommodation temporaryAccommodation in allAccommodations.ToList())
            {
                if(temporaryAccommodation.OwnerUsername.Equals(ownerUsername) == true)
                {
                    if(temporaryAccommodation.Removed == false)
                    {
                        string location = temporaryAccommodation.Location.Country + ", " + temporaryAccommodation.Location.City;

                        bool isAlreadyAdded = worstLocationDTOs.ToList().Exists(x => x.Location.Equals(location) == true);

                        if(isAlreadyAdded == false)
                        {
                            decimal totalBusyPercentage = FindLocationTotalBusyPercentage(temporaryAccommodation.Id, 2022, 2024);
                            TopAndWorstLocationDTO worstLocationDTO = new TopAndWorstLocationDTO(temporaryAccommodation.Location, totalBusyPercentage); //
                            worstLocationDTOs.Add(worstLocationDTO);
                        }
                    }
                }
            }

            return worstLocationDTOs;
        }

        public bool RemoveWorstLocations(string ownerUsername)
        {
            List<Accommodation> allAccommodations = accommodationRepository.FindAll();

            List<TopAndWorstLocationDTO> worstLocationDTOs = FindAllLocationsToWorstLocation(allAccommodations, ownerUsername);

            List<TopAndWorstLocationDTO> worstLocationDTOsToRemove = FindAllToRemove(worstLocationDTOs, ownerUsername);

            return RemoveWorst(worstLocationDTOsToRemove);
        }

        public List<TopAndWorstLocationDTO> FindAllToRemove(List<TopAndWorstLocationDTO> worstLocationDTOs, string ownerUsername)
        {
            List<TopAndWorstLocationDTO> worstLocationDTOsToRemove = new List<TopAndWorstLocationDTO>();

            foreach(TopAndWorstLocationDTO temporaryworstLocationDTOToRemove in worstLocationDTOs.ToList())
            {
                List<int> locationIds = locationService.FindIdByCountryAndCity(temporaryworstLocationDTOToRemove);

                foreach(int temporaryLocationId in locationIds.ToList())
                {
                    if(CheckFutureReservations(temporaryLocationId, ownerUsername) == false && CheckFutureRenovations(temporaryLocationId, ownerUsername) == false)
                    {
                        worstLocationDTOsToRemove.Add(temporaryworstLocationDTOToRemove);
                    }
                }
            }

            return worstLocationDTOsToRemove;
        }

        public bool CheckFutureReservations(int locationId, string ownerUsername)
        {
            return reservationService.CheckFutureReservations(locationId, ownerUsername);
        }

        public bool CheckFutureRenovations(int locationId, string ownerUsername)
        {
            return renovationService.CheckFutureRenovations(locationId, ownerUsername);
        }

        public bool RemoveWorst(List<TopAndWorstLocationDTO> worstLocationDTOs)
        {
            TopAndWorstLocationDTO worstLocation = worstLocationDTOs.MinBy(x => x.TotalBusyPercentage);

            if(worstLocation != null)
            {
                string country = worstLocation.Location.Split(", ")[0];
                string city = worstLocation.Location.Split(", ")[1];

                accommodationRepository.Remove(country, city);

                return true;
            }

            return false;
        }

        public int FindNumberOfNewForums(string ownerUsername)
        {
            return forumNotificationsToOwnerService.FindNumberOfNewForums(ownerUsername);
        }

        public void MarkAsReadNotificationsForums(string ownerUsername)
        {
            forumNotificationsToOwnerService.MarkAsReadNotificationsForums(ownerUsername);
        }
    }
}
