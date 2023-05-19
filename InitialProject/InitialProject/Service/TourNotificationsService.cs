using InitialProject.IRepository;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourNotificationsService
    {
        private readonly ITourNotificationsRepository tourNotificationsRepository;

        public TourNotificationsService()
        {
            tourNotificationsRepository = Injector.Injector.CreateInstance<ITourNotificationsRepository>();
        }

        public void Update()
        {
            tourNotificationsRepository.Update();
        }

        public List<TourNotifications> GetForDisplay(string username)
        {
            List<TourNotifications> result = new List<TourNotifications>();

            foreach(TourNotifications tourNotification in tourNotificationsRepository.FindAll())
            {
                if(tourNotification.IsNotified==false && tourNotification.User.Username==username)
                {
                    result.Add(tourNotification);
                    tourNotificationsRepository.UpdateIsNotified(tourNotification);
                }
            }

            return result;
        }

        public List<TourNotifications> NotifyOfNewTour(string username)
        {
            TourRequestService tourRequestService = new TourRequestService();
            List<TourRequest> tourRequests = tourRequestService.GetRejectedByUser(username);
         

            List<TourNotifications> result = new List<TourNotifications>();

            foreach(TourNotifications tourNotification in GetForDisplay(username))
            {
                foreach(TourRequest tourRequest in tourRequests)
                {
                    if (CheckSameLanguage(tourNotification, tourRequest) || CheckSameLocation(tourNotification, tourRequest))
                    {
                        Add(result, tourNotification);
                    }
                }
            }


            return result.Distinct().ToList();
        }

        public void Add(List<TourNotifications> result, TourNotifications tourNotification)
        {
            result.Add(tourNotification);
        }

        public bool CheckSameLocation(TourNotifications tourNotification, TourRequest tourRequest)
        {
            return (tourRequest.Location.Country.Equals(tourNotification.TourGuidence.Tour.Location.Country) && tourRequest.Location.City.Equals(tourNotification.TourGuidence.Tour.Location.City));
        }

        public bool CheckSameLanguage(TourNotifications tourNotification, TourRequest tourRequest)
        {
            return tourRequest.Language.Equals(tourNotification.TourGuidence.Tour.Language);
        }

    }
}
