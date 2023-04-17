using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class ShowGuestReviewsDTO
    {
        public string AccommodationName { get; set; }

        public string GuestUsername { get; set; }

        public int Cleanliness { get; set; }

        public int Staff { get; set; }

        public int Comfort { get; set; }

        public int ValueForMoney { get; set; }

        public string Comment { get; set; }

        public ShowGuestReviewsDTO()
        {

        }

        public ShowGuestReviewsDTO(Review review)
        {
            AccommodationName = review.Reservation.Accommodation.AccommodationName;
            GuestUsername = review.Reservation.GuestUsername;
            Cleanliness = review.Cleanliness;
            Staff = review.Staff;
            Comfort = review.Comfort;
            ValueForMoney = review.ValueForMoney;
            Comment = review.Comment;
        }
    }
}
