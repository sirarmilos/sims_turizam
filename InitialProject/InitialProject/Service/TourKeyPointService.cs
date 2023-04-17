using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    internal class TourKeyPointService
    {
        private TourKeyPointRepository tourKeyPointRepository;
        public TourKeyPointService()
        {
            tourKeyPointRepository = new TourKeyPointRepository();
        }

        public List<TourKeyPoint> GetByTourGuidance(int id)
        {
            List<TourKeyPoint> keyPoints = new List<TourKeyPoint>();
            foreach (TourKeyPoint tkp in tourKeyPointRepository.GetAll())
            {
                if (id == tkp.TourGuidence.Id)
                    keyPoints.Add(tkp);
            }

            return keyPoints;
        }

        public List<TourKeyPoint> GetAll()
        {
            return tourKeyPointRepository.GetAll();
        }
    }
}
