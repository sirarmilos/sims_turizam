using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.Repository
{
    internal class VoucherRepository : IVoucherRepository
    {
        private const string FilePathVoucher = "../../../Resources/Data/vouchers.csv";

        private readonly Serializer<Voucher> voucherSerializer;

        private List<Voucher> vouchers;



        public VoucherRepository()
        {

            voucherSerializer = new Serializer<Voucher>();
            //vouchers = voucherSerializer.FromCSV(FilePathVoucher);

        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }
            return FindAll().Max(c => c.Id) + 1;
        }

        public void Save(List<Voucher> vouchers)
        {
            voucherSerializer.ToCSV(FilePathVoucher,vouchers);
        }

        public List<Voucher> FindAll()
        {
            UserRepository userRepository = new UserRepository();

            vouchers = voucherSerializer.FromCSV(FilePathVoucher);

            foreach (Voucher temporaryVouchers in vouchers.ToList())
            {
                temporaryVouchers.user = userRepository.FindByUsername(temporaryVouchers.user.Username);
            }

            return vouchers;
        }
    }
}
