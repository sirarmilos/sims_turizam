using InitialProject.DTO;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class RenovationRecommendation : ISerializable
    {
        public Reservation Reservation { get; set; }

        public string Level { get; set; }

        public DateTime CreationDate { get; set; }

        public string Recommendation { get; set; }

        public RenovationRecommendation() { }

        public RenovationRecommendation(Reservation reservation, SaveNewCreateReviewDTO saveNewCreateReviewDTO)
        {
            Reservation = reservation;
            Level = saveNewCreateReviewDTO.RecommendationLevel;
            CreationDate = DateTime.Now;
            Recommendation = saveNewCreateReviewDTO.RecommendationComment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Reservation.ReservationId.ToString(), Level.ToString(), CreationDate.ToString("dd.MM.yyyy"), Recommendation.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Reservation = new Reservation() { ReservationId = Convert.ToInt32(values[0]) };
            Level = values[1];

            string temporaryDate = values[2];
            if (!string.IsNullOrEmpty(temporaryDate))
            {
                CreationDate = DateTime.ParseExact(temporaryDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            Recommendation = values[3];
        }
    }
}
