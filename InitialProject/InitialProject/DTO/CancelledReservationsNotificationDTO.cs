using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class CancelledReservationsNotificationDTO
    {
        public int ReservationId { get; set; }

        public string AccommodationName { get; set; }

        public DateTime ReservationStartDate { get; set; }

        public DateTime ReservationEndDate { get; set; }

        public CancelledReservationsNotificationDTO()
        {

        }

        public CancelledReservationsNotificationDTO(CanceledReservation canceledReservation)
        {
            ReservationId = canceledReservation.ReservationId;
            AccommodationName = canceledReservation.Accommodation.AccommodationName;
            ReservationStartDate = canceledReservation.StartDate;
            ReservationEndDate = canceledReservation.EndDate;
        }
    }
}
