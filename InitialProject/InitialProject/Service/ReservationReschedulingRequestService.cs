using InitialProject.DTO;
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
        private readonly ReservationReschedulingRequestRepository reservationReschedulingRequestRepository;

        private readonly ReservationService reservationService;

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

        public ReservationReschedulingRequestService(string owner)
        {
            Owner = owner;
            reservationReschedulingRequestRepository = new ReservationReschedulingRequestRepository();
            reservationService = new ReservationService(Owner);

            ListInitialization();
        }

        private void ListInitialization()
        {
            BusyReservations = new List<BusyReservation>();
        }

        public ReservationReschedulingRequestService(ReservationReschedulingRequestService reservationReschedulingRequestService, string owner)
        {
            Owner = owner;
            reservationReschedulingRequestRepository = reservationReschedulingRequestService.reservationReschedulingRequestRepository;//new ReservationReschedulingRequestRepository();
            reservationService = new ReservationService();
            /*reservationRepository = reservationReschedulingRequestService.reservationRepository;// new ReservationRepository();

            AllReservations = reservationReschedulingRequestService.AllReservations;// new List<Reservation>();
            OwnerReservations = reservationReschedulingRequestService.OwnerReservations; // new List<Reservation>();
            AllReservationReschedulingRequests = reservationReschedulingRequestService.AllReservationReschedulingRequests; // new List<ReservationReschedulingRequest>();
            OwnerReservationReschedulingRequests = reservationReschedulingRequestService.OwnerReservationReschedulingRequests; // new List<ReservationReschedulingRequest>();
            OwnerPendingReservationReschedulingRequests = reservationReschedulingRequestService.OwnerPendingReservationReschedulingRequests; // new List<ReservationReschedulingRequest>();
            OwnerBookingMoveRequestsDTOs = reservationReschedulingRequestService.OwnerBookingMoveRequestsDTOs; // new List<OwnerBookingMoveRequestsDTO>();*/
            BusyReservations = reservationReschedulingRequestService.BusyReservations; // new List<BusyReservation>();
        }

        public List<OwnerBookingMoveRequestsDTO> FindPendingReservationReschedulingRequests()
        {
            List<Reservation> ownerReservations = new List<Reservation>();
            ownerReservations = reservationService.FindOwnerReservations();

            List<ReservationReschedulingRequest> ownerPendingReservationReschedulingRequests = reservationReschedulingRequestRepository.FindPendingReservationReschedulingRequestByOwnerUsername(Owner);

            return FindShowOwnerBookingMoveRequestsDTO(ownerReservations, ownerPendingReservationReschedulingRequests);
        }

        public List<OwnerBookingMoveRequestsDTO> FindShowOwnerBookingMoveRequestsDTO(List<Reservation> ownerReservations, List<ReservationReschedulingRequest> ownerPendingReservationReschedulingRequests)
        {
            List<OwnerBookingMoveRequestsDTO> ownerBookingMoveRequestsDTOs = new List<OwnerBookingMoveRequestsDTO>();

            foreach (Reservation temporaryReservation in ownerReservations.ToList())
            {
                ReservationReschedulingRequest temporaryReservationReschedulingRequest = new ReservationReschedulingRequest();
                temporaryReservationReschedulingRequest = reservationReschedulingRequestRepository.FindPendingReservationReschedulingRequestByReservationId(temporaryReservation.ReservationId, Owner);

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
            List<Reservation> reservationsWithAccommodationId = reservationService.FindAllReservationsByAccommodationId(accommodationId);

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
            reservationService.UpdateDatesForSelectedBookingMoveRequest(selectedBookingMoveRequest);

            reservationReschedulingRequestRepository.UpdateReservationReschedulingRequestToSelectedBookingMoveRequest(selectedBookingMoveRequest, "accepted", "");

            UpdateReservationsAndBookingMoveRequests(selectedBookingMoveRequest);

            return FindPendingReservationReschedulingRequests();
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

            // ovde stao, BusyReservations neka ostane globalna, posto je potrebna i u drugoj funkciji
            // azuriram fajlove svaki put kada se izvrsi nesto, jer nema listi
            // nije potrebno azurirati ownerBookingMoveRequestsDTOs, jer ce se na kraju pozvati FindPendingReservationReschedulingRequests() koja ce to odraditi

            RemoveReservationReschedulingRequestsToCancelledReservations(selectedBookingMoveRequest, cancelledReservations);

            RemoveCancelledReservations(selectedBookingMoveRequest, cancelledReservations); 

            BusyReservations.Remove(busyReservationToSelectedBookingMoveRequest);
        }

        private void RemoveCancelledReservations(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, List<Reservation> cancelledReservations)
        {
            reservationService.RemoveCancelledReservations(selectedBookingMoveRequest, cancelledReservations);
        }

        private void RemoveReservationReschedulingRequestsToCancelledReservations(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, List<Reservation> cancelledReservations)
        {
            foreach (Reservation temporaryReservation in cancelledReservations.ToList())
            {
                reservationReschedulingRequestRepository.RemoveReservationReschedulingRequestByReservationId(temporaryReservation.ReservationId);
            }
        }

        public void SaveRejectedRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, string comment)
        {
            reservationReschedulingRequestRepository.UpdateReservationReschedulingRequestToSelectedBookingMoveRequest(selectedBookingMoveRequest, "rejected", comment);

            FindPendingReservationReschedulingRequests();
        }
    }
}
