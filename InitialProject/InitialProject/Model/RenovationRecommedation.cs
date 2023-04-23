using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class RenovationRecommedation : ISerializable
    {
        public int ReservationId { get; set; }

        public string Level { get; set; }

        public string Recommendation { get; set; }

        public RenovationRecommedation() { }

        public RenovationRecommedation(int reservationId, string level, string recommendation)
        {
            ReservationId = reservationId;
            Level = level;
            Recommendation = recommendation;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { ReservationId.ToString(), Level.ToString(), Recommendation.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            ReservationId = Convert.ToInt32(values[0]);
            Level = values[1];
            Recommendation = values[2];
        }
    }
}
