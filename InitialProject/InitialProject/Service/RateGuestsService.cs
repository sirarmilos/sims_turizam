using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Service
{
    public class RateGuestsService
    {
        private readonly RateGuestRepository rateGuestRepository;

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

        public List<RateGuest> AllRateGuests
        {
            get;
            set;
        }

        public List<RateGuest> OwnerRateGuests
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

        public RateGuestsService(string owner)
        {
            Owner = owner;
            rateGuestRepository = new RateGuestRepository();
            reservationRepository = new ReservationRepository();

            ListInitialization();
        }

        private void ListInitialization()
        {
            AllReservations = new List<Reservation>();
            OwnerReservations = new List<Reservation>();
            AllRateGuests = new List<RateGuest>();
            OwnerRateGuests = new List<RateGuest>();
        }

        public List<RateGuestsDTO> FindAllGuestsForRate()
        {
            AllReservations = reservationRepository.FindAllReservations();

            FindOwnerReservations();

            AllRateGuests = rateGuestRepository.FindAllRateGuests();

            FindReservationsForRateGuests();

            FindOwnerRateGuests();

            return FindRateGuestsDTOs();
        }

        public void FindOwnerReservations()
        {
            OwnerReservations = AllReservations.ToList().FindAll(x => x.Accommodation.OwnerUsername.Equals(Owner) == true);
        }

        public void FindReservationsForRateGuests()
        {
            foreach (RateGuest temporaryRateGuest in AllRateGuests.ToList())
            {
                temporaryRateGuest.Reservation = AllReservations.ToList().Find(x => x.ReservationId == temporaryRateGuest.Reservation.ReservationId);
            }
        }

        public void FindOwnerRateGuests()
        {
            OwnerRateGuests = AllRateGuests.ToList().FindAll(x => x.Reservation.Accommodation.OwnerUsername.Equals(Owner) == true);
        }

        public List<RateGuestsDTO> FindRateGuestsDTOs()
        {
            List<RateGuestsDTO> rateGuestsDTOs = new List<RateGuestsDTO>();

            foreach (Reservation temporaryReservation in OwnerReservations.ToList())
            {
                RateGuest temporaryRateGuest = OwnerRateGuests.Find(x => x.Reservation.ReservationId == temporaryReservation.ReservationId);

                if(temporaryRateGuest == null)
                {
                    RateGuestsDTO rateGuestDTO = IsValidToAdd(temporaryReservation, temporaryRateGuest);
                    if (rateGuestDTO != null)
                    {
                        rateGuestsDTOs.Add(rateGuestDTO);
                    }
                }
            }

            return rateGuestsDTOs;
        }

        public RateGuestsDTO IsValidToAdd(Reservation temporaryReservation, RateGuest temporaryRateGuest)
        {
            int days = DateTime.Now.Subtract(temporaryReservation.EndDate).Days;

            if (days <= 5 && days >= 0)
            {
                string deadline = "This is the last day to rate a guest.";

                if (5 - days > 0)
                {
                    deadline = "You have " + (5 - days) + " more days to rate the guest.";
                }

                return new RateGuestsDTO(temporaryReservation.ReservationId, temporaryReservation.GuestUsername, deadline);
            }

            return null;
        }

        public void SaveNewRateGuest(SaveNewRateGuestDTO saveNewRateGuestDTO)
        {
            RateGuest rateGuest = new RateGuest(FindReservationByReservationId(saveNewRateGuestDTO.ReservationId), saveNewRateGuestDTO);

            AllRateGuests.Add(rateGuest);

            rateGuestRepository.SaveRateGuests(AllRateGuests);
        }

        private Reservation FindReservationByReservationId(int reservationId)
        {
            return AllReservations.Find(x => x.ReservationId == reservationId);
        }
    }
}
