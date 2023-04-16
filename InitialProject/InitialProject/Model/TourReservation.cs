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
        public List<Boolean> TourKeyPointArrival { get; set; }
        public int numberOfGuests { get; set; }
        public Boolean Confirmed { get; set; }
        public int VoucherId { get; set; }

        public int Id { get; set; }

        public TourReservation() { }

        public TourReservation(string userId, int reservatedTourId, List<Boolean> arrivalTourKeyPoint, int numberOfGuests, Boolean confirmed, int voucherId)
        {
            this.userId = userId;
            this.tourGuidenceId = reservatedTourId;
            TourKeyPointArrival = arrivalTourKeyPoint;
            this.numberOfGuests = numberOfGuests;
            Confirmed = confirmed;
            VoucherId = voucherId;
        }

        public TourReservation(string userId, int reservatedTourId, List<Boolean> arrivalTourKeyPoint, int numberOfGuests, Boolean confirmed, int voucherId, int id)
        {
            this.userId = userId;
            this.tourGuidenceId = reservatedTourId;
            TourKeyPointArrival = arrivalTourKeyPoint;
            this.numberOfGuests = numberOfGuests;
            Confirmed = confirmed;
            VoucherId = voucherId;
            Id = id;
        }

        public string[] ToCSV()
        {
            string boolToString = "";

            foreach (Boolean kp in TourKeyPointArrival)
            {
                boolToString += kp;
                boolToString += ", ";
            }

            boolToString = boolToString.Substring(0, boolToString.Length - 2);


            string[] csvValues = { userId.ToString(), tourGuidenceId.ToString(), boolToString.ToString(), numberOfGuests.ToString(), Confirmed.ToString(), VoucherId.ToString(), Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            userId = values[0];

            tourGuidenceId = Convert.ToInt32(values[1]);

            string[] BoolSplit = values[2].Split(',');

            List<Boolean> arrivals = new List<Boolean>();

            foreach (string b in BoolSplit)
            {
                arrivals.Add(Convert.ToBoolean(b));
            }

            TourKeyPointArrival = arrivals;

            //TourKeyPointArrivalId = Convert.ToInt32(values[2]); 

            numberOfGuests = Convert.ToInt32(values[3]);

            Confirmed = Convert.ToBoolean(values[4]);

            VoucherId = Convert.ToInt32(values[5]);

            Id = Convert.ToInt32(values[6]);
        }



    }
}
