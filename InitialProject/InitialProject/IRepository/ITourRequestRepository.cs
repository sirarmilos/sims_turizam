using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ITourRequestRepository
    {
        List<TourRequest> FindAll();

        TourRequest FindById(int id);

        void Save(List<TourRequest> requests);
    }
}
