using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ITourKeyPointRepository
    {
        List<TourKeyPoint> FindAll();

        void Save(List<TourKeyPoint> tourKeyPoints);

        TourKeyPoint FindById(int id);

        int NextId();

        void SaveToFile(TourKeyPoint tkp);

        void UpdateCheckedKeyPoints(List<TourKeyPoint> keyPoints);

    }
}