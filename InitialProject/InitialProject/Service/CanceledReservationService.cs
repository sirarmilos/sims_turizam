using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class CanceledReservationService
    {
        private CanceledReservationRepository canceledReservationRepository;

        public CanceledReservationService()
        {
            canceledReservationRepository = new CanceledReservationRepository();
        }

        public void Save(Reservation reservation)
        {
            canceledReservationRepository.Save(reservation);
        }
    }
}
