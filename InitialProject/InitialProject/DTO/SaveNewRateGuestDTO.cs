using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class SaveNewRateGuestDTO
    {
        public int ReservationId { get; set; }

        public int Cleanliness { get; set; }

        public int FollowRules { get; set; }

        public int Behavior { get; set; }

        public string TypePayment { get; set; }

        public int Communicativeness { get; set; }

        public string Comment { get; set; }

        public SaveNewRateGuestDTO()
        {

        }

        public SaveNewRateGuestDTO(int reservationId, int cleanliness, int followRules, int behavior, string typePayment, int communicativeness, string comment)
        {
            ReservationId = reservationId;
            Cleanliness = cleanliness;
            FollowRules = followRules;
            Behavior = behavior;
            TypePayment = typePayment;
            Communicativeness = communicativeness;
            Comment = comment;
        }
    }
}
