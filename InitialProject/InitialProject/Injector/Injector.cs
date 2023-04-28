﻿using InitialProject.IRepository;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Injector
{
    public class Injector
    {
        private static Dictionary<Type, object> repositoryImplementations = new Dictionary<Type, object>
        {
            { typeof(IAccommodationRepository), new AccommodationRepository() },
            { typeof(ICanceledRenovationRepository), new CanceledRenovationRepository() },
            { typeof(ICanceledReservationRepository), new CanceledReservationRepository() },
            { typeof(IGuest2Repository), new Guest2Repository() },
            { typeof(ILocationRepository), new LocationRepository() },
            { typeof(IRateGuestRepository), new RateGuestRepository() },
            { typeof(IRateGuideRepository), new RateGuideRepository() },
            { typeof(IRenovationRecommedationRepository), new RenovationRecommedationRepository() },
            { typeof(IRenovationRepository), new RenovationRepository() },
            { typeof(IReservationRepository), new ReservationRepository() },
            { typeof(IReservationReschedulingRequestRepository), new ReservationReschedulingRequestRepository() },
            { typeof(IReviewRepository), new ReviewRepository() },
            { typeof(ITourGuidenceRepository), new TourGuidenceRepository() },
            { typeof(ITourKeyPointRepository), new TourKeyPointRepository() },
            { typeof(ITourRepository), new TourRepository() },
            { typeof(ITourReservationRepository), new TourReservationRepository() },
            { typeof(IUserRepository), new UserRepository() },
            { typeof(IVoucherRepository), new VoucherRepository() },
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (repositoryImplementations.ContainsKey(type))
            {
                return (T)repositoryImplementations[type];
            }

            throw new ArgumentException($"The implementation does not exist. {type}");
        }
    }
}
