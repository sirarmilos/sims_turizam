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

        private const string FilePathAccommodation = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Reservation> reservationSerializer;

        private readonly Serializer<Accommodation> accommodationSerializer;

        private List<Reservation> reservations;

        private List<Accommodation> accommodations;

        public ReservationRepository()
        {
            reservationSerializer = new Serializer<Reservation>();
            reservations = reservationSerializer.FromCSV(FilePathReservation);

            accommodationSerializer = new Serializer<Accommodation>();
            accommodations = accommodationSerializer.FromCSV(FilePathAccommodation);
            
            foreach (Reservation reservation in reservations)
            {
                if (accommodations == null)
                    break;
                foreach (Accommodation accommodation in accommodations)
                {
                    if (accommodation.Id == reservation.Accommodation.Id)
                    {
                        reservation.Accommodation = accommodation;
                        break;
                    }
                }
            }
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
            return FindAll().ToList().FindAll(x => x.Accommodation.OwnerUsername.Equals(ownerUsername) == true);
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
            allReservations.Where(x => x.ReservationId == selectedBookingMoveRequest.ReservationId).SetValue(x => x.StartDate = selectedBookingMoveRequest.NewStartDate).ToList().SetValue(x => x.EndDate = selectedBookingMoveRequest.NewEndDate);
            SaveReservations(allReservations);
        }

        public void RemoveById(int reservationId, int cancelledReservationId)
        {
            List<Reservation> allReservations = FindAll();
            allReservations.Remove(allReservations.Find(x => x.ReservationId == cancelledReservationId && x.ReservationId != reservationId));
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
            List<Reservation> accommodationReservations = new List<Reservation>();

            foreach (Reservation reservation in reservations)
            {
                if (reservation.Accommodation.Id == id)
                {
                    accommodationReservations.Add(reservation);
                }
            }

            return accommodationReservations;
        }

        public List<Reservation> FindGuest1Reservations(string guest1)
        {
            return FindAll().ToList().FindAll(x => x.GuestUsername.Equals(guest1) == true);
        }
    }
}
