using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class TourRepository
    {
        private const string FilePathTour = "../../../Resources/Data/tours.csv";

        private const string FilePathTourKeyPoints = "../../../Resources/Data/tourkeypoints.csv";

        private const string FilePathLocation = "../../../Resources/Data/location.csv";

        private readonly Serializer<Tour> tourSerializer;
       
        private readonly Serializer<TourKeyPoints> tourKeyPointsSerializer;

        private readonly Serializer<Location> locationSerializer;

        private List<Tour> tours;

        private List<TourKeyPoints> tourKeyPoints;

        private List<Location> locations;

        public TourRepository()
        {
            tourSerializer = new Serializer<Tour>();
            tours = tourSerializer.FromCSV(FilePathTour);

            tourKeyPointsSerializer = new Serializer<TourKeyPoints>();
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

        public List<Tour> SearchAndShow(string city=null,string country=null,int duration=0,Languages language = 0,int numberOfPeople=0)
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


            if (language != 0)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Languages == language)
                    {
                        sameLanguage.Add(tour);
                    }
                }
            }
            else
            {
                sameLanguage = tourSerializer.FromCSV(FilePathTour);
            }

            if(numberOfPeople>0)
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


            List<Tour> asd = sameCity.Intersect(sameCountry).Intersect(morePeople).Intersect(sameLanguage).Intersect(longerDuration).ToList();
            return asd;
        }


    }
}
