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

        public List<Tour> GetByName(string id)
        {
            List<Tour> result = new List<Tour>();
            foreach(Tour tour in tours)
            {
                if(id.Equals(tour.TourName))
                {
                    result.Add(tour);
                    break;
                }
            }

            return result;
        }

        public bool CreateReservation(string username,Tour tour,int numberOfGuests)
        {
            try
            {
                TourReservation reservatedTour = new TourReservation(username,tour.Id,numberOfGuests);
                tourReservations.Add(reservatedTour);
                tourReservationSerializer.ToCSV(FilePathReservatedTours, tourReservations);
                UpdateTourFreeSlot(tour,numberOfGuests);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateTourFreeSlot(Tour reservatedTour,int numberOfGuests)
        {

            List<Tour> result = tours;

            foreach(Tour tour in tours)
            {
                if(tour.Equals(reservatedTour))
                {
                    tour.FreeSlots = tour.FreeSlots - numberOfGuests;
                    tourSerializer.ToCSV(FilePathTour, tours);
                    break;
                }
            }

        }

        public List<Tour> UpdateDataGrid(Tour reservatedTour)
        {
            List<Tour> temp = SearchAndShow(reservatedTour.Location.City, reservatedTour.Location.Country, 0, Model.Language.ALL, 0);
            List<Tour> result = new List<Tour>();

            foreach(Tour tour in temp)
            {
                if(!tour.Equals(reservatedTour))
                {
                    result.Add(tour);
                }
            }

            return result;
        }

        public List<Tour> SearchAndShow(string city=null,string country=null,int duration=0,Language language = 0,int numberOfGuests=0)
        {
            List<Tour> sameCity = new List<Tour>();
            List<Tour> sameCountry = new List<Tour>();
            List<Tour> longerDuration = new List<Tour>();
            List<Tour> sameLanguage = new List<Tour>();
            List<Tour> moreGuests = new List<Tour>();


            if (!string.IsNullOrEmpty(city))
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Location.City.ToLower().StartsWith(city.ToLower()))
                    {
                        sameCity.Add(tour);
                    }
                }
            }
            else
            {
                sameCity = tours;
            }

            if (!string.IsNullOrEmpty(country))
            {
                foreach(Tour tour in tours)
                {
                    if(tour.Location.Country.ToLower().StartsWith(country.ToLower()))
                    {
                        sameCountry.Add(tour);
                    }
                }
            }
            else
            {
                sameCountry = tours;
            }

            if (duration >= 0)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Duration >= duration)
                    {
                        longerDuration.Add(tour);
                    }
                }
            }
            else
            {
                longerDuration = tours;
            }

            if (language >= 0)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Language == language)
                    {
                        sameLanguage.Add(tour);
                    }
                }
            }
            else
            {
                sameLanguage = tours;
            }


            if (numberOfGuests >= 0)
            {
                foreach(Tour tour in tours)
                {
                    if(tour.MaxGuests >= numberOfGuests)
                    {
                        moreGuests.Add(tour);
                    }
                }
            }
            else
            {
                moreGuests = tours; 
            }


            List<Tour> result = sameTours(sameCity,sameCountry);
            result = sameTours(result,longerDuration);
            result = sameTours(result, sameLanguage);
            result = sameTours(result,moreGuests);


            return result;
            
        }

        public List<Tour> sameTours(List<Tour> list1,List<Tour> list2)
        {
            List<Tour> result = new List<Tour>();

            foreach (Tour tour1 in list1)
            {
                foreach ( Tour tour2 in list2 )
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
            Tour tour = new Tour(NextIdTour(), tourDto.TourName, tourDto.TourLocation, tourDto.Description, tourDto.Languages, tourDto.MaxGuests, tourDto.Duration, tourDto.Images);
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


    }
}
