using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class ShowOwnerReviewsDTO
    {
        public string AccommodationName { get; set; }

        public string OwnerUsername { get; set; }

        public int Cleanliness { get; set; }

        public int FollowRules { get; set; }

        public int Behavior { get; set; }

        public string TypePayment { get; set; }

        public int Communicativeness { get; set; }

        public string Comment { get; set; }

        public ShowOwnerReviewsDTO()
        {

        }

        public ShowOwnerReviewsDTO(RateGuest rateGuest)
        {
            AccommodationName = rateGuest.Reservation.Accommodation.AccommodationName;
            OwnerUsername = rateGuest.Reservation.Accommodation.OwnerUsername;
            Cleanliness = rateGuest.Cleanliness;
            FollowRules = rateGuest.FollowRules;
            Behavior = rateGuest.Behavior;
            TypePayment = rateGuest.TypePayment;
            Communicativeness = rateGuest.Communicativeness;
            Comment = rateGuest.Comment;
        }
    }
}
