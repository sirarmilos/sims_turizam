using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.View;
using System.Diagnostics;
using System.Windows.Interactivity;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Service
{
    public class ReservationService
    {

        private ReservationRepository reservationRepository;
        private CanceledReservationRepository canceledReservationRepository;
        private string owner;
        private string guest1;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public ReservationService()
        {
            reservationRepository = new ReservationRepository();
            canceledReservationRepository = new CanceledReservationRepository();
        }

        public ReservationService(string username)
        {
            Owner = username;
            Guest1 = username;
            reservationRepository = new ReservationRepository();
            canceledReservationRepository = new CanceledReservationRepository();
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





        public List<ShowReservationDTO> FindAll(string username)
        {
            Guest1 = username;

            List<Reservation> allReservations = reservationRepository.FindAllReservations();

            List<Reservation> guest1Reservations = FindGuest1Reservations(allReservations);

            return GetShowReservationsDTO(guest1Reservations);
        }

        public List<Reservation> FindGuest1Reservations(List<Reservation> allReservations)
        {
            return allReservations.ToList().FindAll(x => x.GuestUsername.Equals(Guest1) == true);
        }

        public List<ShowReservationDTO> GetShowReservationsDTO(List<Reservation> guest1Reservations)
        {
            List<ShowReservationDTO> showReservationDTOs = new List<ShowReservationDTO>();

            foreach (Reservation temporaryReservation in guest1Reservations.ToList())
            {
                if (temporaryReservation.GuestUsername.Equals(Guest1))
                { 
                    showReservationDTOs.Add(new ShowReservationDTO(temporaryReservation));    
                }
            }

            return showReservationDTOs;
        }

        public bool Remove(ShowReservationDTO showReservationDTO)
        {
            Reservation reservation = FindReservationByReservationId(showReservationDTO.ReservationId);

            int days = reservation.StartDate.Subtract(DateTime.Now).Days;


            if (days >= reservation.Accommodation.LeftCancelationDays)
            {
                CancelReservation(reservation);

                return true;
            }

            return false;
        }

        private void CancelReservation(Reservation reservation)
        {
            List<Reservation> allReservations = reservationRepository.FindAllReservations();
            reservationRepository = new ReservationRepository();
            canceledReservationRepository = new CanceledReservationRepository();

            allReservations.Remove(reservation);

            reservationRepository.SaveReservations(allReservations);

            canceledReservationRepository.Save(reservation);
        }
    }
}
