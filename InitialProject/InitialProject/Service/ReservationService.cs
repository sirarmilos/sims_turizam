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

        public List<Reservation> AllReservations
        {
            get;
            set;
        }

        public List<Reservation> OwnerReservations
        {
            get;
            set;
        }

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        public ReservationService(string owner)
        {
            Owner = owner;
            reservationRepository = new ReservationRepository();

            ListInitialization();
        }

        private void ListInitialization()
        {
            AllReservations = new List<Reservation>();
            OwnerReservations = new List<Reservation>();
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
    }
}
