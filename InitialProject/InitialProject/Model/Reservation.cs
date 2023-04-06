using InitialProject.Serializer;
using System;
using System.Globalization;


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

        public Reservation(Reservation reservation)
        {
            this.ReservationId = reservation.ReservationId;
            this.GuestUsername = reservation.GuestUsername;
            this.Accommodation = reservation.Accommodation;
            this.StartDate = reservation.StartDate;
            this.EndDate = reservation.EndDate;
            this.GuestsNumber = reservation.GuestsNumber;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { ReservationId.ToString(), GuestUsername, Accommodation.Id.ToString(), StartDate.ToString("dd.MM.yyyy"), EndDate.ToString("dd.MM.yyyy"), GuestsNumber.ToString() };
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

            GuestsNumber = Convert.ToInt32(values[5]);

        }

    }
}
