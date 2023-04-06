using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class BusyReservation
    {
        public int ReservationId { get; set; }

        public List<Reservation> ReservationsToDelete { get; set; }

        public BusyReservation()
        {

        }

        public BusyReservation(int reservationId, List<Reservation> reservationsToDelete)
        {
            ReservationId = reservationId;
            ReservationsToDelete = reservationsToDelete;
        }
    }
}
