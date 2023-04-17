using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class RateGuideRepository
    {
        private const string FilePathRateGuide = "../../../Resources/Data/guidereviews.csv";

        private readonly Serializer<RateGuide> rateGuideSerializer;

        private List<RateGuide> rates;

        public RateGuideRepository()
        {
            rateGuideSerializer = new Serializer<RateGuide>();
            rates = rateGuideSerializer.FromCSV(FilePathRateGuide);
        }

        public List<Dto.RateGuideDisplayDto> GetForDisplay(string guide)
        {

            List<Dto.RateGuideDisplayDto> retVal = new List<Dto.RateGuideDisplayDto>();
            TourReservationRepository tourReservationRepository = new TourReservationRepository();  

            foreach(RateGuide rate in rates)
            {
                if(rate.GuideId == guide)
                {
                    Dto.RateGuideDisplayDto rateGuide = new Dto.RateGuideDisplayDto();
                    TourReservation tourReservation = tourReservationRepository.FindByGuestAndGuidence(rate.UserId, rate.tourGuidenceId);
                    int counter = 0;
                    for(int i=0; i< tourReservation.TourKeyPointArrival.Count; i++)
                    {
                        if (tourReservation.TourKeyPointArrival[i] == true)
                        {
                            counter = i + 1;
                            break;
                        }
                            
                    }
                    rateGuide.UserId = rate.UserId;
                    rateGuide.tourGuidenceId = rate.tourGuidenceId;
                    rateGuide.GuideKnowledge = rate.GuideKnowledge;
                    rateGuide.GuideLanguage = rate.GuideLanguage;
                    rateGuide.TourExperience = rate.TourExperience;
                    rateGuide.Comment = rate.Comment;
                    rateGuide.Valid = rate.Valid;
                    rateGuide.OrderNumberKeyPoint = counter;

                    retVal.Add(rateGuide);
                }
            }
            return retVal;

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
