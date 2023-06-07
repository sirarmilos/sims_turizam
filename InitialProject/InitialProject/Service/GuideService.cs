using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class GuideService
    {
        private readonly IGuideRepository guideRepository;

        public GuideService()
        {
            guideRepository = Injector.Injector.CreateInstance<IGuideRepository>();
        }

        public bool Resign(string guideUsername)
        {

            List<Guide> guides = guideRepository.FindAll();
            
            Guide guide = guides.Find(x => x.User.Username == guideUsername);
            if(guide.IsResigned == false)
            {
                guide.IsResigned = true;
                guideRepository.Save(guides);
                return true;
            }

            return false;
        }

        public void LoadGuideType(string guideUsername)
        {
            //string type = "super_";

            TourGuidenceService tourGuidenceService = new TourGuidenceService();
            UserService userService = new UserService();
            List<TourGuidence> tourGuidences = tourGuidenceService.FindGuideAllInLastYear(guideUsername);
            RateGuideRepository rateGuideRepository = new RateGuideRepository();
            List<RateGuide> rates = rateGuideRepository.FindAll();

            var jezik = tourGuidences.GroupBy(t => t.Tour.Language).OrderByDescending(g => g.Count()).FirstOrDefault();

            if (jezik != null)
            {
                Language language = jezik.Key;
                int count = jezik.Count();
                float counter = 0;
                float sum = 0;
                float avg = 0;

                foreach (var tg in jezik)
                {
                    foreach (var rate in rates)
                    {
                        if (tg.Id == rate.tourGuidenceId)
                        {
                            counter++;
                            sum += rate.GuideLanguage;
                        }
                    }
                }

                avg = sum / counter;

                if (count >= 20 && avg >= 4)
                {
                    //type += language.ToString();
                    userService.MakeUserSuperGuest(guideUsername);
                }
                else
                {
                    //type = "no_super";
                    userService.MakeUserRegularUser(guideUsername);
                }

                //userService.Update(guideUsername, type);
            }




        }



    }
}
