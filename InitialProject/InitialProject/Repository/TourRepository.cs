using InitialProject.Dto;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.Service;
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
    internal class TourRepository : ITourRepository
    {
        private const string FilePathTour = "../../../Resources/Data/tours.csv";

        private const string FilePathTourKeyPoints = "../../../Resources/Data/tourkeypoints.csv";

        private const string FilePathLocation = "../../../Resources/Data/locations.csv";

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

        public List<Tour> FindAll()
        { 
            return tours;
        }

        public Tour FindByName(string id)
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


        public Tour Save(TourDto tourDto)
        {
            Tour tour = new Tour(NextId(), tourDto.TourName, tourDto.TourLocation, tourDto.Description, tourDto.Languages, tourDto.MaxGuests, tourDto.Duration, tourDto.Images, tourDto.Username);
            tours.Add(tour);
            tourSerializer.ToCSV(FilePathTour, tours);
            return tour;
        }

        public int NextId()
        {
            tours = tourSerializer.FromCSV(FilePathTour);
            if (tours.Count < 1)
            {
                return 1;
            }
            return tours.Max(c => c.Id) + 1;
        }

        /*public int NextIdLocation()
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
        }*/

        public Tour FindById(int id) => tours.FirstOrDefault(x => x.Id == id);

        public List<int> GetGuestNumber(int tourId)
        {
            // type = [(1, <18), (2, 18-50), (3, >50)]
            List<int> count = new List<int>(new int[3]);
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();
            Guest2Repository guest2Repository = new Guest2Repository();
            List<TourGuidence> tourGuidences = tourGuidenceRepository.FindAll();
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




    }
}
