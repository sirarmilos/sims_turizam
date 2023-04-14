using InitialProject.Model;
using InitialProject.Serializer;
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

        private readonly Serializer<Guest2> guest2Serializer;
        private readonly Serializer<Voucher> voucherSerializer;

        private List<Guest2> guests2;
        private List<Voucher> vouchers;

        public Guest2Repository()
        {
            guest2Serializer = new Serializer<Guest2>();
            guests2 = guest2Serializer.FromCSV(FilePathGuest2);

            voucherSerializer = new Serializer<Voucher>();
            vouchers = voucherSerializer.FromCSV(FilePathVouchers);
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

    }
}
