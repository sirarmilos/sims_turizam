using InitialProject.DTO;
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
    public class ReservationReschedulingRequestRepository
    {
        private ReservationRepository reservationRepository;

        private const string FilePathRescheduledReservations = "../../../Resources/Data/rescheduledreservations.csv";

        private readonly Serializer<ReservationReschedulingRequest> reservationReschedulingRequestSerializer;

        private List<ReservationReschedulingRequest> reservationReschedulingRequests;

        public ReservationReschedulingRequestRepository()
        {
            reservationReschedulingRequestSerializer = new Serializer<ReservationReschedulingRequest>();
            reservationReschedulingRequests = reservationReschedulingRequestSerializer.FromCSV(FilePathRescheduledReservations);
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
    }
}
