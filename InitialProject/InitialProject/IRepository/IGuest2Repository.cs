using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IGuest2Repository
    {
        int NextIdVoucher();

        // Guest2 GetByUsername(string username);

        void UpdateVoucherUsedStatus(int voucherId);

        List<Voucher> GetGuestsVouchers(string username);

        int GetAge(string username);

        void GuideRating(string userId, string guideId, int tourGuidenceId, int guideKnowledge, int guideLanguage, int tourExperience, string comment, List<string> images);
    }
}
