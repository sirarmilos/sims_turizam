using InitialProject.Injector;
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
    internal class Guest2Service
    {
        private readonly IGuest2Repository guest2Repository;

        public Guest2Service()
        {
            guest2Repository = Injector.Injector.CreateInstance<IGuest2Repository>();
        }

        public int FindAge(string username)
        {
            foreach (Guest2 guest2 in guest2Repository.FindAll())
            {
                if (guest2.User.Username.Equals(username))
                {
                    return guest2.Age;
                }
            }
            return 0;
        }

        public Guest2 GetByUsername(string username)
        {
            return guest2Repository.GetByUsername(username);
        }

        public void UpdateVoucherUsedStatus(int voucherId)
        {
            guest2Repository.UpdateVoucherUsedStatus(voucherId);
        }


        public List<Voucher> GetGuestsVouchers(string username)
        {
            return guest2Repository.GetGuestsVouchers(username);
        }

        public void GuideRating(string userId, string guideId, int tourGuidenceId, int guideKnowledge, int guideLanguage, int tourExperience, string comment, List<string> images)
        {
            guest2Repository.GuideRating(userId, guideId, tourGuidenceId, guideKnowledge, guideLanguage, tourExperience, comment, images);
        }
    }
}