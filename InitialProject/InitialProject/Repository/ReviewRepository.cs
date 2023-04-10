using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class ReviewRepository
    {
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
            return reviews;
        }

        public List<Review> FindAllReviews()
        {
            return reviews;
        }

        public void Save(List<Review> allReviews)
        {
            reviewSerializer.ToCSV(FilePathReview, allReviews);
        }

    }
}
