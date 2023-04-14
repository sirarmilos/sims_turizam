using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class TourKeyPointRepository
    {
        private const string FilePathTourKeyPoint = "../../../Resources/Data/tourkeypoints.csv";

        private readonly Serializer<TourKeyPoint> tourKeyPointSerializer;

        private List<TourKeyPoint> tourKeyPoints;  
        
        public TourKeyPointRepository()
        {
            tourKeyPointSerializer = new Serializer<TourKeyPoint>();
            tourKeyPoints = tourKeyPointSerializer.FromCSV(FilePathTourKeyPoint);
        }

        public TourKeyPoint Save(TourKeyPointDto tourKeyPointDto)
        {
            TourKeyPoint tourKeyPoint = new(NextId(), tourKeyPointDto.TourKeyPointName, tourKeyPointDto.Location, null, false);
            tourKeyPoints.Add(tourKeyPoint);
            //tourKeyPointSerializer.ToCSV(FilePathTourKeyPoint, tourKeyPoints);
            return tourKeyPoint;
        }

        public void Update(TourKeyPoint tourKeyPoint)
        {
            TourKeyPoint tKP = tourKeyPoints.FirstOrDefault(x => x.Id == tourKeyPoint.Id);
            tKP = tourKeyPoint;
            tourKeyPointSerializer.ToCSV(FilePathTourKeyPoint, tourKeyPoints);
        }

        public int NextId()
        {
            if (tourKeyPoints.Count < 1)
            {
                return 1;
            }
            return tourKeyPoints.Max(c => c.Id) + 1;
        }

        public TourKeyPoint GetById(int id) => tourKeyPoints.FirstOrDefault(x => x.Id == id);

        public List<TourKeyPoint> GetAll()
        {
            return tourKeyPoints;
        }

        public void SaveKeyPoint(TourKeyPoint t)
        {
            tourKeyPoints.Add(t);
            tourKeyPointSerializer.ToCSV(FilePathTourKeyPoint, tourKeyPoints);
        }

        internal void SaveToFile(TourKeyPoint tkp)
        {
            //tkp.Id = NextId();
            tourKeyPoints.Add(tkp);
            tourKeyPointSerializer.ToCSV(FilePathTourKeyPoint, tourKeyPoints);
        }

        public List<TourKeyPoint> Load(int id)
        {
            List<TourKeyPoint> keyPoints = new List<TourKeyPoint>();
            foreach (TourKeyPoint tkp in tourKeyPoints)
            {
                if (id == tkp.TourGuidence.Id)
                    keyPoints.Add(tkp);
            }
            return keyPoints;
        }

        public void UpdateCheckedKeyPoints()
        {
            tourKeyPointSerializer.ToCSV(FilePathTourKeyPoint, tourKeyPoints);
        }
    }
}
