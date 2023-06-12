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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InitialProject.Service
{
    internal class VoucherService
    {
        private readonly IVoucherRepository voucherRepository;
        public VoucherService() 
        {
            voucherRepository = Injector.Injector.CreateInstance<IVoucherRepository>();

            //voucherRepository = new VoucherRepository();
        }

        public void CreateForCancelledTourGuidence(int guidenceId)
        {
            TourReservationService tourReservationService = new TourReservationService();
            UserRepository userRepository = new UserRepository();
            List<Voucher> vouchers = voucherRepository.FindAll();
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();
            string guide = tourGuidenceRepository.FindGuide(guidenceId);
            

            foreach (Model.TourReservation reservation in tourReservationService.FindAll())
            {
                if (reservation.tourGuidenceId == guidenceId)
                {
                    User guest = userRepository.FindByUsername(reservation.userId);
                    Voucher v = new Voucher(voucherRepository.NextId(), guest, VoucherType.TOURCANCELATION, DateTime.Now.AddYears(1), false, guide);
                    vouchers.Add(v);
                    voucherRepository.Save(vouchers);
                }
            }
            
        }

        public double GetWithVoucherPercentage(int tourId)
        {
            List<double> retVal = new List<double>(new double[2]);

            TourGuidenceService tourGuidanceService = new TourGuidenceService();
            TourReservationService tourReservationService = new TourReservationService();

            List<TourGuidence> tourGuidences = tourGuidanceService.FindAll();

            double withVoucher = 0, count = 0;

            foreach (TourGuidence tg in tourGuidences)
            {
                if (tg.Finished == true && tg.Tour.Id == tourId)
                {
                    foreach (Model.TourReservation tr in tourReservationService.FindAll())
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

            if (count != 0)
            {
                retVal[0] = Math.Round((withVoucher / count) * 100, 2);
                retVal[1] = Math.Round((1 - (withVoucher / count)) * 100, 2);
            } 
            return retVal[0];
        
        }

        public double GetWithoutVoucherPercentage(int tourId)
        {
            List<double> retVal = new List<double>(new double[2]);

            TourGuidenceService tourGuidanceService = new TourGuidenceService();
            TourReservationService tourReservationService = new TourReservationService();

            List<TourGuidence> tourGuidences = tourGuidanceService.FindAll();

            double withVoucher = 0, count = 0;

            foreach (TourGuidence tg in tourGuidences)
            {
                if (tg.Finished == true && tg.Tour.Id == tourId)
                {
                    foreach (Model.TourReservation tr in tourReservationService.FindAll())
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

            if (count != 0)
            {
                retVal[0] = Math.Round((withVoucher / count) * 100, 2);
                retVal[1] = Math.Round((1 - (withVoucher / count)) * 100, 2);
            }
            return retVal[1];

        }

        public bool CheckForNewVoucher(string username)
        {
            TourReservationService tourReservationService = new TourReservationService();
            TourGuidenceService tourGuidenceService = new TourGuidenceService();

            int count = 0;

            foreach (TourReservation tourReservation in tourReservationService.FindAll())
            {
                if (tourReservation.userId.Equals(username))
                {
                    foreach (TourGuidence tourGuidence in tourGuidenceService.FindAll())
                    {
                        if (tourGuidence.StartTime >= DateTime.Now.AddYears(-1) && tourGuidence.StartTime <= DateTime.Now && tourReservation.Confirmed == true && tourReservation.IsUsedForVoucher == false && tourGuidence.Id == tourReservation.Id)
                        {
                            count++;
                        }
                    }
                }
            }

            if (count >= 5)
                return true;

            return false;
        }


        public void AddNewVoucher(string username)
        {
            TourReservationService tourReservationService = new TourReservationService();
            TourGuidenceService tourGuidenceService = new TourGuidenceService();

            int count = 0;

            foreach(TourReservation tourReservation in tourReservationService.FindAll())
            {
                if (tourReservation.userId.Equals(username))
                {
                    foreach (TourGuidence tourGuidence in tourGuidenceService.FindAll())
                    {
                        if (tourGuidence.StartTime >= DateTime.Now.AddYears(-1) && tourGuidence.StartTime <= DateTime.Now && tourReservation.Confirmed==true && tourReservation.IsUsedForVoucher==false && tourGuidence.Id==tourReservation.tourGuidenceId)
                        {
                            count++;
                        }
                    }
                }
            }

            if(count>=5)
            {
                int checkCount = 5;
                foreach (TourReservation tourReservation in tourReservationService.FindAll())
                {
                    if (tourReservation.userId.Equals(username))
                    {
                        foreach (TourGuidence tourGuidence in tourGuidenceService.FindAll())
                        {
                            if (tourGuidence.StartTime >= DateTime.Now.AddYears(-1) && tourGuidence.StartTime <= DateTime.Now && tourReservation.Confirmed == true && tourReservation.IsUsedForVoucher == false && tourGuidence.Id == tourReservation.tourGuidenceId)
                            {
                                tourReservationService.UpdateIsUsedForVoucher(tourReservation.Id);
                                checkCount--;

                                if(checkCount==0)
                                {
                                    break;
                                }
                            }
                        }
                        if (checkCount == 0)
                        {
                            break;
                        }
                    }
                }

                List<Voucher> vouchers = voucherRepository.FindAll();
                UserService userService = new UserService();
                Voucher voucher = new Voucher();
                voucher.Id = voucherRepository.NextId();
                voucher.user = userService.FindByUsername(username);
                voucher.voucherType = VoucherType.WONVOUCHER;
                voucher.expirationDate = DateTime.Now.AddMonths(6);

                vouchers.Add(voucher);

                voucherRepository.Save(vouchers);
            }

        }

        public void CreateForGuideResignation(int guidenceId, string guideUsername)
        {
            TourReservationService tourReservationService = new TourReservationService();
            UserRepository userRepository = new UserRepository();
            List<Voucher> vouchers = voucherRepository.FindAll();

            foreach(Voucher v in vouchers)
            {
                if (v.Guide.Equals(guideUsername))
                    v.Guide = "ALL";
            }

            foreach (Model.TourReservation reservation in tourReservationService.FindAll())
            {
                if (reservation.tourGuidenceId == guidenceId)
                {
                    User guest = userRepository.FindByUsername(reservation.userId);
                    Voucher v = new Voucher(voucherRepository.NextId(), guest, VoucherType.GUIDERESIGNATION, DateTime.Now.AddYears(2), false, "ALL");
                    vouchers.Add(v);
                    voucherRepository.Save(vouchers);
                }
            }

        }
    }
}
