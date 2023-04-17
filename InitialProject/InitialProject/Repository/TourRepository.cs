using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Repository
{
    internal class TourRepository
    {
        private const string FilePathTour = "../../../Resources/Data/tours.csv";

        private const string FilePathTourKeyPoints = "../../../Resources/Data/tourkeypoints.csv";

        private const string FilePathLocation = "../../../Resources/Data/location.csv";

        private const string FilePathReservatedTours = "../../../Resources/Data/reservatedtours.csv";


        private readonly Serializer<Tour> tourSerializer;
       
        private readonly Serializer<TourKeyPoint> tourKeyPointsSerializer;

        private readonly Serializer<Location> locationSerializer;

        private readonly Serializer<TourReservation> tourReservationSerializer;


        private List<Tour> tours;

        private List<TourKeyPoint> tourKeyPoints;

        private List<Location> locations;

        private List<TourReservation> tourReservations;



        public TourRepository()
        {
            tourSerializer = new Serializer<Tour>();
            tours = tourSerializer.FromCSV(FilePathTour);

            tourKeyPointsSerializer = new Serializer<TourKeyPoint>();
            //tourKeyPoints = tourKeyPointsSerializer.FromCSV(FilePathTourKeyPoints);

            locationSerializer = new Serializer<Location>();
            locations = locationSerializer.FromCSV(FilePathLocation);

            tourReservationSerializer = new Serializer<TourReservation>();
            tourReservations = tourReservationSerializer.FromCSV(FilePathReservatedTours);


        }

        public List<Tour> Load()
        { 
            return tours;
        }

        public Tour GetByName(string id)
        {
            Tour result = new Tour();
            foreach(Tour tour in tours)
            {
                if(id.Equals(tour.TourName))
                {
                    result = tour;
                    break;
                }
            }

            return result;
        }

        public List<TourDisplayDTO> GetToursForDisplay()
        {
            List<TourDisplayDTO> tourDisplayDTOs = new List<TourDisplayDTO>();
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            List<TourGuidence> tourGuidences = tourGuidenceRepository.GetAll();


            foreach(TourGuidence tourGuidence in tourGuidences)
            {
                TourDisplayDTO tourDisplayDTO = new TourDisplayDTO();

                tourDisplayDTO.TourName = tourGuidence.Tour.TourName;
                tourDisplayDTO.Location = tourGuidence.Tour.Location;
                tourDisplayDTO.Description = tourGuidence.Tour.Description;
                tourDisplayDTO.Language = tourGuidence.Tour.Language;
                tourDisplayDTO.FreeSlots = tourGuidence.FreeSlots;
                tourDisplayDTO.TourDate = tourGuidence.StartTime;

                tourDisplayDTO.TourKeyPoints = new List<TourKeyPoint>();

                foreach (TourKeyPoint tourKeyPoint in tourKeyPointRepository.GetAll())
                {
                    if(tourKeyPoint.TourGuidence.Id == tourGuidence.Id)
                    {
                        tourDisplayDTO.TourKeyPoints.Add(tourKeyPoint);
                    }
                }

                tourDisplayDTO.Duration = tourGuidence.Tour.Duration;
                tourDisplayDTO.Images = tourGuidence.Tour.Images;



                if(tourDisplayDTO.FreeSlots>0)
                    tourDisplayDTOs.Add(tourDisplayDTO);

            }

            return tourDisplayDTOs;
        }

        public TourDisplayDTO GetTourForDisplay(int tourReservationId)
        {
            TourDisplayDTO tourDisplayDTO = new TourDisplayDTO();
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            List<TourGuidence> tourGuidences = tourGuidenceRepository.GetAll();
            List<TourReservation> tourReservations = tourReservationRepository.GetAll();


            foreach (TourReservation tourReservation in tourReservations)
            {
                foreach (TourGuidence tourGuidence in tourGuidences)
                {
                    if (tourGuidence.Id == tourReservation.tourGuidenceId && tourReservationId==tourReservation.Id)
                    {

                        tourDisplayDTO.TourName = tourGuidence.Tour.TourName;
                        tourDisplayDTO.Location = tourGuidence.Tour.Location;
                        tourDisplayDTO.Description = tourGuidence.Tour.Description;
                        tourDisplayDTO.Language = tourGuidence.Tour.Language;
                        tourDisplayDTO.FreeSlots = tourGuidence.FreeSlots;
                        tourDisplayDTO.TourDate = tourGuidence.StartTime;

                        tourDisplayDTO.TourKeyPoints = new List<TourKeyPoint>();

                        foreach (TourKeyPoint tourKeyPoint in tourKeyPointRepository.GetAll())
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

        public bool CreateReservation(string username,TourGuidence tourGuidence, List<Boolean> arrivals, int numberOfGuests,int voucherId)
        {
            try
            {
                TourReservation reservatedTour = new TourReservation(username,tourGuidence.Id,arrivals, numberOfGuests, false,voucherId);
                tourReservations.Add(reservatedTour);
                tourReservationSerializer.ToCSV(FilePathReservatedTours, tourReservations);
                //UpdateTourFreeSlot(tour,numberOfGuests);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public List<TourDisplayDTO> SearchAndShow(string city=null,string country=null,int duration=0,Language language = 0,int numberOfGuests=0)
        {
            List<TourDisplayDTO> sameCity = new List<TourDisplayDTO>();
            List<TourDisplayDTO> sameCountry = new List<TourDisplayDTO>();
            List<TourDisplayDTO> longerDuration = new List<TourDisplayDTO>();
            List<TourDisplayDTO> sameLanguage = new List<TourDisplayDTO>();
            List<TourDisplayDTO> moreGuests = new List<TourDisplayDTO>();

            List<TourDisplayDTO> displayedTours = GetToursForDisplay();

            sameCity = CheckSamCity(city, sameCity, displayedTours);
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

        private static List<TourDisplayDTO> CheckFreeSlots(int numberOfGuests, List<TourDisplayDTO> moreGuests, List<TourDisplayDTO> displayedTours)
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

        private static List<TourDisplayDTO> CheckSameLanguage(Language language, List<TourDisplayDTO> sameLanguage, List<TourDisplayDTO> displayedTours)
        {
            if (language >= 0)
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

        private static List<TourDisplayDTO> CheckLongerDuration(int duration, List<TourDisplayDTO> longerDuration, List<TourDisplayDTO> displayedTours)
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

        private static List<TourDisplayDTO> CheckSamCity(string city, List<TourDisplayDTO> sameCity, List<TourDisplayDTO> displayedTours)
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

        private static List<TourDisplayDTO> CheckSameCountry(string country, List<TourDisplayDTO> sameCountry, List<TourDisplayDTO> displayedTours)
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

        public List<TourDisplayDTO> sameTours(List<TourDisplayDTO> list1,List<TourDisplayDTO> list2)
        {
            List<TourDisplayDTO> result = new List<TourDisplayDTO>();

            foreach (TourDisplayDTO tour1 in list1)
            {
                foreach ( TourDisplayDTO tour2 in list2 )
                {
                    if(tour1.Equals(tour2))
                    { 
                        result.Add(tour1); 
                    }
                }
            }

            return result;
        }


        public Tour Save(TourDto tourDto)
        {
            Tour tour = new Tour(NextIdTour(), tourDto.TourName, tourDto.TourLocation, tourDto.Description, tourDto.Languages, tourDto.MaxGuests, tourDto.Duration, tourDto.Images, tourDto.Username);
            tours.Add(tour);
            tourSerializer.ToCSV(FilePathTour, tours);
            return tour;
        }

        public int NextIdTour()
        {
            tours = tourSerializer.FromCSV(FilePathTour);
            if (tours.Count < 1)
            {
                return 1;
            }
            return tours.Max(c => c.Id) + 1;
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

        public int NextIdTourKeyPoints()
        {
            tourKeyPoints = tourKeyPointsSerializer.FromCSV(FilePathTourKeyPoints);
            if (tourKeyPoints.Count < 1)
            {
                return 1;
            }
            return tourKeyPoints.Max(c => c.Id) + 1;
        }

        public Tour GetById(int id) => tours.FirstOrDefault(x => x.Id == id);

        public List<int> GetGuestNumber(int tourId)
        {
            // type = [(1, <18), (2, 18-50), (3, >50)]
            List<int> count = new List<int>(new int[3]);
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();
            Guest2Repository guest2Repository = new Guest2Repository();
            List<TourGuidence> tourGuidences = tourGuidenceRepository.GetAll();
            foreach (TourGuidence tourGuidence in tourGuidences)
            {
                if (tourGuidence.Finished == true && tourId == tourGuidence.Tour.Id)
                {
                    foreach (TourReservation tourReservation in tourReservations)
                    {
                        if (tourReservation.tourGuidenceId == tourGuidence.Id)
                        {
                            int age = guest2Repository.GetAge(tourReservation.userId);
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
                }
            }
            return count;
        }

        public List<double> GetVoucherPercentage(int tourId)
        {
            List<double> retVal = new List<double>(new double[2]);
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();
            List<TourGuidence> tourGuidences = tourGuidenceRepository.GetAll();
            double withVoucher = 0, count = 0;

            foreach(TourGuidence tg in tourGuidences)
            {
                if(tg.Finished == true && tg.Tour.Id == tourId)
                {
                    foreach(TourReservation tr in tourReservations)
                    {
                        if(tg.Id == tr.tourGuidenceId && tr.Confirmed == true)
                        {
                            if(tr.VoucherId != 0)
                            {
                                withVoucher++;
                            }

                                count++;
                        }
                    }
                }
            }
            retVal[0] = Math.Round((withVoucher / count) * 100, 2);
            retVal[1] = Math.Round((1-(withVoucher / count)) * 100,2);
            return retVal;  
        }




    }
}
