using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class RateGuideRepository : IRateGuideRepository
    {
        private const string FilePathRateGuide = "../../../Resources/Data/guidereviews.csv";

        private readonly Serializer<RateGuide> rateGuideSerializer;

        private List<RateGuide> rates;


        public RateGuideRepository()
        {
            rateGuideSerializer = new Serializer<RateGuide>();
            rates = rateGuideSerializer.FromCSV(FilePathRateGuide);

        }

        public List<RateGuide> FindAll()
        {
            return rateGuideSerializer.FromCSV(FilePathRateGuide);
        }

        public void UpdateIsDeleted(string user, int id)
        {
            foreach(RateGuide rate in rates)
            {
                if(rate.UserId == user && rate.tourGuidenceId == id)
                {
                    rate.Valid = false;
                    break;
                }
            }
            rateGuideSerializer.ToCSV(FilePathRateGuide, rates);
        }


    }
}
