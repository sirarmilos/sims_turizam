using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<ReservationReschedulingRequest> FindAllReservationReschedulingRequests()
        {
            reservationRepository = new ReservationRepository();

            reservationReschedulingRequests = reservationReschedulingRequestSerializer.FromCSV(FilePathRescheduledReservations);

            foreach (ReservationReschedulingRequest temporaryReservationReschedulingRequest in reservationReschedulingRequests.ToList())
            {
                temporaryReservationReschedulingRequest.Reservation = reservationRepository.FindReservationByReservationId(temporaryReservationReschedulingRequest.Reservation.ReservationId);
            }

            return reservationReschedulingRequests;
        }

        public void UpdateReservationReschedulingRequest(List<ReservationReschedulingRequest> reservationReschedulingRequests)
        {
            reservationReschedulingRequestSerializer.ToCSV(FilePathRescheduledReservations, reservationReschedulingRequests);
        }

        public List<ReservationReschedulingRequest> FindReservationReschedulingRequestByOwnerUsername(string ownerUsername)
        {
            return FindAllReservationReschedulingRequests().ToList().FindAll(x => x.Reservation.Accommodation.OwnerUsername.Equals(ownerUsername) == true);
        }

        public List<ReservationReschedulingRequest> FindPendingReservationReschedulingRequestByOwnerUsername(string ownerUsername)
        {
            return FindReservationReschedulingRequestByOwnerUsername(ownerUsername).ToList().FindAll(x => x.Status.Equals("pending") == true);
        }

        public ReservationReschedulingRequest FindPendingReservationReschedulingRequestByReservationId(int reservationId, string ownerUsername)
        {
            return FindPendingReservationReschedulingRequestByOwnerUsername(ownerUsername).ToList().Find(x => x.Reservation.ReservationId == reservationId);
        }

        public void UpdateStatusForSelectedBookingMoveRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest)
        {
            List<ReservationReschedulingRequest> allReservationReschedulingRequests = FindAllReservationReschedulingRequests();
            allReservationReschedulingRequests.Where(x => x.Reservation.ReservationId == selectedBookingMoveRequest.ReservationId).SetValue(x => x.Status = "accepted");
            SaveReservationReschedulingRequests(allReservationReschedulingRequests);
        }

        public void SaveReservationReschedulingRequests(List<ReservationReschedulingRequest> allReservationReschedulingRequests)
        {
            reservationReschedulingRequestSerializer.ToCSV(FilePathRescheduledReservations, allReservationReschedulingRequests);
        }
    }
}
