using InitialProject.IRepository;
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
        //private static Dictionary<Type, object> repositoryImplementations = new Dictionary<Type, object>
        //{
        //    { typeof(IAccommodationRepository), new AccommodationRepository() },
        //    { typeof(ICanceledRenovationRepository), new CanceledRenovationRepository() },
        //    { typeof(ICanceledReservationRepository), new CanceledReservationRepository() },
        //    { typeof(IGuest2Repository), new Guest2Repository() },
        //    { typeof(ILocationRepository), new LocationRepository() },
        //    { typeof(IRateGuestRepository), new RateGuestRepository() },
        //    { typeof(IRateGuideRepository), new RateGuideRepository() },
        //    { typeof(IRenovationRecommedationRepository), new RenovationRecommedationRepository() },
        //    { typeof(IRenovationRepository), new RenovationRepository() },
        //    { typeof(IReservationRepository), new ReservationRepository() },
        //    { typeof(IReservationReschedulingRequestRepository), new ReservationReschedulingRequestRepository() },
        //    { typeof(IReviewRepository), new ReviewRepository() },
        //    { typeof(ITourGuidenceRepository), new TourGuidenceRepository() },
        //    { typeof(ITourKeyPointRepository), new TourKeyPointRepository() },
        //    { typeof(ITourRepository), new TourRepository() },
        //    { typeof(ITourReservationRepository), new TourReservationRepository() },
        //    { typeof(IUserRepository), new UserRepository() },
        //    { typeof(IVoucherRepository), new VoucherRepository() },
        //};

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            Dictionary<Type, object> repositoryImplementations = new Dictionary<Type, object>
            {
                { typeof(IAccommodationRepository), new AccommodationRepository() },
                { typeof(ICanceledRenovationRepository), new CanceledRenovationRepository() },
                { typeof(ICanceledReservationRepository), new CanceledReservationRepository() },
                { typeof(IGuest2Repository), new Guest2Repository() },
                { typeof(ILocationRepository), new LocationRepository() },
                { typeof(IRateGuestRepository), new RateGuestRepository() },
                { typeof(IRateGuideRepository), new RateGuideRepository() },
                { typeof(IRenovationRecommendationRepository), new RenovationRecommendationRepository() },
                { typeof(IRenovationRepository), new RenovationRepository() },
                { typeof(IReservationRepository), new ReservationRepository() },
                { typeof(IReservationReschedulingRequestRepository), new ReservationReschedulingRequestRepository() },
                { typeof(IReviewRepository), new ReviewRepository() },
                { typeof(ISuperGuestRepository), new SuperGuestRepository() },
                { typeof(ITourGuidenceRepository), new TourGuidenceRepository() },
                { typeof(ITourKeyPointRepository), new TourKeyPointRepository() },
                { typeof(ITourRepository), new TourRepository() },
                { typeof(ITourReservationRepository), new TourReservationRepository() },
                { typeof(IUserRepository), new UserRepository() },
                { typeof(IVoucherRepository), new VoucherRepository() },
                {typeof(ITourRequestRepository), new TourRequestRepository() },
                { typeof(ITourNotificationsRepository), new TourNotificationsRepository() },
                { typeof(IGuideRepository), new GuideRepository() },
                { typeof(IComplexTourRepository), new ComplexTourRepository() },
            };


            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (repositoryImplementations.ContainsKey(type))
            {
                return (T)repositoryImplementations[type];
            }

            throw new ArgumentException($"The implementation does not exist. {type}");
        }
    }
}
