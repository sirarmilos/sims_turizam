using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class ReservationService
    {
        private readonly ReservationRepository reservationRepository;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        public ReservationService()
        {
            reservationRepository = new ReservationRepository();
        }

        public ReservationService(string owner)
        {
            Owner = owner;
            reservationRepository = new ReservationRepository();
        }

        public List<Reservation> FindAllReservations()
        {
            return reservationRepository.FindAllReservations();
        }

        public List<Reservation> FindOwnerReservations()
        {
            return reservationRepository.FindReservationsByOwnerUsername(Owner);
        }

        public Reservation FindReservationsForOwnerRateGuests(RateGuest rateGuest)
        {
            return reservationRepository.FindReservationByReservationId(rateGuest.Reservation.ReservationId);
        }

        public Reservation FindReservationByReservationId(int reservationId)
        {
            return reservationRepository.FindReservationByReservationId(reservationId);
        }

        public string FindOwnerByReservationId(int reservationId)
        {
            return reservationRepository.FindOwnerByReservationId(reservationId);
        }

        public List<Reservation> FindAllReservationsByAccommodationId(int accommodationId)
        {
            return FindOwnerReservations().ToList().FindAll(x => x.Accommodation.Id == accommodationId);
        }

        public void UpdateDatesForSelectedBookingMoveRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest)
        {
            reservationRepository.UpdateDatesForSelectedBookingMoveRequest(selectedBookingMoveRequest);
        }
    }
}
