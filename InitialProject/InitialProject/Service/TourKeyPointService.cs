using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourKeyPointService
    {
        private readonly ITourKeyPointRepository tourKeyPointRepository;
        public TourKeyPointService()
        {
            tourKeyPointRepository = Injector.Injector.CreateInstance<ITourKeyPointRepository>();
            //tourKeyPointRepository = new TourKeyPointRepository();
        }

        public List<TourKeyPoint> FindAll()
        {
            return tourKeyPointRepository.FindAll();
        }

        public List<TourKeyPoint> FindByTourGuidance(int id)
        {
            List<TourKeyPoint> keyPoints = new List<TourKeyPoint>();
            foreach (TourKeyPoint tkp in tourKeyPointRepository.FindAll())
            {
                if (id == tkp.TourGuidence.Id)
                    keyPoints.Add(tkp);
            }

            return keyPoints;
        }

        
    }
}
