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


        public void AddNotifyOfNewTour(string username)
        {
            TourRequestService tourRequestService = new TourRequestService();
            List<TourRequest> tourRequests = tourRequestService.GetRejectedByUser(username);

            TourGuidenceService tourGuidenceService = new TourGuidenceService();
            List<TourGuidence> tourGuidences = tourGuidenceService.FindAll();

            List<TourNotifications> result = new List<TourNotifications>();

            foreach(TourGuidence tourGuidence in tourGuidences)
            {
                foreach(TourRequest tourRequest in tourRequests)
                {
                    if (CheckSameLanguage(tourGuidence,tourRequest) || CheckSameLocation(tourGuidence, tourRequest))
                    {
                        UserService userService = new UserService();
                        User user = userService.FindByUsername(username);

                        TourNotifications tourNotifications = new TourNotifications();
                        tourNotifications.TourGuidence = tourGuidence;
                        tourNotifications.Type = "newTourNotification";
                        tourNotifications.IsNotified = false;
                        tourNotifications.User = user;

                        Add(tourNotifications);
                    }
                }
            }

        }

        public List<TourNotifications> NotifyOfNewTours(string username)
        {
            List<TourNotifications> result = new List<TourNotifications>();

            foreach(TourNotifications tourNotifications in tourNotificationsRepository.FindAll())
            {
                if(tourNotifications.Type.Equals("newTourNotification") && tourNotifications.User.Username.Equals(username))
                {
                    result.Add(tourNotifications);
                }
            }

            return result;
        }

        public void Add(List<TourNotifications> result, TourNotifications tourNotification)
        {
            result.Add(tourNotification);
        }

        public bool CheckSameLocation(TourGuidence tourGuidence, TourRequest tourRequest)
        {
            return (tourRequest.Location.Country.Equals(tourGuidence.Tour.Location.Country) && tourRequest.Location.City.Equals(tourGuidence.Tour.Location.City));
        }

        public bool CheckSameLanguage(TourGuidence tourGuidence, TourRequest tourRequest)
        {
            return tourRequest.Language.Equals(tourGuidence.Tour.Language);
        }

        public void Add(TourNotifications tour)
        {
            List<TourNotifications> tourNotifications = tourNotificationsRepository.FindAll();
            tourNotifications.Add(tour);

            tourNotificationsRepository.Save(tourNotifications);
        }

        public List<TourNotifications> GetAcceptedTourRequests(string username)
        {
            TourRequestService tourRequestService = new TourRequestService();
            List<TourRequest> tourRequests = tourRequestService.GetRejectedByUser(username);


            List<TourNotifications> result = new List<TourNotifications>();

            foreach (TourNotifications tourNotification in GetForDisplay(username))
            {
                if(tourNotification.Type.Equals("acceptedRequest"))
                {
                    result.Add(tourNotification);
                }
            }

            return result.Distinct().ToList();
        }

    }
}
