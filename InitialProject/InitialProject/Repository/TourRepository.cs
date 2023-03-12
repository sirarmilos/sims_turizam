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

        public List<Tour> SearchAndShow(string city=null,string state=null,int duration=0,string language=null,int numberOfPeople=0)
        {
            return tourSerializer.FromCSV(FilePathTour);
        }


    }
}
