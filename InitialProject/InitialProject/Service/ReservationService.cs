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
        private readonly CanceledReservationService canceledReservationService;

        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;

        private ReservationRepository reservationRepository;

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
            canceledReservationService = new CanceledReservationService();
            reservationReschedulingRequestService = new ReservationReschedulingRequestService();
        }

        public ReservationService(string username)
        {
            Owner = username;
            Guest1 = username;
            reservationRepository = new ReservationRepository();
            canceledReservationService = new CanceledReservationService();
            reservationReschedulingRequestService = new ReservationReschedulingRequestService();
        }

        public List<Reservation> FindOwnerReservations()
        {
            return reservationRepository.FindByOwnerUsername(Owner);
        }

        public Reservation FindById(int reservationId)
        {
            return reservationRepository.FindById(reservationId);
        }

        public string FindOwnerByReservationId(int reservationId)
        {
            return reservationRepository.FindOwnerByReservationId(reservationId);
        }

        public List<Reservation> FindReservationsByAccommodationId(int accommodationId)
        {
            return FindOwnerReservations().ToList().FindAll(x => x.Accommodation.Id == accommodationId);
        }

        public void UpdateDatesToSelectedBookingMoveRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest)
        {
            reservationRepository.UpdateDatesToSelectedBookingMoveRequest(selectedBookingMoveRequest);
        }

        public void RemoveCancelledReservations(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, List<Reservation> cancelledReservations)
        {
            foreach (Reservation temporaryReservation in cancelledReservations.ToList())
            {
                 reservationRepository.RemoveById(selectedBookingMoveRequest.ReservationId, temporaryReservation.ReservationId);
            }
        }




        public List<ShowReservationDTO> FindAll(string username)
        {
            Guest1 = username;

            List<Reservation> guest1Reservations = FindGuest1Reservations(Guest1);

            return GetShowReservationsDTO(guest1Reservations);
        }

        public List<Reservation> FindGuest1Reservations(string username)
        {
            return reservationRepository.FindGuest1Reservations(username);
        }

        public List<ShowReservationDTO> GetShowReservationsDTO(List<Reservation> guest1Reservations)
        {
            List<ShowReservationDTO> showReservationDTOs = new List<ShowReservationDTO>();

            foreach (Reservation temporaryReservation in guest1Reservations.ToList())
            {
                showReservationDTOs.Add(new ShowReservationDTO(temporaryReservation));    
            }

            return showReservationDTOs;
        }

        public bool IsRemoved(ShowReservationDTO showReservationDTO)
        {
            Reservation reservation = FindById(showReservationDTO.ReservationId);

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
            reservationReschedulingRequestService.RemoveRequestsToCancelledReservation(reservation.ReservationId);

            reservationRepository.RemoveById(reservation.ReservationId);

            canceledReservationService.Save(reservation);
        }

        public bool Guest1HasNotification()
        {
            return reservationReschedulingRequestService.Guest1HasNotification(Guest1);
        }

        public void Save(ShowReservationDTO showReservationDTO)
        {
            reservationRepository.Save(
                Guest1, 
                showReservationDTO.Accommodation, 
                showReservationDTO.StartDate, 
                showReservationDTO.EndDate, 
                showReservationDTO.GuestsNumber
            );
        }









        public List<DateSlot> FindAvailableDateSlots(
            List<DateSlot> freeDateSlots, Accommodation accommodation, DateTime StartDate, DateTime EndDate, int calendarReservationDays)
        {

            List<Reservation> accommodationReservations = reservationRepository.FindAllByAccommodation(accommodation.Id);

            if (accommodationReservations.Count > 0)
            {
                accommodationReservations.Sort((r1, r2) => r1.StartDate.CompareTo(r2.StartDate));
            }
            else
            {
                if (StartDate.AddDays(calendarReservationDays) <= EndDate)
                {
                    DateSlot temporary = new DateSlot(StartDate, EndDate);
                    freeDateSlots.Add(temporary);
                    return freeDateSlots;
                }

                return null;
            }

            if (IsDateSlotAfterReservations(accommodationReservations, freeDateSlots, StartDate, EndDate, calendarReservationDays)) return freeDateSlots;

            if (IsDateSlotBeforeReservations(accommodationReservations, freeDateSlots, StartDate, EndDate, calendarReservationDays)) return freeDateSlots;

            FindDateSlotsAmongReservations(accommodationReservations, freeDateSlots, StartDate, EndDate, calendarReservationDays);

            return freeDateSlots;
        }

        private void FindDateSlotsAmongReservations(
            List<Reservation> accommodationReservations, List<DateSlot> freeDateSlots, DateTime StartDate, DateTime EndDate, int calendarReservationDays)
        {
            DateSlot dateSlot;

            foreach (Reservation reservation in accommodationReservations)
            {
                bool isFoundGap = (StartDate.AddDays(calendarReservationDays) < reservation.StartDate) &&
                                  (StartDate.AddDays(calendarReservationDays) <= EndDate);
                if (isFoundGap)
                {
                    if (accommodationReservations.Last() == reservation)
                    {
                        AddDateSlotsWithinLastReservation(freeDateSlots, reservation, StartDate, EndDate, calendarReservationDays);

                        return;
                    }

                    if (reservation.StartDate > EndDate)
                    {
                        dateSlot = new DateSlot(StartDate, EndDate);
                    }
                    else
                    {
                        dateSlot = new DateSlot(StartDate, reservation.StartDate.AddDays(-1));
                    }

                    freeDateSlots.Add(dateSlot);
                }

                else
                {
                    bool isFoundGapAfterLastReservation = (accommodationReservations.Last() == reservation) &&
                            (reservation.EndDate.AddDays(calendarReservationDays + 1) <= EndDate);
                    if (isFoundGapAfterLastReservation)
                    {
                        dateSlot = new DateSlot(reservation.EndDate.AddDays(1), EndDate);
                        freeDateSlots.Add(dateSlot);
                        return;
                    }
                }


                bool isStartDateOverlappedByReservation = reservation.EndDate.AddDays(1) >= StartDate;
                if (isStartDateOverlappedByReservation)
                {
                    StartDate = reservation.EndDate.AddDays(1);
                }
            }
        }

        private void AddDateSlotsWithinLastReservation(
            List<DateSlot> freeDateSlots, Reservation reservation, DateTime StartDate, DateTime EndDate, int calendarReservationDays)
        {
            DateSlot dateSlot;
            if (reservation.EndDate.AddDays(calendarReservationDays) <= EndDate)
            {
                dateSlot = new DateSlot(StartDate,
                    reservation.StartDate.AddDays(-1));
                freeDateSlots.Add(dateSlot);
                dateSlot = new DateSlot(reservation.EndDate.AddDays(1), EndDate);
                freeDateSlots.Add(dateSlot);
            }
            else
            {
                dateSlot = new DateSlot(StartDate, EndDate);
                freeDateSlots.Add(dateSlot);
            }
        }

        private bool IsDateSlotBeforeReservations(
            List<Reservation> accommodationReservations, List<DateSlot> freeDateSlots, DateTime StartDate, DateTime EndDate, int calendarReservationDays)
        {
            if (accommodationReservations.First().StartDate > EndDate)
            {
                if (StartDate.AddDays(calendarReservationDays) <= EndDate)
                {
                    DateSlot temporary = new DateSlot(StartDate, EndDate);
                    freeDateSlots.Add(temporary);
                    return true;
                }
            }

            return false;
        }

        private bool IsDateSlotAfterReservations(
            List<Reservation> accommodationReservations, List<DateSlot> freeDateSlots, DateTime StartDate, DateTime EndDate, int calendarReservationDays)
        {
            if (accommodationReservations.Last().EndDate < StartDate)
            {
                if (StartDate.AddDays(calendarReservationDays) <= EndDate)
                {
                    DateSlot temporary = new DateSlot(StartDate, EndDate);
                    freeDateSlots.Add(temporary);
                    return true;
                }
            }

            return false;
        }

    }
}
