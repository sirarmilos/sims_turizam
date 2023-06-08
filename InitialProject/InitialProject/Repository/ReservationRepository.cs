using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private AccommodationRepository accommodationRepository;

        private const string FilePathReservation = "../../../Resources/Data/reservations.csv";

        private readonly Serializer<Reservation> reservationSerializer;

        private List<Reservation> reservations;

        public ReservationRepository()
        {
            reservationSerializer = new Serializer<Reservation>();
        }

        public void SaveReservations(List<Reservation> reservations)
        {
            reservationSerializer.ToCSV(FilePathReservation, reservations);
        }

        public List<Reservation> FindAll()
        {
            accommodationRepository = new AccommodationRepository();

            reservations = reservationSerializer.FromCSV(FilePathReservation);

            foreach(Reservation temporaryReservation in reservations.ToList())
            {
                temporaryReservation.Accommodation = accommodationRepository.FindById(temporaryReservation.Accommodation.Id);
            }

            return reservations;
        }

        public List<Reservation> FindByOwnerUsername(string ownerUsername)
        {
            return FindAll().ToList().FindAll(x => x.Accommodation.OwnerUsername.Equals(ownerUsername) == true && x.Accommodation.Removed == false);
        }

        public Reservation FindById(int reservationId)
        {
            return FindAll().ToList().Find(x => x.ReservationId == reservationId);
        }

        public string FindOwnerByReservationId(int reservationId)
        {
            return FindAll().Find(x => x.ReservationId == reservationId).Accommodation.OwnerUsername;
        }

        public void UpdateDatesToSelectedBookingMoveRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest)
        {
            List<Reservation> allReservations = FindAll();
            allReservations.Where(x => x.ReservationId == selectedBookingMoveRequest.ReservationId && x.Accommodation.Removed == false).SetValue(x => x.StartDate = selectedBookingMoveRequest.NewStartDate).ToList().SetValue(x => x.EndDate = selectedBookingMoveRequest.NewEndDate);
            SaveReservations(allReservations);
        }

        public void RemoveById(int reservationId, int cancelledReservationId)
        {
            List<Reservation> allReservations = FindAll();
            allReservations.Remove(allReservations.Find(x => x.ReservationId == cancelledReservationId && x.ReservationId != reservationId && x.Accommodation.Removed == false));
            SaveReservations(allReservations);
        }

        public void RemoveById(int reservationId)
        {
            List<Reservation> allReservations = FindAll();
            allReservations.Remove(allReservations.Find(x => x.ReservationId == reservationId));
            SaveReservations(allReservations);
        }

        public void Save(string guest1Username, Accommodation accommodation, DateTime startDate, DateTime endDate, int guestsNumber)
        {
            reservations = reservationSerializer.FromCSV(FilePathReservation);
            Reservation reservation = new Reservation(NextId(), guest1Username, accommodation, startDate, endDate, guestsNumber);
            reservations.Add(reservation);
            reservationSerializer.ToCSV(FilePathReservation, reservations);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }
            return FindAll().Max(c => c.ReservationId) + 1;
        }

        public List<Reservation> FindAllByAccommodation(int id)
        {
            return FindAll().FindAll(x => x.Accommodation.Id == id);
        }

        public List<Reservation> FindGuest1Reservations(string guest1)
        {
            return FindAll().ToList().FindAll(x => x.GuestUsername.Equals(guest1) == true);
        }

        public List<Reservation> FindReservationsByAccommodationName(string accommodationName)
        {
            return FindAll().ToList().FindAll(x => x.Accommodation.AccommodationName.Equals(accommodationName) == true);
        }

        public List<Reservation> FindByAccommodationId(int accommodationId)
        {
            return FindAll().ToList().FindAll(x => x.Accommodation.Id == accommodationId && x.Accommodation.Removed == false);
        }

        public int FindAccommodationReservationCountByYear(int accommodationId, int year)
        {
            return FindByAccommodationId(accommodationId).ToList().FindAll(x => x.StartDate.Year == year || x.EndDate.Year == year).Count;
        }

        public List<Reservation> FindAccommodationReservationsByYear(int accommodationId, int year)
        {
            return FindByAccommodationId(accommodationId).ToList().FindAll(x => x.StartDate.Year == year || x.EndDate.Year == year);
        }

        public bool IsFutureReservationExistByLocationId(int locationId, string ownerUsername)
        {
            return FindAll().ToList().Exists(x => x.Accommodation.Location.Id == locationId && (x.StartDate.Subtract(DateTime.Now).Days > 0) == true && x.Accommodation.OwnerUsername.Equals(ownerUsername) == true);
        }
    }
}
