using InitialProject.DTO;
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
    internal class ReservationRepository
    {
        private AccommodationRepository accommodationRepository;

        private const string FilePathReservation = "../../../Resources/Data/reservation.csv";

        private const string FilePathAccommodation = "../../../Resources/Data/accommodation.csv";

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

        public void /* UpdateReservations*/ SaveReservations(List<Reservation> reservations)
        {
            reservationSerializer.ToCSV(FilePathReservation, reservations);
        }

        public List<Reservation> FindAllReservations()
        {
            accommodationRepository = new AccommodationRepository();

            reservations = reservationSerializer.FromCSV(FilePathReservation);

            foreach(Reservation temporaryReservation in reservations.ToList())
            {
                temporaryReservation.Accommodation = accommodationRepository.FindAccommodationByAccommodationId(temporaryReservation.Accommodation.Id);
            }

            return reservations;
        }

        public List<Reservation> FindReservationsByOwnerUsername(string ownerUsername)
        {
            return FindAllReservations().ToList().FindAll(x => x.Accommodation.OwnerUsername.Equals(ownerUsername) == true);
        }

        public Reservation FindReservationByReservationId(int reservationId)
        {
            return FindAllReservations().ToList().Find(x => x.ReservationId == reservationId);
        }

        public string FindOwnerByReservationId(int reservationId)
        {
            return FindAllReservations().Find(x => x.ReservationId == reservationId).Accommodation.OwnerUsername;
        }

        public void UpdateDatesForSelectedBookingMoveRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest)
        {
            List<Reservation> allReservations = FindAllReservations();
            allReservations.Where(x => x.ReservationId == selectedBookingMoveRequest.ReservationId).SetValue(x => x.StartDate = selectedBookingMoveRequest.NewStartDate).ToList().SetValue(x => x.EndDate = selectedBookingMoveRequest.NewEndDate);
            SaveReservations(allReservations);
        }

        public void Save(string guestUsername, Accommodation accommodation, DateTime startDate, DateTime endDate, int guestsNumber)
        {

            reservations = reservationSerializer.FromCSV(FilePathReservation);
            Reservation reservation = new Reservation(NextIdReservation(), "username123", accommodation, startDate, endDate, guestsNumber); // todo: izmeniti username
            reservations.Add(reservation);
            reservationSerializer.ToCSV(FilePathReservation, reservations);

        }

        public int NextIdReservation()
        {
            reservations = reservationSerializer.FromCSV(FilePathReservation);
            if (reservations.Count < 1)
            {
                return 1;
            }
            return reservations.Max(c => c.ReservationId) + 1;
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

        /* public List<Reservation> FindAllReservationsByAccommodationid(int accommodationId)
        {
            return FindOwnerReservations().ToList().FindAll(x => x.Accommodation.Id == accommodationId);
        }*/






        public List<Reservation> FindGuest1Reservations(string guest1)
        {
            return FindAllReservations().ToList().FindAll(x => x.GuestUsername.Equals(guest1) == true);
        }

        public Reservation FindById(int id)
        {
            return FindAllReservations().ToList().Find(x => x.ReservationId == id);
        }

    }
}
