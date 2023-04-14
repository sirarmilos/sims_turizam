using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class ReviewRepository
    {
        private ReservationRepository reservationRepository;

        private const string FilePathReview = "../../../Resources/Data/reviews.csv";

        private readonly Serializer<Review> reviewSerializer;

        private List<Review> reviews;

        public ReviewRepository()
        {
            reviewSerializer = new Serializer<Review>();
            reviews = reviewSerializer.FromCSV(FilePathReview);
        }

        public List<Review> FindAll()
        {
            reservationRepository = new ReservationRepository();

            reviews = reviewSerializer.FromCSV(FilePathReview);

            foreach (Review temporaryReview in reviews.ToList())
            {
                temporaryReview.Reservation = reservationRepository.FindReservationByReservationId(temporaryReview.Reservation.ReservationId);
            }

            return reviews;
        }

        public List<Review> FindAllReviews()
        {
            reservationRepository = new ReservationRepository();

            reviews = reviewSerializer.FromCSV(FilePathReview);

            foreach(Review temporaryReview in reviews.ToList())
            {
                temporaryReview.Reservation = reservationRepository.FindReservationByReservationId(temporaryReview.Reservation.ReservationId);
            }

            return reviews;
        }

        public void Save(List<Review> allReviews)
        {
            reviewSerializer.ToCSV(FilePathReview, allReviews);
        }

        public List<Review> FindReviewsByOwnerUsername(string ownerUsername)
        {
            return FindAllReviews().ToList().FindAll(x => x.Reservation.Accommodation.OwnerUsername.Equals(ownerUsername) == true);
        }

        public Review FindOwnerReviewByReservationId(string ownerUsername, int reservationId)
        {
            return FindReviewsByOwnerUsername(ownerUsername).ToList().Find(x => x.Reservation.ReservationId == reservationId);
        }

        public List<Review> FindGuest1Reviews(string guest1Username)
        {
            return reviews.FindAll(x => x.Reservation.GuestUsername.Equals(guest1Username) == true);
        }

        public void Save(Review review)
        {
            reviews = FindAllReviews();
            
            reviews.Add(review);    

            reviewSerializer.ToCSV(FilePathReview, reviews);
        }
    }
}
