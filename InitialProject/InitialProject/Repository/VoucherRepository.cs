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
    internal class VoucherRepository
    {
        private const string FilePathVoucher = "../../../Resources/Data/vouchers.csv";

        private const string FilePathReservatedTours = "../../../Resources/Data/reservatedtours.csv";

        private readonly Serializer<Voucher> voucherSerializer;

        private readonly Serializer<TourReservation> tourReservationSerializer;

        private List<Voucher> vouchers;

        private List<TourReservation> tourReservations;


        public VoucherRepository()
        {

            voucherSerializer = new Serializer<Voucher>();
            vouchers = voucherSerializer.FromCSV(FilePathVoucher);

            tourReservationSerializer = new Serializer<TourReservation>();
            tourReservations = tourReservationSerializer.FromCSV(FilePathReservatedTours);
        }

        public void CreateForCancelledTourGuidence(int guidenceId)
        {
            UserRepository userRepository = new UserRepository();

            foreach(TourReservation reservation in tourReservations)
            {
                if(reservation.tourGuidenceId == guidenceId)
                {
                    User guest = userRepository.GetByUsername(reservation.userId);
                    Voucher v = new Voucher(NextId(), guest, VoucherType.TOURCANCELATION, DateTime.Now.AddYears(1), false);
                    vouchers.Add(v); 
                }
            }
            voucherSerializer.ToCSV(FilePathVoucher, vouchers);
        }

        public int NextId()
        {
            vouchers = voucherSerializer.FromCSV(FilePathVoucher);
            if (vouchers.Count < 1)
            {
                return 1;
            }
            return vouchers.Max(c => c.Id) + 1;
        }
    }
}
