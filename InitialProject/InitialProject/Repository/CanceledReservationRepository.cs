using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System.Collections.Generic;

namespace InitialProject.Repository
{
    internal class CanceledReservationRepository : ICanceledReservationRepository
    {
        private const string FilePathCanceledReservations = "../../../Resources/Data/canceledreservations.csv";

        private readonly Serializer<Reservation> canceledReservationsSerializer;

        private List<Reservation> canceledReservations;

        public CanceledReservationRepository()
        {
            canceledReservationsSerializer = new Serializer<Reservation>();
        }

        public void Save(Reservation reservation)
        {
            canceledReservations = canceledReservationsSerializer.FromCSV(FilePathCanceledReservations);
            canceledReservations.Add(reservation);
            canceledReservationsSerializer.ToCSV(FilePathCanceledReservations, canceledReservations);
        }
    }
}
