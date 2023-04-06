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
            return reservationReschedulingRequests;
        }

        public void UpdateReservationReschedulingRequest(List<ReservationReschedulingRequest> reservationReschedulingRequests)
        {
            reservationReschedulingRequestSerializer.ToCSV(FilePathRescheduledReservations, reservationReschedulingRequests);
        }
    }
}
