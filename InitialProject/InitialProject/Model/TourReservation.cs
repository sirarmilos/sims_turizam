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
        public string userId { get; set; }
        public int tourGuidenceId { get; set; }
        public int TourKeyPointArrivalId { get; set; }
        public int numberOfGuests { get; set; }
        public Boolean Confirmed { get; set; }

        public TourReservation() { }

        public TourReservation(string userId, int reservatedTourId, int arrivalTourKeyPointId, int numberOfGuests, Boolean confirmed)
        {
            this.userId = userId;
            this.tourGuidenceId = reservatedTourId;
            TourKeyPointArrivalId = arrivalTourKeyPointId;
            this.numberOfGuests = numberOfGuests;
            Confirmed = confirmed;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { userId.ToString(), tourGuidenceId.ToString(), TourKeyPointArrivalId.ToString(), numberOfGuests.ToString(), Confirmed.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            userId = values[0];

            tourGuidenceId = Convert.ToInt32(values[1]);

            TourKeyPointArrivalId = Convert.ToInt32(values[2]); 

            numberOfGuests = Convert.ToInt32(values[3]);

            Confirmed = Convert.ToBoolean(values[4]);
        }



    }
}
