using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class RenovationRecommendation : ISerializable
    {
        public Reservation Reservation { get; set; }

        public string Level { get; set; }

        public string Recommendation { get; set; }

        public RenovationRecommendation() { }

        public RenovationRecommendation(Reservation reservation, string level, string recommendation)
        {
            Reservation = reservation;
            Level = level;
            Recommendation = recommendation;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Reservation.ReservationId.ToString(), Level.ToString(), Recommendation.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Reservation = new Reservation() { ReservationId = Convert.ToInt32(values[0]) };
            Level = values[1];
            Recommendation = values[2];
        }
    }
}
