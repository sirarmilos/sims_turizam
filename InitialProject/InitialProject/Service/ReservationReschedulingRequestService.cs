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

        public List<ReservationReschedulingRequest> AllReservationReschedulingRequests
        {
            get;
            set;
        }

        public List<ReservationReschedulingRequest> OwnerReservationReschedulingRequests
        {
            get;
            set;
        }

        public List<ReservationReschedulingRequest> OwnerPendingReservationReschedulingRequests
        {
            get;
            set;
        }

        public List<OwnerBookingMoveRequestsDTO> OwnerBookingMoveRequestsDTOs
        {
            get;
            set;
        }

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
            reservationRepository = new ReservationRepository();

            ListInitialization();
        }

        private void ListInitialization()
        {
            AllReservations = new List<Reservation>();
            OwnerReservations = new List<Reservation>();
            AllReservationReschedulingRequests = new List<ReservationReschedulingRequest>();
            OwnerReservationReschedulingRequests = new List<ReservationReschedulingRequest>();
            OwnerPendingReservationReschedulingRequests = new List<ReservationReschedulingRequest>();
            OwnerBookingMoveRequestsDTOs = new List<OwnerBookingMoveRequestsDTO>();
            BusyReservations = new List<BusyReservation>();
        }

        public ReservationReschedulingRequestService(ReservationReschedulingRequestService reservationReschedulingRequestService, string owner)
        {
            Owner = owner;
            reservationReschedulingRequestRepository = reservationReschedulingRequestService.reservationReschedulingRequestRepository;//new ReservationReschedulingRequestRepository();
            reservationRepository = reservationReschedulingRequestService.reservationRepository;// new ReservationRepository();

            AllReservations = reservationReschedulingRequestService.AllReservations;// new List<Reservation>();
            OwnerReservations = reservationReschedulingRequestService.OwnerReservations; // new List<Reservation>();
            AllReservationReschedulingRequests = reservationReschedulingRequestService.AllReservationReschedulingRequests; // new List<ReservationReschedulingRequest>();
            OwnerReservationReschedulingRequests = reservationReschedulingRequestService.OwnerReservationReschedulingRequests; // new List<ReservationReschedulingRequest>();
            OwnerPendingReservationReschedulingRequests = reservationReschedulingRequestService.OwnerPendingReservationReschedulingRequests; // new List<ReservationReschedulingRequest>();
            OwnerBookingMoveRequestsDTOs = reservationReschedulingRequestService.OwnerBookingMoveRequestsDTOs; // new List<OwnerBookingMoveRequestsDTO>();
            BusyReservations = reservationReschedulingRequestService.BusyReservations; // new List<BusyReservation>();
        }

        public List<OwnerBookingMoveRequestsDTO> FindPendingReservationReschedulingRequests()
        {
            List<Reservation> ownerReservatons = reservationService.FindOwnerReservations();

            List<ReservationReschedulingRequest> ownerPendingReservationReschedulingRequests = reservationReschedulingRequestRepository.FindPendingReservationReschedulingRequestByOwnerUsername(Owner);

            return FindShowOwnerBookingMoveRequestsDTO(ownerReservatons, ownerPendingReservationReschedulingRequests);
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

            reservationReschedulingRequestRepository.UpdateStatusForSelectedBookingMoveRequest(selectedBookingMoveRequest);

            UpdateReservationsAndBookingMoveRequests(selectedBookingMoveRequest);

            ListInitialization();

            return FindPendingReservationReschedulingRequests();
        }

        private void UpdateReservationsAndBookingMoveRequests(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest)
        {
            if (selectedBookingMoveRequest.NewDateAvailable.Equals("Available") == true)
            {
                // UpdateReservationsAndBookingMoveRequestsAvailable(selectedBookingMoveRequest);
            }
            else
            {
                UpdateReservationsAndBookingMoveRequestsBusy(selectedBookingMoveRequest);
            }

            // ova linija treba, samo treba save iz servisa, pa on pozvima reservation repository
            // reservationRepository.UpdateReservations(AllReservations);

            reservationReschedulingRequestRepository.UpdateReservationReschedulingRequest(AllReservationReschedulingRequests);
        }

        /* private void UpdateReservationsAndBookingMoveRequestsAvailable(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest)
        {
            OwnerBookingMoveRequestsDTOs.Remove(selectedBookingMoveRequest);
        }*/

        private void UpdateReservationsAndBookingMoveRequestsBusy(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest)
        {
            BusyReservation busyReservationToSelectedBookingMoveRequest = BusyReservations.Find(x => x.ReservationId == selectedBookingMoveRequest.ReservationId);

            List<Reservation> cancelledReservations = busyReservationToSelectedBookingMoveRequest.ReservationsToDelete.FindAll(x => x.ReservationId != selectedBookingMoveRequest.ReservationId).ToList();

            // ovde stao, BusyReservations neka ostane globalna, posto je potrebna i u drugoj funkciji
            // azuriram fajlove svaki put kada se izvrsi nesto, jer nema listi
            // nije potrebno azurirati ownerBookingMoveRequestsDTOs, jer ce se na kraju pozvati FindPendingReservationReschedulingRequests() koja ce to odraditi

            RemoveCancelledReservations(selectedBookingMoveRequest, cancelledReservations);

            RemoveReservationReschedulingRequestsToCancelledReservations(selectedBookingMoveRequest, cancelledReservations);/*busyReservationToSelectedBookingMoveRequest.ReservationsToDelete.ToList());*/

            BusyReservations.Remove(busyReservationToSelectedBookingMoveRequest);

            OwnerBookingMoveRequestsDTOs.Remove(selectedBookingMoveRequest);
        }

        private void RemoveCancelledReservations(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, List<Reservation> cancelledReservations)
        {
            foreach (Reservation temporaryReservation in cancelledReservations.ToList())
            {
                AllReservations.Remove(AllReservations.Find(x => x.ReservationId == temporaryReservation.ReservationId && x.ReservationId != selectedBookingMoveRequest.ReservationId));
            }
        }

        private void RemoveReservationReschedulingRequestsToCancelledReservations(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, List<Reservation> cancelledReservations)
        {
            foreach (Reservation temporaryReservation in cancelledReservations.ToList())
            {
                AllReservationReschedulingRequests.Remove(AllReservationReschedulingRequests.Find(x => x.Reservation.ReservationId == temporaryReservation.ReservationId));
                OwnerBookingMoveRequestsDTOs.Remove(OwnerBookingMoveRequestsDTOs.Find(x => x.ReservationId == temporaryReservation.ReservationId));
            }
        }

        public void SaveRejectedRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, string comment)
        {
            AllReservationReschedulingRequests.Where(x => x.Reservation.ReservationId == selectedBookingMoveRequest.ReservationId).SetValue(x => x.Status = "rejected").SetValue(x => x.Comment = comment).ToList();

            reservationReschedulingRequestRepository.UpdateReservationReschedulingRequest(AllReservationReschedulingRequests);

            OwnerBookingMoveRequestsDTOs.Remove(selectedBookingMoveRequest);
        }
    }
}
