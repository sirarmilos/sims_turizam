using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace InitialProject.Repository
{
    internal class CanceledReservationRepository : ICanceledReservationRepository
    {
        private AccommodationRepository accommodationRepository;

        private const string FilePathCanceledReservations = "../../../Resources/Data/canceledreservations.csv";

        private readonly Serializer<CanceledReservation> canceledReservationsSerializer;

        private List<CanceledReservation> canceledReservations;

        public CanceledReservationRepository()
        {
            canceledReservationsSerializer = new Serializer<CanceledReservation>();
        }
        public void Save(CanceledReservation canceledReservation)
        {
            canceledReservations = canceledReservationsSerializer.FromCSV(FilePathCanceledReservations);
            canceledReservations.Add(canceledReservation);
            canceledReservationsSerializer.ToCSV(FilePathCanceledReservations, canceledReservations);
        }

        public void Save(List<CanceledReservation> allCanceledReservations)
        {
            canceledReservationsSerializer.ToCSV(FilePathCanceledReservations, allCanceledReservations);
        }

        public List<CanceledReservation> FindAll()
        {
            accommodationRepository = new AccommodationRepository();

            canceledReservations = canceledReservationsSerializer.FromCSV(FilePathCanceledReservations);

            foreach (CanceledReservation temporaryCanceledReservation in canceledReservations.ToList())
            {
                temporaryCanceledReservation.Accommodation = accommodationRepository.FindById(temporaryCanceledReservation.Accommodation.Id);
            }

            return canceledReservations;
        }

        public List<CanceledReservation> FindByOwnerUsername(string ownerUsername)
        {
            return FindAll().ToList().FindAll(x => x.Accommodation.OwnerUsername.Equals(ownerUsername) == true);
        }

        public List<CanceledReservation> FindUnreadCancelledReservationsByOwnerUsername(string ownerUsername)
        {
            return FindByOwnerUsername(ownerUsername).ToList().FindAll(x => x.ViewedByOwner.Equals(false) == true);
        }

        public CanceledReservation FindByDTO(CancelledReservationsNotificationDTO cancelledReservationsNotificationDTO)
        {
            return FindAll().ToList().Find(x => x.Accommodation.AccommodationName.Equals(cancelledReservationsNotificationDTO.AccommodationName) == true && x.StartDate.Equals(cancelledReservationsNotificationDTO.ReservationStartDate) == true && x.EndDate.Equals(cancelledReservationsNotificationDTO.ReservationEndDate) == true);
        }

        public void UpdateViewed(List<CancelledReservationsNotificationDTO> unreadCancelledReservations)
        {
            List<CanceledReservation> allCanceledReservation = FindAll();

            foreach (CancelledReservationsNotificationDTO temporaryCancelledReservationsNotificationDTO in unreadCancelledReservations.ToList())
            {
                allCanceledReservation.Where(x => x.ReservationId == temporaryCancelledReservationsNotificationDTO.ReservationId).SetValue(x => x.ViewedByOwner = true);
            }

            Save(allCanceledReservation);
        }

        public List<CanceledReservation> FindByAccommodationId(int accommodationId)
        {
            return FindAll().ToList().FindAll(x => x.Accommodation.Id == accommodationId);
        }

        public int FindAccommodationCanceledReservationCountByYear(int accommodationId, int year)
        {
            return FindByAccommodationId(accommodationId).ToList().FindAll(x => x.CancellationDate.Year == year).Count;
        }

        public List<CanceledReservation> FindAccommodationCanceledReservationsByYear(int accommodationId, int year)
        {
            return FindByAccommodationId(accommodationId).ToList().FindAll(x => x.CancellationDate.Year == year);
        }
    }
}
