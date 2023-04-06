using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class ReservationRepository
    {
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

        public List<Review> FindReservationsForReviews(List<Review> allReviews)
        {
            foreach (Review temporaryReview in allReviews)
            {
                foreach (Reservation temporaryReservation in reservations)
                {
                    if (temporaryReview.Reservation.ReservationId == temporaryReservation.ReservationId)
                    {
                        temporaryReview.Reservation = temporaryReservation;
                        foreach(Accommodation temporaryAccommodation in accommodations)
                        {
                            if (temporaryReview.Reservation.Accommodation.Id == temporaryAccommodation.Id)
                            {
                                temporaryReview.Reservation.Accommodation = temporaryAccommodation;
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            return allReviews;
        }

        public List<RateGuest> FindReservationsForRateGuestsReview(List<RateGuest> allGuests)
        {
            foreach (RateGuest temporaryRateGuest in allGuests)
            {
                foreach (Reservation temporaryReservation in reservations)
                {
                    if (temporaryRateGuest.Reservation.ReservationId == temporaryReservation.ReservationId)
                    {
                        temporaryRateGuest.Reservation = temporaryReservation;
                        foreach (Accommodation temporaryAccommodation in accommodations)
                        {
                            if (temporaryRateGuest.Reservation.Accommodation.Id == temporaryAccommodation.Id)
                            {
                                temporaryRateGuest.Reservation.Accommodation = temporaryAccommodation;
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            return allGuests;
        }

        public List<RateGuest> FindReservationsForRateGuests(List<RateGuest> allRateGuests)
        {
            List<RateGuest> temporaryRateTheGuests = new List<RateGuest>();

            foreach (RateGuest temporaryRateGuest in allRateGuests)
            {
                foreach (Reservation temporaryReservation in reservations)
                {
                    if (temporaryRateGuest.Reservation.ReservationId == temporaryReservation.ReservationId)
                    {
                        temporaryRateGuest.Reservation = temporaryReservation;
                        foreach (Accommodation temporaryAccommodation in accommodations)
                        {
                            if (temporaryRateGuest.Reservation.Accommodation.Id == temporaryAccommodation.Id)
                            {
                                temporaryRateGuest.Reservation.Accommodation = temporaryAccommodation;
                                temporaryRateTheGuests.Add(temporaryRateGuest);
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            return temporaryRateTheGuests;
        }

        public List<ReservationReschedulingRequest> FindReservationsForReservationReschedulingRequests(List<ReservationReschedulingRequest> allReservationReschedulingRequests, List<Reservation> ownerReservations)
        {
            List<ReservationReschedulingRequest> temporaryReservationReschedulingRequests = new List<ReservationReschedulingRequest>();

            foreach (ReservationReschedulingRequest temporaryReservationReschedulingRequest in allReservationReschedulingRequests)
            {
                foreach (Reservation temporaryReservation in ownerReservations)
                {
                    if (temporaryReservationReschedulingRequest.Reservation.ReservationId == temporaryReservation.ReservationId)
                    {
                        temporaryReservationReschedulingRequest.Reservation = temporaryReservation;
                        foreach (Accommodation temporaryAccommodation in accommodations)
                        {
                            if (temporaryReservationReschedulingRequest.Reservation.Accommodation.Id == temporaryAccommodation.Id)
                            {
                                temporaryReservationReschedulingRequest.Reservation.Accommodation = temporaryAccommodation;
                                temporaryReservationReschedulingRequests.Add(temporaryReservationReschedulingRequest);
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            return temporaryReservationReschedulingRequests;
        }

        public void UpdateReservations(List<Reservation> reservations)
        {
            reservationSerializer.ToCSV(FilePathReservation, reservations);
        }

        public List<Reservation> FindAllReservations()
        {
            /*reservations = new List<Reservation>();
            reservations = reservationSerializer.FromCSV(FilePathReservation);
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
            }*/

            return reservations;
        }

        public void Save(string guestUsername, Accommodation accommodation, DateTime startDate, DateTime endDate, int guestsNumber)
        {

            reservations = reservationSerializer.FromCSV(FilePathReservation);
            Reservation reservation = new Reservation(NextIdReservation(), "username123", accommodation, startDate, endDate, guestsNumber);
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

    }
}
