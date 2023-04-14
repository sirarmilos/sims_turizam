using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class ShowReservationDTO 
    {
        public int ReservationId { get; set; }
        public string AccommodationName { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int GuestsNumber { get; set; }

        public ShowReservationDTO()
        {

        }

        public ShowReservationDTO(int reservationId, string accommodationName, DateTime startDate, DateTime endDate, int guestsNumber)
        {
            ReservationId = reservationId;
            AccommodationName = accommodationName;
            StartDate = startDate;
            EndDate = endDate;
            GuestsNumber = guestsNumber;
        }

        public ShowReservationDTO(Reservation reservation)
        {
            ReservationId = reservation.ReservationId; // todo: razmisli da li ti treba zaista id
            AccommodationName = reservation.Accommodation.AccommodationName;
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;
            GuestsNumber = reservation.GuestsNumber;
        }

    }
}
