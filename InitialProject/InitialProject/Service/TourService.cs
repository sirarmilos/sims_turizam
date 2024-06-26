﻿using InitialProject.Dto;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourService
    {
        private readonly ITourRepository tourRepository;

        public TourService()
        {
            tourRepository = Injector.Injector.CreateInstance<ITourRepository>();
        }

        public List<Tour> FindAll()
        {
            return tourRepository.FindAll();
        }


        public List<TourGuidence> GetSuperGuideTours()
        {
            UserService userService = new UserService();
            TourGuidenceService tourGuidenceService = new TourGuidenceService();

            List<TourGuidence> tourGuidences = new List<TourGuidence>();    

            foreach(TourGuidence tourGuidence in tourGuidenceService.FindAll())
            {
                if (userService.FindByUsername(tourGuidence.Tour.GuideUsername).SuperType.Equals("super"))
                {
                    tourGuidences.Add(tourGuidence);
                }
            }

            return tourGuidences;
        }

        public List<TourGuidence> GetNonSuperGuideTours()
        {
            UserService userService = new UserService();
            TourGuidenceService tourGuidenceService = new TourGuidenceService();

            List<TourGuidence> tourGuidences = new List<TourGuidence>();

            foreach (TourGuidence tourGuidence in tourGuidenceService.FindAll())
            {
                if (!userService.FindByUsername(tourGuidence.Tour.GuideUsername).SuperType.Equals("super"))
                {
                    tourGuidences.Add(tourGuidence);
                }
            }

            return tourGuidences;
        }

        public List<TourDisplayDTO> GetToursForDisplay()
        {
            List<TourDisplayDTO> tourDisplayDTOs = new List<TourDisplayDTO>();
            TourGuidenceService tourGuidanceService = new TourGuidenceService();
            TourKeyPointService tourKeyPointService = new TourKeyPointService();


            List<TourGuidence> tourGuidencesSuperGuide = GetSuperGuideTours();
            List<TourGuidence> tourGuidencesNonSuperGuide = GetNonSuperGuideTours();
            List<TourGuidence> tourGuidences = tourGuidencesSuperGuide.Concat(tourGuidencesNonSuperGuide).ToList();


            foreach (TourGuidence tourGuidence in tourGuidences)
            {
                TourDisplayDTO tourDisplayDTO = new TourDisplayDTO();

                tourDisplayDTO.TourName = tourGuidence.Tour.TourName;
                tourDisplayDTO.Location = tourGuidence.Tour.Location;
                tourDisplayDTO.Description = tourGuidence.Tour.Description;
                tourDisplayDTO.Language = tourGuidence.Tour.Language;
                tourDisplayDTO.FreeSlots = tourGuidence.FreeSlots;
                tourDisplayDTO.TourDate = tourGuidence.StartTime;

                tourDisplayDTO.TourKeyPoints = new List<TourKeyPoint>();

                foreach (TourKeyPoint tourKeyPoint in tourKeyPointService.FindAll())
                {
                    if (tourKeyPoint.TourGuidence.Id == tourGuidence.Id)
                    {
                        tourDisplayDTO.TourKeyPoints.Add(tourKeyPoint);
                    }
                }

                tourDisplayDTO.Duration = tourGuidence.Tour.Duration;
                tourDisplayDTO.Images = tourGuidence.Tour.Images;



                if (tourDisplayDTO.FreeSlots > 0)
                    tourDisplayDTOs.Add(tourDisplayDTO);

            }

            return tourDisplayDTOs;
        }

        public TourDisplayDTO GetTourForDisplay(int tourReservationId)
        {
            TourDisplayDTO tourDisplayDTO = new TourDisplayDTO();


            TourKeyPointService tourKeyPointService = new TourKeyPointService();
            TourReservationService tourReservationService = new TourReservationService();
            TourGuidenceService tourGuidanceService = new TourGuidenceService();



            foreach (Model.TourReservation tourReservation in tourReservationService.FindAll())
            {
                foreach (TourGuidence tourGuidence in tourGuidanceService.FindAll())
                {
                    if (tourGuidence.Id == tourReservation.tourGuidenceId && tourReservationId == tourReservation.Id)
                    {

                        tourDisplayDTO.TourName = tourGuidence.Tour.TourName;
                        tourDisplayDTO.Location = tourGuidence.Tour.Location;
                        tourDisplayDTO.Description = tourGuidence.Tour.Description;
                        tourDisplayDTO.Language = tourGuidence.Tour.Language;
                        tourDisplayDTO.FreeSlots = tourGuidence.FreeSlots;
                        tourDisplayDTO.TourDate = tourGuidence.StartTime;

                        tourDisplayDTO.TourKeyPoints = new List<TourKeyPoint>();

                        foreach (TourKeyPoint tourKeyPoint in tourKeyPointService.FindAll())
                        {
                            if (tourKeyPoint.TourGuidence.Id == tourGuidence.Id)
                            {
                                tourDisplayDTO.TourKeyPoints.Add(tourKeyPoint);
                            }
                        }

                        tourDisplayDTO.Duration = tourGuidence.Tour.Duration;
                        tourDisplayDTO.Images = tourGuidence.Tour.Images;

                        break;
                    }

                }
            }

            return tourDisplayDTO;
        }


        public List<TourDisplayDTO> SearchAndShow(string city = null, string country = null, int duration = 0, Language language = 0, int numberOfGuests = 0)
        {
            List<TourDisplayDTO> sameCity = new List<TourDisplayDTO>();
            List<TourDisplayDTO> sameCountry = new List<TourDisplayDTO>();
            List<TourDisplayDTO> longerDuration = new List<TourDisplayDTO>();
            List<TourDisplayDTO> sameLanguage = new List<TourDisplayDTO>();
            List<TourDisplayDTO> moreGuests = new List<TourDisplayDTO>();

            List<TourDisplayDTO> displayedTours = GetToursForDisplay();

            sameCity = CheckSameCity(city, sameCity, displayedTours);
            sameCountry = CheckSameCountry(country, sameCountry, displayedTours);
            longerDuration = CheckLongerDuration(duration, longerDuration, displayedTours);
            sameLanguage = CheckSameLanguage(language, sameLanguage, displayedTours);
            moreGuests = CheckFreeSlots(numberOfGuests, moreGuests, displayedTours);

            List<TourDisplayDTO> result = GetIntersections(sameCity, sameCountry, longerDuration, sameLanguage, moreGuests);

            return result;

        }

        private List<TourDisplayDTO> GetIntersections(List<TourDisplayDTO> sameCity, List<TourDisplayDTO> sameCountry, List<TourDisplayDTO> longerDuration, List<TourDisplayDTO> sameLanguage, List<TourDisplayDTO> moreGuests)
        {
            List<TourDisplayDTO> result = sameTours(sameCity, sameCountry);
            result = sameTours(result, longerDuration);
            result = sameTours(result, sameLanguage);
            result = sameTours(result, moreGuests);
            return result;
        }

        private  List<TourDisplayDTO> CheckFreeSlots(int numberOfGuests, List<TourDisplayDTO> moreGuests, List<TourDisplayDTO> displayedTours)
        {
            if (numberOfGuests >= 0)
            {
                foreach (TourDisplayDTO tour in displayedTours)
                {
                    if (tour.FreeSlots >= numberOfGuests)
                    {
                        moreGuests.Add(tour);
                    }
                }
            }
            else
            {
                moreGuests = displayedTours;
            }

            return moreGuests;
        }

        private  List<TourDisplayDTO> CheckSameLanguage(Language language, List<TourDisplayDTO> sameLanguage, List<TourDisplayDTO> displayedTours)
        {
            if (language > 0)
            {
                foreach (TourDisplayDTO tour in displayedTours)
                {
                    if (tour.Language == language)
                    {
                        sameLanguage.Add(tour);
                    }
                }
            }
            else
            {
                sameLanguage = displayedTours;
            }

            return sameLanguage;
        }

        private  List<TourDisplayDTO> CheckLongerDuration(int duration, List<TourDisplayDTO> longerDuration, List<TourDisplayDTO> displayedTours)
        {
            if (duration >= 0)
            {
                foreach (TourDisplayDTO tour in displayedTours)
                {
                    if (tour.Duration >= duration)
                    {
                        longerDuration.Add(tour);
                    }
                }
            }
            else
            {
                longerDuration = displayedTours;
            }

            return longerDuration;
        }

        private  List<TourDisplayDTO> CheckSameCity(string city, List<TourDisplayDTO> sameCity, List<TourDisplayDTO> displayedTours)
        {
            if (!string.IsNullOrEmpty(city))
            {
                foreach (TourDisplayDTO tour in displayedTours)
                {
                    if (tour.Location.City.ToLower().StartsWith(city.ToLower()))
                    {
                        sameCity.Add(tour);
                    }
                }
            }
            else
            {
                sameCity = displayedTours;
            }

            return sameCity;
        }

        private  List<TourDisplayDTO> CheckSameCountry(string country, List<TourDisplayDTO> sameCountry, List<TourDisplayDTO> displayedTours)
        {
            if (!string.IsNullOrEmpty(country))
            {
                foreach (TourDisplayDTO tour in displayedTours)
                {
                    if (tour.Location.Country.ToLower().StartsWith(country.ToLower()))
                    {
                        sameCountry.Add(tour);
                    }
                }
            }
            else
            {
                sameCountry = displayedTours;
            }

            return sameCountry;
        }

        public List<TourDisplayDTO> sameTours(List<TourDisplayDTO> list1, List<TourDisplayDTO> list2)
        {
            List<TourDisplayDTO> result = new List<TourDisplayDTO>();

            foreach (TourDisplayDTO tour1 in list1)
            {
                foreach (TourDisplayDTO tour2 in list2)
                {
                    if (tour1.Equals(tour2))
                    {
                        result.Add(tour1);
                    }
                }
            }

            return result;
        }

        

        public List<int> FindGuestNumber(int tourId, string username)
        {
            // type = [(1, <18), (2, 18-50), (3, >50)]
            List<int> count = new List<int>(new int[3]);

            TourGuidenceService tourGuidenceService = new TourGuidenceService();

            TourReservationService tourReservationService = new TourReservationService();

            Guest2Service guest2Service = new Guest2Service();

            List<TourGuidence> finishedTourGuidence = tourGuidenceService.FindFinishedByGuideUsername(tourId, username);

            foreach (Model.TourReservation tourReservation in tourReservationService.FindAll())
            {
                List<TourGuidence> finalList = finishedTourGuidence.FindAll(x => x.Id == tourReservation.tourGuidenceId);

                foreach (TourGuidence tourGuidence in finalList)
                {
                    int age = guest2Service.FindAge(tourReservation.userId);
                    switch (age)
                    {
                        case <= 18:
                            count[0]++;
                            break;
                        case >= 50:
                            count[2]++;
                            break;
                        default:
                            count[1]++;
                            break;
                    }
                }
            }

            return count;
        }

        public Tour FindByName(TourDisplayDTO tourDisplayDTO)
        {
            return tourRepository.FindByName(tourDisplayDTO.TourName);
        }


    }
}
