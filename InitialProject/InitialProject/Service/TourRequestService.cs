using InitialProject.Dto;
using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



    }
}
