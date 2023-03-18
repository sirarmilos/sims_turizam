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
            TourKeyPoint tourKeyPoint = new(NextId(), tourKeyPointDto.TourKeyPointName, tourKeyPointDto.Location);
            tourKeyPoints.Add(tourKeyPoint);
            tourKeyPointSerializer.ToCSV(FilePathTourKeyPoint, tourKeyPoints);
            return tourKeyPoint;
        }

        public int NextId()
        {
            if (tourKeyPoints.Count < 1)
            {
                return 1;
            }
            return tourKeyPoints.Max(c => c.Id) + 1;
        }

        public TourKeyPoint GetById(int id)
        {
            foreach(TourKeyPoint keyPoint in tourKeyPoints)
            {
                if(keyPoint.Id == id)
                    return keyPoint;
            }
            return null;


        }


    }
}
