using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ITourReservationRepository
    {
        List<TourReservation> FindAll();

        TourReservation FindById(int id);

        void Save(List<TourReservation> tourReservations);

        int NextId();
    }
}
