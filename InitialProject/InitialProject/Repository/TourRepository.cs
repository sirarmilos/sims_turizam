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

        public List<Tour> SearchAndShow(string city=null,string state=null,int duration=0,string language=null,int numberOfPeople=0)
        {
            return tourSerializer.FromCSV(FilePathTour);
        }

        public void Save(string tourName, string tourCountry, string tourCity, string tourAddress, decimal tourLatitude, decimal tourLongitude, string description, Languages languages, int maxGuests, List<TourKeyPoints> tourKeyPointss, string keyPointName, string keyPointCountry, 
            string keyPointCity, string keyPointAddress, decimal keyPointLatitude, decimal keyPointLongitude, List<DateTime> tourDates, int duration, List<string> images)
        {
            int indicator = 0;
            locations = locationSerializer.FromCSV(FilePathLocation);
            tourKeyPoints = tourKeyPointsSerializer.FromCSV(FilePathTourKeyPoints);
            
            Location tourLocation; 

            foreach (TourKeyPoints tourKeyPoint in tourKeyPointss)
            {
                tourLocation = new Location(tourKeyPoint.Location);
                locations.Add(tourLocation);
                tourKeyPoints.Add(tourKeyPoint);
                Debug.WriteLine(tourLocation.Country);
            }

            locationSerializer.ToCSV(FilePathLocation, locations);

            Location location = new Location(NextIdLocation(), tourCountry, tourCity, tourAddress, tourLatitude, tourLongitude);
            Tour tour = new Tour(NextIdTour(), tourName, location, description, languages, maxGuests, tourKeyPointss, tourDates, duration, images);
            Debug.WriteLine("a");

            tours = tourSerializer.FromCSV(FilePathTour);

            foreach (Tour temporaryTour in tours)
            {
                if (temporaryTour.TourName.Equals(tourName) == true)
                {
                    indicator = 1;
                    MessageBox.Show("Tour with this name already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }

            if (indicator == 0)
            {
                locations = locationSerializer.FromCSV(FilePathLocation);
                locations.Add(location);
                locationSerializer.ToCSV(FilePathLocation, locations);


                tourKeyPointsSerializer.ToCSV(FilePathTourKeyPoints, tourKeyPoints);
                Debug.WriteLine("a");
                Debug.WriteLine(tour.Id.ToString(), tour.TourName, tour.Location, tour.Description, tour.Images, tour.MaxGuests, tour.TourKeyPoints);
                tours.Add(tour);
                Debug.WriteLine("a");
                tourSerializer.ToCSV(FilePathTour, tours);

            }


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
