using InitialProject.IRepository;
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
    public class ReviewRepository : IReviewRepository
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
                temporaryReview.Reservation = reservationRepository.FindById(temporaryReview.Reservation.ReservationId);
            }

            return reviews;
        }

        public void Save(List<Review> allReviews)
        {
            reviewSerializer.ToCSV(FilePathReview, allReviews);
        }

        public List<Review> FindByOwnerUsername(string ownerUsername)
        {
            return FindAll().ToList().FindAll(x => x.Reservation.Accommodation.OwnerUsername.Equals(ownerUsername) == true);
        }

        public Review FindOwnerReviewByReservationId(string ownerUsername, int reservationId)
        {
            return FindByOwnerUsername(ownerUsername).ToList().Find(x => x.Reservation.ReservationId == reservationId);
        }

        public List<Review> FindReviewsByGuest1Username(string guest1Username)
        {
            return FindAll().ToList().FindAll(x => x.Reservation.GuestUsername.Equals(guest1Username) == true);
        }

        public Review FindGuest1ReviewByReservationId(string guest1Username, int reservationId)
        {
            return FindReviewsByGuest1Username(guest1Username).ToList().Find(x => x.Reservation.ReservationId == reservationId);
        }

        public void Add(Review review)
        {
            reviews = FindAll();
            reviews.Add(review);
            Save(reviews);
        }
    }
}
