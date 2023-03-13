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

        public List<Tour> SearchAndShow(string city=null,string country=null,int duration=0,Languages language = 0,int numberOfPeople=0)
        {
            List<Tour> sameCity = new List<Tour>();
            List<Tour> sameCountry = new List<Tour>();
            List<Tour> longerDuration = new List<Tour>();
            List<Tour> sameLanguage = new List<Tour>();
            List<Tour> morePeople = new List<Tour>();

            List<Tour> searchResult = new List<Tour>();

            #region Check city
            if (city != null)
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
            if (country != null)
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

            foreach ( Tour tour in tours ) 
            {
                foreach( Location location in locations)
                {
                    if(tour.Location.Id == location.Id)
                    {
                        tour.Location = location;
                    }
                }
                searchResult.Add(tour);
            }
            return searchResult;
        }


    }
}
