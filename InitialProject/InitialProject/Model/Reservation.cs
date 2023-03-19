using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Reservation : ISerializable
    {
        public int ReservationId { get; set; }

        public string GuestUsername { get; set; }

        public Accommodation Accommodation { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int GuestsNumber { get; set; }

        public Reservation() { }

        public Reservation(int reservationId, string guestUsername, Accommodation accommodation, DateTime startDate, DateTime endDate, int guestsNumber)
        {
            ReservationId = reservationId;
            GuestUsername = guestUsername;
            Accommodation = accommodation;
            StartDate = startDate;
            EndDate = endDate;
            GuestsNumber = guestsNumber;
        }

        public string[] ToCSV()
        {
            // to do
            string[] csvValues = { "a", "b", "c", "d" };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (string.IsNullOrWhiteSpace(values[0])) return;

            ReservationId = Convert.ToInt32(values[0]);
            GuestUsername = values[1];
            Accommodation = new Accommodation() { Id = Convert.ToInt32(values[2]) };
            StartDate = Convert.ToDateTime(values[3]);
            EndDate = Convert.ToDateTime(values[4]);
            GuestsNumber = Convert.ToInt32(values[5]);
        }

    }
}
