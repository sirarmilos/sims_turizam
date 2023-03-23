using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Repository
{
    internal class TourRepository
    {
        private const string FilePathTour = "../../../Resources/Data/tours.csv";

        private const string FilePathTourKeyPoints = "../../../Resources/Data/tourkeypoints.csv";

        private const string FilePathLocation = "../../../Resources/Data/location.csv";

        private readonly Serializer<Tour> tourSerializer;
       
        private readonly Serializer<TourKeyPoint> tourKeyPointsSerializer;

        private readonly Serializer<Location> locationSerializer;

        private List<Tour> tours;

        private List<TourKeyPoint> tourKeyPoints;

        private List<Location> locations;

        public TourRepository()
        {
            tourSerializer = new Serializer<Tour>();
            tours = tourSerializer.FromCSV(FilePathTour);

            tourKeyPointsSerializer = new Serializer<TourKeyPoint>();
            tourKeyPoints = tourKeyPointsSerializer.FromCSV(FilePathTourKeyPoints);

            locationSerializer = new Serializer<Location>();
            locations = locationSerializer.FromCSV(FilePathLocation);
        }

        public List<Tour> Load()
        {
            List<Tour> result = new List<Tour>();

            foreach (Tour tour in tours)
            {
                foreach (Location location in locations)
                {
                    if (tour.Location.Id == location.Id)
                    {
                        tour.Location = location;
                    }
                }
                result.Add(tour);
            }

            return result;
        }

        public List<Tour> SearchAndShow(string city=null,string country=null,int duration=0,Language language = 0,int numberOfPeople=0)
        {
 
            List<Tour> sameCity = new List<Tour>();
            List<Tour> sameCountry = new List<Tour>();
            List<Tour> longerDuration = new List<Tour>();
            List<Tour> sameLanguage = new List<Tour>();
            List<Tour> morePeople = new List<Tour>();

            #region Check city
            if (city != "")
            {
                foreach (Tour tour in tours)
                {
                    if(tour.Location.City.Equals(city))
                    {
                        sameCity.Add(tour);
                    }
                }
            }
            else
            {
                sameCity = tourSerializer.FromCSV(FilePathTour);
            }
            #endregion

            #region check country
            if (country != "")
            {
                foreach(Tour tour in tours)
                {
                    if(tour.Location.Country.Equals(country))
                    {
                        sameCountry.Add(tour); 
                    }
                }
            }
            else
            {
                sameCountry = tourSerializer.FromCSV(FilePathTour);
            }
            #endregion

            #region dutration
            if (duration > 0)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Duration > duration)
                    {
                        longerDuration.Add(tour);
                    }
                }
            }
            else
            {
                longerDuration = tourSerializer.FromCSV(FilePathTour);
            }
            #endregion

            #region languages
            if (language != 0)
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
                sameLanguage = tourSerializer.FromCSV(FilePathTour);
            }
            #endregion

            #region number of people
            if (numberOfPeople>0)
            {
                foreach(Tour tour in tours)
                {
                    if(tour.MaxGuests>numberOfPeople)
                    {
                        morePeople.Add(tour);
                    }
                }
            }
            else
            {
                morePeople = tourSerializer.FromCSV(FilePathTour);
            }
            #endregion

            List<Tour> asd = new List<Tour>();
            asd = sameCity.Intersect(longerDuration).ToList();

            return asd;
        }

        public void Save(TourDto tourDto)
        {
            Tour tour = new Tour(NextIdTour(), tourDto.TourName, tourDto.TourLocation, tourDto.Description, tourDto.Languages, tourDto.MaxGuests, tourDto.TourKeyPoints, tourDto.TourDate, tourDto.Duration, tourDto.Images);
            tours.Add(tour);
            tourSerializer.ToCSV(FilePathTour, tours);
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


    }
}
