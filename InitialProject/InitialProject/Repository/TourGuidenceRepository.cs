 using InitialProject.Dto;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.Repository
{
    public class TourGuidenceRepository : ITourGuidenceRepository
    {
        private TourRepository tourRepository;

        private TourReservationRepository tourReservationRepository;
        
        private TourKeyPointRepository tourKeyPointRepository;

        private const string FilePathTourGuidence = "../../../Resources/Data/tourguidences.csv";

        private readonly Serializer<TourGuidence> tourGuidenceSerializer;

        private List<TourGuidence> tourGuidences;

        public TourGuidenceRepository()
        {
            tourGuidenceSerializer = new Serializer<TourGuidence>();
           // tourGuidences = tourGuidenceSerializer.FromCSV(FilePathTourGuidence);
        }

        public List<TourGuidence> FindAll()
        {
            tourRepository = new TourRepository();

            tourGuidences = tourGuidenceSerializer.FromCSV(FilePathTourGuidence);

            foreach (TourGuidence temporaryTourGuidence in tourGuidences.ToList())
            {
                temporaryTourGuidence.Tour = tourRepository.FindById(temporaryTourGuidence.Tour.Id);
            }

            return tourGuidences;
        }

        public TourGuidence FindById(int id)
        {
            return FindAll().ToList().Find(x => x.Id == id);
        }

        public void Save(List<TourGuidence> tourGuidences)
        {
            tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
        }

       /* public void Update(TourGuidence tourGuidence)
        {
            TourGuidence tG = tourGuidences.FirstOrDefault(x => x.Id == tourGuidence.Id);
            tG = tourGuidence;
            tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
        }*/
 
        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }

            return FindAll().Max(x => x.Id) + 1;
        }

        // public TourGuidence FindById(int id) => tourGuidences.FirstOrDefault(x => x.Id == id);

        

        /* public List<TourGuidence> FindAll()
        {
            return tourGuidences;
        }*/

        public void SaveToFile(TourGuidence t)
        {
            tourGuidences = FindAll();
            t.Id = NextId();
            tourGuidences.Add(t);
            tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
        }

        public TourGuidence FindByTourAndDate(Tour tour, DateTime date)
        {
            tourGuidences = FindAll();
            TourGuidence tourGuidence = new TourGuidence();

            foreach(TourGuidence tourG in tourGuidences)
            {
                if(tour.Id == tourG.Tour.Id)
                {
                    if (DateTime.Compare(date, tourG.StartTime) == 0)
                    {
                        tourGuidence = tourG;
                        break;
                    }
                }
            }

            return tourGuidence;
        }


        public string FindGuide(int tourGuidenceId)
        {
            tourGuidences = FindAll();
            string guideUsername= "";

            foreach(TourGuidence tourGuidence in tourGuidences)
            {
                if(tourGuidence.Id==tourGuidenceId)
                {
                    guideUsername += tourGuidence.Tour.GuideUsername;
                    break;
                }

            }

            return guideUsername;

        }
        
        public TourAttendanceDTO FindTourAttendanceDTO(int tourReservationId)
        {
            TourAttendanceDTO dto = new TourAttendanceDTO();

            tourReservationRepository = new TourReservationRepository();
            tourKeyPointRepository = new TourKeyPointRepository();   

            TourReservation tourReservation = tourReservationRepository.FindById(tourReservationId);
            TourGuidence tourGuidence = FindById(tourReservation.tourGuidenceId);


            foreach(TourKeyPoint tourKeyPoint in tourKeyPointRepository.FindAll())
            {
                if(tourKeyPoint.TourGuidence.Id==tourGuidence.Id)
                {
                    dto.TourKeyPoints.Add(tourKeyPoint);
                }
            }

            dto.Date = tourGuidence.StartTime;
            dto.GuideUsername = tourGuidence.Tour.GuideUsername;

            return dto;
        }

        public List<TourGuidence> FindGuideAll(string username)
        {
            return FindAll().ToList().FindAll(x => x.Tour.GuideUsername.Equals(username) == true);
        }

        public List<TourGuidence> FindFinishedByGuideUsername(int tourId, string username)
        {
            return FindGuideAll(username).ToList().FindAll(x => x.Finished == true && x.Tour.Id == tourId);
        }

        public List<TourGuidence> FindGuideTodayUpcomming(string guideUsername)
        {
            return FindGuideAll(guideUsername).ToList().FindAll(x => x.StartTime.Date == DateTime.Today && x.StartTime.TimeOfDay >= DateTime.Now.TimeOfDay && x.Finished == false);
        }


    }
}
