using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Repository
{
    public class ReservationReschedulingRequestRepository : IReservationReschedulingRequestRepository
    {
        private ReservationRepository reservationRepository;

        private const string FilePathRescheduledReservations = "../../../Resources/Data/rescheduledreservations.csv";

        private readonly Serializer<ReservationReschedulingRequest> reservationReschedulingRequestSerializer;

        private List<ReservationReschedulingRequest> reservationReschedulingRequests;

        public ReservationReschedulingRequestRepository()
        {
            reservationReschedulingRequestSerializer = new Serializer<ReservationReschedulingRequest>();
        }

        public List<ReservationReschedulingRequest> FindAll()
        {
            reservationRepository = new ReservationRepository();

            reservationReschedulingRequests = reservationReschedulingRequestSerializer.FromCSV(FilePathRescheduledReservations);

            foreach (ReservationReschedulingRequest temporaryReservationReschedulingRequest in reservationReschedulingRequests.ToList())
            {
                temporaryReservationReschedulingRequest.Reservation = reservationRepository.FindById(temporaryReservationReschedulingRequest.Reservation.ReservationId);
            }

            return reservationReschedulingRequests;
        }

        public void Save(List<ReservationReschedulingRequest> allReservationReschedulingRequests)
        {
            reservationReschedulingRequestSerializer.ToCSV(FilePathRescheduledReservations, allReservationReschedulingRequests);
        }

        public List<ReservationReschedulingRequest> FindRequestByOwnerUsername(string ownerUsername)
        {
            return FindAll().ToList().FindAll(x => x.Reservation.Accommodation.OwnerUsername.Equals(ownerUsername) == true);
        }

        public List<ReservationReschedulingRequest> FindPendingRequestsByOwnerUsername(string ownerUsername)
        {
            return FindRequestByOwnerUsername(ownerUsername).ToList().FindAll(x => x.Status.Equals("pending") == true);
        }

        public ReservationReschedulingRequest FindPendingRequestByReservationId(int reservationId, string ownerUsername)
        {
            return FindPendingRequestsByOwnerUsername(ownerUsername).ToList().Find(x => x.Reservation.ReservationId == reservationId);
        }

        public void UpdateRequestToSelectedBookingMoveRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, string status, string comment)
        {
            List<ReservationReschedulingRequest> allReservationReschedulingRequests = FindAll();
            allReservationReschedulingRequests.Where(x => x.Reservation.ReservationId == selectedBookingMoveRequest.ReservationId).SetValue(x => x.Status = status).SetValue(x => x.Comment = comment);
            Save(allReservationReschedulingRequests);
        }

        public void RemoveRequestByReservationId(int reservationId)
        {
            List<ReservationReschedulingRequest> allReservationReschedulingRequests = FindAll();
            allReservationReschedulingRequests.Remove(allReservationReschedulingRequests.Find(x => x.Reservation.ReservationId == reservationId));
            Save(allReservationReschedulingRequests);
        }





        public void Create(Reservation reservation, DateTime newStartDate, DateTime newEndDate, string status, string comment) // todo: izmestiti u servis? ili srediti sa sirarovim funkcijama kao dole
        {
            ReservationReschedulingRequest requestAlreadyExists = FindRequestByReservationId(reservation.ReservationId, reservation.GuestUsername);
            List<ReservationReschedulingRequest> allReservationReschedulingRequests;

            if (requestAlreadyExists != null)
            {
                requestAlreadyExists.NewStartDate = newStartDate; 
                requestAlreadyExists.NewEndDate = newEndDate;
                requestAlreadyExists.Status = "pending";
                requestAlreadyExists.ViewedByGuest = false;

                RemoveRequestByReservationId(reservation.ReservationId);

                allReservationReschedulingRequests = FindAll();
                allReservationReschedulingRequests.Add(requestAlreadyExists);
                Save(allReservationReschedulingRequests);

                return;
            }
            
            allReservationReschedulingRequests = FindAll();
            allReservationReschedulingRequests.Add(
                new ReservationReschedulingRequest(NextId(), reservation, newStartDate, newEndDate, status, comment, false));
            Save(allReservationReschedulingRequests);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }

            return FindAll().Max(x => x.Id) + 1;
        }

        public List<ReservationReschedulingRequest> FindAllByGuest1Username(string guest1Username)
        {
            return FindAll().ToList().FindAll(x => x.Reservation.GuestUsername.Equals(guest1Username) == true);
        }

        public ReservationReschedulingRequest FindRequestByReservationId(int reservationId, string guest1Username)
        {
            return FindAllByGuest1Username(guest1Username).ToList().Find(x => x.Reservation.ReservationId == reservationId);
        }

        public void UpdateViewedRequestsByGuest1(string guest1Username) 
        {
            List<ReservationReschedulingRequest> allReservationReschedulingRequests = FindAll();
            allReservationReschedulingRequests.Where(x => x.Reservation.GuestUsername.Equals(guest1Username) && ( !x.Status.Equals("pending") && (x.ViewedByGuest == false)) ).SetValue(x => x.ViewedByGuest = true);
            Save(allReservationReschedulingRequests);
        }
    }
}
