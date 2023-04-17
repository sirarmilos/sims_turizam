using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IReviewRepository
    {
        List<Review> FindAll();

        void Save(List<Review> allReviews);

        List<Review> FindByOwnerUsername(string ownerUsername);

        Review FindOwnerReviewByReservationId(string ownerUsername, int reservationId);

        List<Review> FindReviewsByGuest1Username(string guest1Username);

        Review FindGuest1ReviewByReservationId(string guest1Username, int reservationId);

        void Add(Review review);
    }
}
