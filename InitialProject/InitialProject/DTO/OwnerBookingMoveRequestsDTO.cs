using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class OwnerBookingMoveRequestsDTO
    {
        public int ReservationId { get; set; }

        public string GuestUsername { get; set; }

        public string AccommodationName { get; set; }

        public DateTime OldStartDate { get; set; }

        public DateTime OldEndDate { get; set; }

        public DateTime NewStartDate { get; set; }

        public DateTime NewEndDate { get; set; }

        public string NewDateAvailable { get; set; }

        public OwnerBookingMoveRequestsDTO()
        {

        }

        public OwnerBookingMoveRequestsDTO(Reservation reservation, ReservationReschedulingRequest reservationReschedulingRequest, string newDateAvailable)
        {
            ReservationId = reservation.ReservationId;
            GuestUsername = reservation.GuestUsername;
            AccommodationName = reservation.Accommodation.AccommodationName;
            OldStartDate = reservation.StartDate;
            OldEndDate = reservation.EndDate;
            NewStartDate = reservationReschedulingRequest.NewStartDate;
            NewEndDate = reservationReschedulingRequest.NewEndDate;
            NewDateAvailable = newDateAvailable;
        }
    }
}
