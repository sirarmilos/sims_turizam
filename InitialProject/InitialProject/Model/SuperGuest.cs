using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class SuperGuest : ISerializable
    {
        public string Guest1Username { get; set; }

        public DateTime StartDate { get; set; }

        public int NumberOfBonusPoints { get; set; }

        public SuperGuest() { }

        public SuperGuest(string guest1Username, DateTime startDate, int numberOfBonusPoints)
        {
            Guest1Username = guest1Username;
            StartDate = startDate;
            NumberOfBonusPoints = numberOfBonusPoints;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Guest1Username.ToString(), StartDate.ToString("dd.MM.yyyy").ToString(), NumberOfBonusPoints.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Guest1Username = values[0];

            string temporaryDate = values[1];
            if (!string.IsNullOrEmpty(temporaryDate))
            {
                StartDate = DateTime.ParseExact(temporaryDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            NumberOfBonusPoints = Convert.ToInt32(values[2]);
        }
    }
}
