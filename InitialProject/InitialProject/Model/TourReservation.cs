using System;
using System.Collections.Generic;
using System.Linq;
using InitialProject.Serializer;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Repository;
using System.Windows.Input;

namespace InitialProject.Model
{
    public class TourReservation : ISerializable
    {
        private string userId { get; set; }
        private int reservatedTourId { get; set; }
        private int numberOfGuests { get; set; }

        public TourReservation() { }

        public TourReservation(string userId, int reservatedTourId, int numberOfGuests)
        {
            this.userId = userId;
            this.reservatedTourId = reservatedTourId;
            this.numberOfGuests = numberOfGuests;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { userId.ToString(), reservatedTourId.ToString(), numberOfGuests.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            userId = values[0];

            reservatedTourId = Convert.ToInt32(values[1]);

            numberOfGuests = Convert.ToInt32(values[2]);
        }



    }
}
