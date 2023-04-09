using InitialProject.DTO;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class RateGuest : ISerializable
    {
        public Reservation Reservation { get; set; }

        public int Cleanliness { get; set; }

        public int FollowRules { get; set; }

        public int Behavior { get; set; }

        public string TypePayment { get; set; }

        public int Communicativeness { get; set; }

        public string Comment { get; set; }

        public RateGuest()
        {

        }

        public RateGuest(Reservation reservation, int cleanliness, int followRules, int behavior, string typePayment, int communicativeness, string comment)
        {
            Reservation = reservation;
            Cleanliness = cleanliness;
            FollowRules = followRules;
            Behavior = behavior;
            TypePayment = typePayment;
            Communicativeness = communicativeness;
            Comment = comment;
        }

        public RateGuest(Reservation reservation, SaveNewRateGuestDTO saveNewRateGuestDTO)
        {
            Reservation = reservation;
            Cleanliness = saveNewRateGuestDTO.Cleanliness;
            FollowRules = saveNewRateGuestDTO.FollowRules;
            Behavior = saveNewRateGuestDTO.Behavior;
            TypePayment = saveNewRateGuestDTO.TypePayment;
            Communicativeness = saveNewRateGuestDTO.Communicativeness;
            Comment = saveNewRateGuestDTO.Comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Reservation.ReservationId.ToString(), Cleanliness.ToString(), FollowRules.ToString(), Behavior.ToString(), TypePayment, Communicativeness.ToString(), Comment };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (string.IsNullOrWhiteSpace(values[0])) return;

            Reservation = new Reservation() { ReservationId = Convert.ToInt32(values[0]) };
            Cleanliness = Convert.ToInt32(values[1]);
            FollowRules = Convert.ToInt32(values[2]);
            Behavior = Convert.ToInt32(values[3]);
            TypePayment = values[4];
            Communicativeness = Convert.ToInt32(values[5]);
            Comment = values[6];
        }
    }
}
