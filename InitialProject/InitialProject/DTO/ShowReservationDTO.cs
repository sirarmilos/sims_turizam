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

        //public string Guest1 { get; set; }

        public Accommodation Accommodation { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int GuestsNumber { get; set; }

        public ShowReservationDTO()
        {

        }

        public ShowReservationDTO(Accommodation accommodation, DateTime startDate, DateTime endDate, int guestsNumber)
        {
            Accommodation = accommodation;
            StartDate = startDate;
            EndDate = endDate;
            GuestsNumber = guestsNumber;
        }

        public ShowReservationDTO(int reservationId, Accommodation accommodation, DateTime startDate, DateTime endDate, int guestsNumber)
        {
            ReservationId = reservationId;
            Accommodation = accommodation;
            StartDate = startDate;
            EndDate = endDate;
            GuestsNumber = guestsNumber;
        }

        public ShowReservationDTO(Reservation reservation)
        {
            ReservationId = reservation.ReservationId; 
            Accommodation = reservation.Accommodation;
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;
            GuestsNumber = reservation.GuestsNumber;
        }

        public Reservation TransformToOrigin() // todo: ne znam da l treba ovde
        {
            Reservation reservation = new Reservation();
            reservation.ReservationId = ReservationId;
            reservation.Accommodation = Accommodation;
            reservation.StartDate = StartDate;
            reservation.EndDate = EndDate;
            reservation.GuestsNumber = GuestsNumber;

            return reservation;
        }

    }
}
