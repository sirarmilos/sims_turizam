using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ICanceledReservationRepository
    {
        void Save(Reservation reservation);
    }
}
