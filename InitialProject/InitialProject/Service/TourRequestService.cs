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
            List<TourRequest> countryResults, cityResults, guestNumberResults, languageResults;
            List<TourRequest> allTourRequests = tourRequestRepository.FindAll();

            //if (!IsNameContained(allAccommodations, searchShowAndAccommodationDTO.AccommodationName, out accommodationNameResults)) return null;
            if (!IsCountryContained(allTourRequests, searchShowTourRequestDTO.Country, out countryResults)) return null;
            if (!IsCityContained(allTourRequests, searchShowTourRequestDTO.City, out cityResults)) return null;
            //if (!IsTypeContained(allAccommodations, searchShowAndAccommodationDTO.Type, out typeResults)) return null;
            if (!IsGuestsNumberContained(allTourRequests, searchShowTourRequestDTO.GuestNumber, out guestNumberResults)) return null;
            //if (!AreReservationDaysContained(allAccommodations, searchShowAndAccommodationDTO.MinDaysReservation, out minDaysReservationResults)) return null;
            if (!IsLanguageContained(allTourRequests, searchShowTourRequestDTO.Language, out languageResults)) return null;

            allTourRequests = cityResults.Intersect(countryResults).Intersect(guestNumberResults).Intersect(languageResults).ToList();

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
    }
}
