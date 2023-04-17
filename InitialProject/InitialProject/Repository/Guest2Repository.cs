using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    class Guest2Repository
    {
        private const string FilePathGuest2 = "../../../Resources/Data/guest2.csv";
        private const string FilePathVouchers = "../../../Resources/Data/vouchers.csv";
        private const string FilePathGuideReviews = "../../../Resources/Data/guidereviews.csv";

        private readonly Serializer<Guest2> guest2Serializer;
        private readonly Serializer<Voucher> voucherSerializer;
        private readonly Serializer<Model.RateGuide> rateGuideSerializer;

        private List<Guest2> guests2;
        private List<Voucher> vouchers;
        private List<Model.RateGuide> rateGuides;

        public Guest2Repository()
        {
            guest2Serializer = new Serializer<Guest2>();
            guests2 = guest2Serializer.FromCSV(FilePathGuest2);

            voucherSerializer = new Serializer<Voucher>();
            vouchers = voucherSerializer.FromCSV(FilePathVouchers);

            rateGuideSerializer = new Serializer<Model.RateGuide>();
            rateGuides = rateGuideSerializer.FromCSV(FilePathGuideReviews);
        }

        public int NextIdVoucher()
        {
            if(vouchers.Count<1)
            {
                return 1;
            }
            return vouchers.Max(c => c.Id) + 1;
        }


        public Guest2 GetByUsername(string username)
        {
            Guest2 guest = new Guest2();

            foreach(Guest2 guest2 in guests2)
            {
                if(guest2.User.Username.Equals(username))
                {
                    guest = guest2;
                    break;
                }
            }

            return guest;
        }


        public void UpdateVoucherUsedStatus(int voucherId)
        {
            List<Voucher> result = vouchers;

            foreach(Voucher voucher in vouchers)
            {
                if(voucher.Id==voucherId)
                {
                    voucher.IsUsed = true;
                    voucherSerializer.ToCSV(FilePathVouchers,result);
                }
            }
        }


        public List<Voucher> GetGuestsVouchers(string username)
        {
            List<Voucher> result = new List<Voucher> ();

            foreach(Voucher voucher in vouchers)
            {
                if(voucher.user.Username.Equals(username))
                {
                    if(!voucher.IsUsed)
                        result.Add(voucher);
                }
            }

            return result;

        }

        public int GetAge(string username)
        {
            foreach(Guest2 guest2 in guests2)
            {
                if (guest2.User.Username.Equals(username))
                {
                    return guest2.Age;
                }
            }
            return 0;
        }

        public void GuideRating(string userId, string guideId, int tourGuidenceId, int guideKnowledge, int guideLanguage, int tourExperience, string comment, List<string> images)
        {
            Model.RateGuide rateGuide = new Model.RateGuide(userId,guideId,tourGuidenceId,guideKnowledge,guideLanguage,tourExperience,comment,images);
            rateGuides.Add(rateGuide);
            rateGuideSerializer.ToCSV(FilePathGuideReviews,rateGuides);
        
        }

    }
}
