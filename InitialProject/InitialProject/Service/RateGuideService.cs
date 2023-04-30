using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class RateGuideService
    {
        private readonly IRateGuideRepository rateGuideRepository;

        private readonly TourReservationService tourReservationService;


        public RateGuideService()
        {
            rateGuideRepository = Injector.Injector.CreateInstance<IRateGuideRepository>();
            tourReservationService = new TourReservationService();
        }

        public List<Dto.RateGuideDisplayDto> FindForDisplay(string guide)
        {

            List<Dto.RateGuideDisplayDto> retVal = new List<Dto.RateGuideDisplayDto>();
            List<RateGuide> rates = rateGuideRepository.FindAll();

            foreach (RateGuide rate in rates)
            {
                if (rate.GuideId == guide)
                {
                    Dto.RateGuideDisplayDto rateGuide = new Dto.RateGuideDisplayDto();
                    TourReservation tourReservation = tourReservationService.FindByGuestAndGuidence(rate.UserId, rate.tourGuidenceId);
                    int counter = 0;
                    for (int i = 0; i < tourReservation.TourKeyPointArrival.Count; i++)
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
            rateGuideRepository.UpdateIsDeleted(user, id);
        }
    }
}
