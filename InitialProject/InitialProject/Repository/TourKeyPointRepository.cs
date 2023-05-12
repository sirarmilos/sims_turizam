using InitialProject.Dto;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class TourKeyPointRepository : ITourKeyPointRepository
    {
        //private LocationRepository locationRepository; 

       // private TourGuidenceRepository tourGuidenceRepository;

        private const string FilePathTourKeyPoint = "../../../Resources/Data/tourkeypoints.csv";

        private readonly Serializer<TourKeyPoint> tourKeyPointSerializer;

        private List<TourKeyPoint> tourKeyPoints;  
        

        public TourKeyPointRepository()
        {
            tourKeyPointSerializer = new Serializer<TourKeyPoint>();
            //tourKeyPoints = tourKeyPointSerializer.FromCSV(FilePathTourKeyPoint);
        }

        public List<TourKeyPoint> FindAll()
        {
            LocationRepository locationRepository = new LocationRepository();

            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();

            tourKeyPoints = tourKeyPointSerializer.FromCSV(FilePathTourKeyPoint);

            foreach (TourKeyPoint temporaryTourKeyPoint in tourKeyPoints.ToList())
            {
                temporaryTourKeyPoint.Location = locationRepository.FindById(temporaryTourKeyPoint.Location.Id);
                temporaryTourKeyPoint.TourGuidence = tourGuidenceRepository.FindById(temporaryTourKeyPoint.TourGuidence.Id);
            }

            return tourKeyPoints;
        }

        public TourKeyPoint FindById(int tourKeyPointId)
        {
            return FindAll().ToList().Find(x => x.Id == tourKeyPointId);
        }

        public void Save(List<TourKeyPoint>  tourKeyPoints)
        {
            tourKeyPointSerializer.ToCSV(FilePathTourKeyPoint,tourKeyPoints);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }

            return FindAll().Max(x => x.Id) + 1;
        }

        //public TourKeyPoint FindById(int id) => tourKeyPoints.FirstOrDefault(x => x.Id == id);

        public void SaveToFile(TourKeyPoint tkp)
        {
            tourKeyPoints = FindAll();
            tourKeyPoints.Add(tkp);
            Save(tourKeyPoints);
        }

        public void UpdateCheckedKeyPoints(List<TourKeyPoint> keyPoints)
        {
            tourKeyPoints = FindAll();
            foreach(TourKeyPoint tkp in tourKeyPoints)
            {
                foreach(TourKeyPoint keyPoint in keyPoints)
                {
                    if(keyPoint.Id == tkp.Id)
                    {
                        tkp.Passed = keyPoint.Passed;
                        break;
                    }
                }
            }
            Save(tourKeyPoints);
            /*foreach(TourKeyPoint tourKeyPoint in keyPoints)
            {
                TourKeyPoint kp = FindById(tourKeyPoint.Id);
                kp.Passed = tourKeyPoint.Passed;
            }
            Save(tourKeyPoints); // proveriti*/


        }
    }
}
