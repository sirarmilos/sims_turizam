using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class CanceledReservation : ISerializable
    {
        public int ReservationId { get; set; }

        public string GuestUsername { get; set; }

        public Accommodation Accommodation { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CancellationDate { get; set; }

        public int GuestsNumber { get; set; }

        public bool ViewedByOwner { get; set; }

        public CanceledReservation() { }

        public CanceledReservation(Reservation reservation, bool viewedByOwner)
        {
            ReservationId = reservation.ReservationId;
            GuestUsername = reservation.GuestUsername;
            Accommodation = reservation.Accommodation;
            StartDate = reservation.StartDate;
            EndDate = reservation.EndDate;
            CancellationDate = DateTime.Now;
            GuestsNumber = reservation.GuestsNumber;
            ViewedByOwner = viewedByOwner;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { ReservationId.ToString(), GuestUsername, Accommodation.Id.ToString(), StartDate.ToString("dd.MM.yyyy"), EndDate.ToString("dd.MM.yyyy"), CancellationDate.ToString("dd.MM.yyyy"), GuestsNumber.ToString(), ViewedByOwner.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            // return if the file was empty
            if (string.IsNullOrWhiteSpace(values[0])) return;

            ReservationId = Convert.ToInt32(values[0]);
            GuestUsername = values[1];
            Accommodation = new Accommodation() { Id = Convert.ToInt32(values[2]) };

            string temporaryDate = values[3];
            if (!string.IsNullOrEmpty(temporaryDate))
            {
                StartDate = DateTime.ParseExact(temporaryDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            temporaryDate = values[4];
            if (!string.IsNullOrEmpty(temporaryDate))
            {
                EndDate = DateTime.ParseExact(temporaryDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            temporaryDate = values[5];
            if (!string.IsNullOrEmpty(temporaryDate))
            {
                CancellationDate = DateTime.ParseExact(temporaryDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            GuestsNumber = Convert.ToInt32(values[6]);
            ViewedByOwner = Convert.ToBoolean(values[7]);
        }
    }
}