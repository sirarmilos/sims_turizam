using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Service
{
    public class ReservationReschedulingRequestService
    {
        private readonly IReservationReschedulingRequestRepository reservationReschedulingRequestRepository;

        private readonly ReservationService reservationService;

        private readonly RateGuestsService rateGuestsService;

        public List<BusyReservation> BusyReservations
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

        private string guest1;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public ReservationReschedulingRequestService(string username)
        {
            Owner = username;
            Guest1 = username;
            reservationReschedulingRequestRepository = new ReservationReschedulingRequestRepository();
            reservationService = new ReservationService(Owner);
            rateGuestsService = new RateGuestsService(Owner);

            BusyReservations = new List<BusyReservation>();
        }

        public List<OwnerBookingMoveRequestsDTO> FindPendingRequests()
        {
            List<Reservation> ownerReservations = new List<Reservation>();
            ownerReservations = reservationService.FindOwnerReservations();

            List<ReservationReschedulingRequest> ownerPendingRequests = reservationReschedulingRequestRepository.FindPendingRequestsByOwnerUsername(Owner);

            return FindShowOwnerBookingMoveRequestsDTO(ownerReservations, ownerPendingRequests);
        }

        public List<OwnerBookingMoveRequestsDTO> FindShowOwnerBookingMoveRequestsDTO(List<Reservation> ownerReservations, List<ReservationReschedulingRequest> ownerPendingRequests)
        {
            List<OwnerBookingMoveRequestsDTO> ownerBookingMoveRequestsDTOs = new List<OwnerBookingMoveRequestsDTO>();

            foreach (Reservation temporaryReservation in ownerReservations.ToList())
            {
                ReservationReschedulingRequest temporaryReservationReschedulingRequest = new ReservationReschedulingRequest();
                temporaryReservationReschedulingRequest = reservationReschedulingRequestRepository.FindPendingRequestByReservationId(temporaryReservation.ReservationId, Owner);

                if (temporaryReservationReschedulingRequest != null)
                {
                    string newDateAvailable = DateAvailability(temporaryReservationReschedulingRequest, temporaryReservation.Accommodation.Id);

                    OwnerBookingMoveRequestsDTO temporaryOwnerBookingMoveRequestsDTO = new OwnerBookingMoveRequestsDTO(temporaryReservation, temporaryReservationReschedulingRequest, newDateAvailable);
                    ownerBookingMoveRequestsDTOs.Add(temporaryOwnerBookingMoveRequestsDTO);
                }
            }

            return ownerBookingMoveRequestsDTOs;
        }

        private string DateAvailability(ReservationReschedulingRequest reservationReschedulingRequest, int accommodationId)
        {
            List<Reservation> reservationsWithAccommodationId = reservationService.FindReservationsByAccommodationId(accommodationId);

            List<Reservation> reservationsToBusyReservations = new List<Reservation>();

            reservationsToBusyReservations.Add(reservationsWithAccommodationId.ToList().Find(x => x.ReservationId == reservationReschedulingRequest.Reservation.ReservationId));

            reservationsToBusyReservations.AddRange(reservationsWithAccommodationId.ToList().FindAll(x => (IsDateBusy(x.StartDate, x.EndDate, reservationReschedulingRequest.NewStartDate, reservationReschedulingRequest.NewEndDate) == false && x.ReservationId != reservationReschedulingRequest.Reservation.ReservationId)));

            BusyReservations.Add(new BusyReservation(reservationReschedulingRequest.Reservation.ReservationId, reservationsToBusyReservations));

            if (reservationsToBusyReservations.Count > 1) // 1 jer se ubacuje taj bas koji treba da se odobri ili ne
            {
                return "Busy";
            }

            return "Available";
        }

        private bool IsDateBusy(DateTime startDate, DateTime endDate, DateTime newStartDate, DateTime newEndDate)
        {
            bool dateCheckNewStartDate = (DateTime.Compare(startDate, newStartDate) < 0 && DateTime.Compare(endDate, newStartDate) < 0);
            bool dateCheckNewEndDate = (DateTime.Compare(startDate, newEndDate) > 0 && DateTime.Compare(endDate, newEndDate) > 0);

            return dateCheckNewStartDate || dateCheckNewEndDate;
        }

        public List<OwnerBookingMoveRequestsDTO> SaveAcceptedRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest)
        {
            reservationService.UpdateDatesToSelectedBookingMoveRequest(selectedBookingMoveRequest);

            reservationReschedulingRequestRepository.UpdateRequestToSelectedBookingMoveRequest(selectedBookingMoveRequest, "accepted", "");

            UpdateReservationsAndBookingMoveRequests(selectedBookingMoveRequest);

            return FindPendingRequests();
        }

        private void UpdateReservationsAndBookingMoveRequests(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest)
        {
            if (selectedBookingMoveRequest.NewDateAvailable.Equals("Busy") == true)
            {
                UpdateReservationsAndBookingMoveRequestsBusy(selectedBookingMoveRequest);
            }
        }

        private void UpdateReservationsAndBookingMoveRequestsBusy(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest)
        {
            BusyReservation busyReservationToSelectedBookingMoveRequest = BusyReservations.Find(x => x.ReservationId == selectedBookingMoveRequest.ReservationId);

            List<Reservation> cancelledReservations = busyReservationToSelectedBookingMoveRequest.ReservationsToDelete.FindAll(x => x.ReservationId != selectedBookingMoveRequest.ReservationId).ToList();

            RemoveRequestsToCancelledReservations(selectedBookingMoveRequest, cancelledReservations);

            RemoveCancelledReservations(selectedBookingMoveRequest, cancelledReservations); 

            BusyReservations.Remove(busyReservationToSelectedBookingMoveRequest);
        }

        private void RemoveRequestsToCancelledReservations(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, List<Reservation> cancelledReservations)
        {
            foreach (Reservation temporaryReservation in cancelledReservations.ToList())
            {
                reservationReschedulingRequestRepository.RemoveRequestByReservationId(temporaryReservation.ReservationId);
            }
        }

        private void RemoveCancelledReservations(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, List<Reservation> cancelledReservations)
        {
            reservationService.RemoveCancelledReservations(selectedBookingMoveRequest, cancelledReservations);
        }

        public void SaveRejectedRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, string comment)
        {
            reservationReschedulingRequestRepository.UpdateRequestToSelectedBookingMoveRequest(selectedBookingMoveRequest, "rejected", comment);
        }

        public int FindNumberOfUnratedGuests(string ownerUsername)
        {
            return rateGuestsService.FindNumberOfUnratedGuests(ownerUsername);
        }
    }
}
