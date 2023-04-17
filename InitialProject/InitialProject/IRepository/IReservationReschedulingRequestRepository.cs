using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IReservationReschedulingRequestRepository
    {
        List<ReservationReschedulingRequest> FindAll();

        void Save(List<ReservationReschedulingRequest> allReservationReschedulingRequests);

        List<ReservationReschedulingRequest> FindRequestByOwnerUsername(string ownerUsername);

        List<ReservationReschedulingRequest> FindPendingRequestsByOwnerUsername(string ownerUsername);

        ReservationReschedulingRequest FindPendingRequestByReservationId(int reservationId, string ownerUsername);

        void UpdateRequestToSelectedBookingMoveRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, string status, string comment);

        void RemoveRequestByReservationId(int reservationId);




        public ReservationReschedulingRequest FindRequestByReservationId(int reservationId, string guest1Username);

        public void Create(Reservation reservation, DateTime newStartDate, DateTime newEndDate, string status, string comment);

        public void UpdateViewedRequestsByGuest1(string guest1Username);
    }
}
