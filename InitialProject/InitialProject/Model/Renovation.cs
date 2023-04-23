using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Renovation : ISerializable
    {
        public int RenovationId { get; set; }

        public int AccommodationId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public Renovation() { }

        public Renovation(int renovationId, int accommodationId, DateTime startDate, DateTime endDate, string description)
        {
            RenovationId = renovationId;
            AccommodationId = accommodationId;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { RenovationId.ToString(), AccommodationId.ToString(), StartDate.ToString("dd.MM.yyyy").ToString(), EndDate.ToString("dd.MM.yyyy").ToString(), Description.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            RenovationId = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);

            string temporaryDate = values[2];
            if (!string.IsNullOrEmpty(temporaryDate))
            {
                StartDate = DateTime.ParseExact(temporaryDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            temporaryDate = values[3];
            if (!string.IsNullOrEmpty(temporaryDate))
            {
                EndDate = DateTime.ParseExact(temporaryDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            Description = values[4];
        }
    }
}
