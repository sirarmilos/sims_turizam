using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class ReservationRepository
    {
        private const string FilePathReservation = "../../../Resources/Data/reservation.csv";

        private const string FilePathAccommodation = "../../../Resources/Data/accommodation.csv";

        private const string FilePathRateGuests = "../../../Resources/Data/rateguests.csv";

        private readonly Serializer<Reservation> reservationSerializer;

        private readonly Serializer<RateGuest> rateGuestsSerializer;

        private readonly Serializer<Accommodation> accommodationSerializer;

        private List<Reservation> reservations;

        private List<RateGuest> rateGuests;

        private List<Accommodation> accommodations;

        public ReservationRepository()
        {
            reservationSerializer = new Serializer<Reservation>();
            reservations = reservationSerializer.FromCSV(FilePathReservation);

            rateGuestsSerializer = new Serializer<RateGuest>();
            rateGuests = rateGuestsSerializer.FromCSV(FilePathRateGuests);

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

        public List<Reservation> FindAllReservations()
        {
            return reservations;
        }

        public List<RateGuest> FindAllRateGuests()
        {
            return rateGuests;
        }

    }
}
