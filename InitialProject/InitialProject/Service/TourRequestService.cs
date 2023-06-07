using InitialProject.Dto;
using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace InitialProject.Service
{
    public class TourRequestService
    {
        private readonly ITourRequestRepository tourRequestRepository;

        public TourRequestService()
        {
            tourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();
        }

        public List<TourRequest> FindAll(SearchAndShowTourRequestDTO searchShowTourRequestDTO)
        {
            List<TourRequest> countryResults, cityResults, guestNumberResults, languageResults, dateResults;
            List<TourRequest> allTourRequests = tourRequestRepository.FindAll();

            //if (!IsNameContained(allAccommodations, searchShowAndAccommodationDTO.AccommodationName, out accommodationNameResults)) return null;
            if (!IsCountryContained(allTourRequests, searchShowTourRequestDTO.Country, out countryResults)) return null;
            if (!IsCityContained(allTourRequests, searchShowTourRequestDTO.City, out cityResults)) return null;
            //if (!IsTypeContained(allAccommodations, searchShowAndAccommodationDTO.Type, out typeResults)) return null;
            if (!IsGuestsNumberContained(allTourRequests, searchShowTourRequestDTO.GuestNumber, out guestNumberResults)) return null;
            //if (!AreReservationDaysContained(allAccommodations, searchShowAndAccommodationDTO.MinDaysReservation, out minDaysReservationResults)) return null;
            if (!IsLanguageContained(allTourRequests, searchShowTourRequestDTO.Language, out languageResults)) return null;
            if (!IsDateContained(allTourRequests, searchShowTourRequestDTO.StartDate, searchShowTourRequestDTO.EndDate, out dateResults)) return null;

            allTourRequests = cityResults.Intersect(countryResults).Intersect(guestNumberResults).Intersect(languageResults).Intersect(dateResults).ToList();

            return allTourRequests;
        }

        private bool IsCountryContained(List<TourRequest> allTourRequests, string country, out List<TourRequest> countryResults)
        {
            if (!string.IsNullOrWhiteSpace(country))
            {
                country = country.Trim();
                countryResults = tourRequestRepository.FindAllByCountry(allTourRequests, country);
                if (countryResults.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                countryResults = allTourRequests;
            }

            return true;
        }

        private bool IsCityContained(List<TourRequest> allTourRequests, string city, out List<TourRequest> cityResults)
        {
            if (!string.IsNullOrWhiteSpace(city))
            {
                city = city.Trim();
                cityResults = tourRequestRepository.FindAllByCity(allTourRequests, city);
                if (cityResults.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                cityResults = allTourRequests;
            }

            return true;
        }

        private bool IsGuestsNumberContained(List<TourRequest> allTourRequests, int? guestNumber, out List<TourRequest> guestNumberResults)
        {
            if ((guestNumber != null) && (guestNumber > 0))
            {
                guestNumberResults = tourRequestRepository.FindAllByGuestsNumber(allTourRequests, guestNumber);
                if (guestNumberResults.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                guestNumberResults = allTourRequests;
            }

            return true;
        }

        private bool IsLanguageContained(List<TourRequest> allTourRequests, string? language, out List<TourRequest> languageResults)
        {
            if (!string.IsNullOrWhiteSpace(language))
            {
                language = language.Trim();
                languageResults = tourRequestRepository.FindAllByLanguage(allTourRequests, language);
                if (languageResults.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                languageResults = allTourRequests;
            }

            return true;
        }

        private bool IsDateContained(List<TourRequest> allTourRequests, DateTime filterStartDate, DateTime filterEndDate, out List<TourRequest> dateResults)
        {            
            dateResults = tourRequestRepository.FindAllByDateRange(allTourRequests, filterStartDate, filterEndDate);
            if (dateResults.Count == 0)
            {
                return false;
            }
            return true;
        }

        public List<TourRequest> FindAll(string country, string city, string language)
        {
            List<TourRequest> countryResults, cityResults, languageResults;
            List<TourRequest> allTourRequests = tourRequestRepository.FindAll();

            //if (!IsNameContained(allAccommodations, searchShowAndAccommodationDTO.AccommodationName, out accommodationNameResults)) return null;
            if (!IsCountryContained(allTourRequests, country, out countryResults)) return null;
            if (!IsCityContained(allTourRequests, city, out cityResults)) return null;
            //if (!IsTypeContained(allAccommodations, searchShowAndAccommodationDTO.Type, out typeResults)) return null;
            //if (!IsGuestsNumberContained(allTourRequests, searchShowTourRequestDTO.GuestNumber, out guestNumberResults)) return null;
            //if (!AreReservationDaysContained(allAccommodations, searchShowAndAccommodationDTO.MinDaysReservation, out minDaysReservationResults)) return null;
            if (!IsLanguageContained(allTourRequests, language, out languageResults)) return null;
            //if (!IsDateContained(allTourRequests, searchShowTourRequestDTO.StartDate, searchShowTourRequestDTO.EndDate, out dateResults)) return null;

            allTourRequests = cityResults.Intersect(countryResults).Intersect(languageResults).ToList();

            return allTourRequests;
        }

        public void UpdateStatusToAccepted(TourRequest selectedItem)
        {
            List<TourRequest> allTourRequests = tourRequestRepository.FindAll();
            foreach(TourRequest requests in  allTourRequests)
            {
                if(selectedItem.Id == requests.Id) 
                {
                    requests.Status = "accepted";
                    break;
                }
            }
            tourRequestRepository.Save(allTourRequests);
        }

        public bool CheckIfIsInDateRange(TourRequest tourRequest, DateTime optionalDate)
        {
            if(optionalDate>= tourRequest.StartDate && optionalDate<= tourRequest.EndDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SaveTourRequest(TourRequest tourRequest)
        {
            try
            {
                tourRequestRepository.Save(tourRequest);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AcceptTourRequest(string guideUsername, TourRequest tourRequest, DateTime selectedStartDate)
        {
            TourGuidenceService tourGuidenceService = new TourGuidenceService();

            if(!CheckIfIsInDateRange(tourRequest, selectedStartDate) || !tourGuidenceService.CheckIfGuideIsFreeInPeriod(guideUsername, selectedStartDate))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
            

        public Location FindMostFrequentLocationInLastYear()
        {
            DateTime lastYearPeriod = DateTime.Now.AddYears(-1);
            List<TourRequest> tourRequests = tourRequestRepository.FindAll().Where(t => t.CreationDate>=lastYearPeriod).ToList();
            LocationService locationService = new LocationService();
            int locId = tourRequests.GroupBy(t => t.Location.Id).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault();
            return locationService.FindById(locId);
        }

        public Language FindMostFrequentLanguageInLastYear()
        {
            DateTime lastYearPeriod = DateTime.Now.AddYears(-1);
            List<TourRequest> tourRequests = tourRequestRepository.FindAll().Where(t => t.CreationDate>=lastYearPeriod).ToList();
            return tourRequests.GroupBy(t => t.Language).OrderByDescending(g => g.Count()).Select(g=>g.Key).FirstOrDefault();
        }

        public List<(int Year, int Count)> FindCountTourRequestsForYears(List<TourRequest> tourRequests)
        {
            //List<TourRequest> tourRequests = tourRequestRepository.FindAll();
            return tourRequests.GroupBy(t => t.CreationDate.Year).Select(g => (Year: g.Key, Count: g.Count())).ToList();
        }

        public List<(int Month, int Count)> FindCountTourRequestForMonthsForSpecificYear(List<TourRequest> tourRequests, int year)
        {
            List<TourRequest> requests = tourRequests.FindAll(x => x.CreationDate.Year == year).ToList();
            return requests.GroupBy(t => t.CreationDate.Month).OrderBy(x => x.Key).Select(g => (Month: g.Key, Count: g.Count())).ToList();
        }



        public List<TourRequest> GetByUser(string username)
        {
            List<TourRequest> tourRequests = new List<TourRequest>();

            foreach(TourRequest tourRequest in tourRequestRepository.FindAll())
            {
                if(tourRequest.User.Username.Equals(username))
                    tourRequests.Add(tourRequest);
            }

            return tourRequests;
        }


        public List<TourRequest> GetRejectedByUser(string username)
        {
            List<TourRequest> tourRequests = new List<TourRequest>();

            foreach(TourRequest tourRequest in GetByUser(username))
            {
                if(tourRequest.Status.Equals("invalid"))
                {
                    tourRequests.Add(tourRequest);
                }
            }

            return tourRequests;
        }

        public int CountAcceptedForUser(string username,string year)
        {
            int number = 0;

            foreach (TourRequest tourRequest in GetByUser(username))
            {
                if(tourRequest.Status.Equals("accepted") && string.IsNullOrEmpty(year))
                {
                    number++;
                }

                else if(tourRequest.Status.Equals("accepted") && tourRequest.StartDate.Year.ToString().Equals(year))
                {
                    number++;
                }
            }

            return number;

        }


        public int CountInvalidForUser(string username,string year)
        {
            int number = 0;

            foreach (TourRequest tourRequest in GetByUser(username))
            {
                if (tourRequest.Status.Equals("invalid") && string.IsNullOrEmpty(year))
                {
                    number++;
                }

                else if(tourRequest.Status.Equals("invalid") && tourRequest.StartDate.Year.ToString().Equals(year))
                {
                    number++;
                }
            }

            return number;

        }


        public List<string> GetYears(string username)
        {
            List<string> years = new List<string>();

            foreach(TourRequest tourRequest in GetByUser(username))
            {
                years.Add(tourRequest.StartDate.Year.ToString());
            }

            return years.Distinct().ToList();

        }

        public int GetCountOfLanguage(string username,Language language)
        {
            int number = 0;

            foreach (TourRequest tourRequest in GetByUser(username))
            {
                if(tourRequest.Language.Equals(language))
                {
                    number++;
                }
            }

            return number;
        }

        public double GetAverageGuestsByYear(string username,string year)
        {
            int count = 0;
            double average = 0;


            foreach(TourRequest tourRequest in GetByUser(username))
            {
                if (tourRequest.Status.Equals("accepted"))
                {
                    if (string.IsNullOrEmpty(year))
                    {
                        count++;
                        average += tourRequest.GuestNumber;
                    }

                    else if (tourRequest.StartDate.Year.ToString().Equals(year))
                    {
                        count++;
                        average += tourRequest.GuestNumber;
                    }
                }
            }

            return  Math.Round(average/count,2);

        }

        public Dictionary<string, int> LocationCount(string username)
        {
            Dictionary<string, int> locationCounts = new Dictionary<string, int>();

            LocationService locationService = new LocationService();

            List<Location> locations = locationService.FindAll();

            foreach(TourRequest tourRequest in tourRequestRepository.FindAllByUser(username))
            {
                string key = tourRequest.Location.Country + "_" + tourRequest.Location.City;

                if (locationCounts.ContainsKey(key))
                {
                    locationCounts[key]++;
                }
                else
                {
                    locationCounts[key] = 1;
                } 
            }

            return locationCounts;

        }

        public void SaveList(List<TourRequest> tourRequests)
        {
            tourRequestRepository.SaveList(tourRequests);
        }


    }
}
