using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.IRepository;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    internal class VoucherService
    {
        private readonly IVoucherRepository voucherRepository;
        public VoucherService() 
        {
            voucherRepository = new VoucherRepository();
        }

        public void CreateForCancelledTourGuidence(int guidenceId)
        {
            TourReservationService tourReservationService = new TourReservationService();
            UserRepository userRepository = new UserRepository();
            List<Voucher> vouchers = voucherRepository.FindAll();

            foreach (Model.TourReservation reservation in tourReservationService.GetAll())
            {
                if (reservation.tourGuidenceId == guidenceId)
                {
                    User guest = userRepository.FindByUsername(reservation.userId);
                    Voucher v = new Voucher(voucherRepository.NextId(), guest, VoucherType.TOURCANCELATION, DateTime.Now.AddYears(1), false);
                    vouchers.Add(v);
                }
            }
            voucherRepository.Save(vouchers);
        }

        public List<double> GetVoucherPercentage(int tourId)
        {
            List<double> retVal = new List<double>(new double[2]);

            TourGuidenceService tourGuidanceService = new TourGuidenceService();
            TourReservationService tourReservationService = new TourReservationService();

            List<TourGuidence> tourGuidences = tourGuidanceService.GetAll();

            double withVoucher = 0, count = 0;

            foreach (TourGuidence tg in tourGuidences)
            {
                if (tg.Finished == true && tg.Tour.Id == tourId)
                {
                    foreach (Model.TourReservation tr in tourReservationService.GetAll())
                    {
                        if (tg.Id == tr.tourGuidenceId && tr.Confirmed == true)
                        {
                            if (tr.VoucherId != 0)
                            {
                                withVoucher++;
                            }
                            count++;
                        }
                    }
                }
            }
            retVal[0] = Math.Round((withVoucher / count) * 100, 2);
            retVal[1] = Math.Round((1 - (withVoucher / count)) * 100, 2);
            return retVal;
        }
    }
}
