using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class TourNotificationsRepository:ITourNotificationsRepository
    {
        private const string FilePathTourNotifications = "../../../Resources/Data/tournotifications.csv";

        private readonly Serializer<TourNotifications> tourNotificationsSerializer;

        private List<TourNotifications> tourNotifications;


        public TourNotificationsRepository()
        {
            tourNotificationsSerializer = new Serializer<TourNotifications>();
        }

        public List<TourNotifications> FindAll()
        {
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();
            UserRepository userRepository = new UserRepository();

            tourNotifications = tourNotificationsSerializer.FromCSV(FilePathTourNotifications);

            foreach(TourNotifications tourNotification in tourNotifications)
            {
                tourNotification.TourGuidence = tourGuidenceRepository.FindById(tourNotification.TourGuidence.Id);
                tourNotification.User = userRepository.FindByUsername(tourNotification.User.Username);
            }

            return tourNotifications;
        }

        public void Save(List<TourNotifications> tourNotifications)
        {
            tourNotificationsSerializer.ToCSV(FilePathTourNotifications,tourNotifications);
        }

        public void Save(TourNotifications tourNotification)
        {
            List<TourNotifications> tourNotifications = FindAll();
            tourNotifications.Add(tourNotification);
            tourNotificationsSerializer.ToCSV(FilePathTourNotifications, tourNotifications);

        }

        public void Update()
        {
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();
            UserRepository userRepository = new UserRepository();

            List<TourNotifications> tourNotifications = FindAll();
            List<TourGuidence> tours = tourGuidenceRepository.FindAll();

            List<TourNotifications> result = FindAll();

            foreach (TourGuidence tour in tours)
            {
                TourNotifications notification = new TourNotifications();
                bool contains = false;


                notification.TourGuidence = tour;

                foreach (TourNotifications tourNotification in tourNotifications)
                {
                    if (tour.Id == tourNotification.TourGuidence.Id)
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    foreach (User user in userRepository.FindAll())
                    {
                        if (user.Type.Equals("guest2"))
                        {
                            TourNotifications tourNotification = new TourNotifications();

                            tourNotification.TourGuidence = notification.TourGuidence;
                            tourNotification.IsNotified = false;
                            tourNotification.User = user;
                            result.Add(tourNotification);
                        }
                   
                    }
                }
            }

            Save(result);
        }

        public void UpdateIsNotified(TourNotifications tourNotification)
        {
            List<TourNotifications> tourNotifications = FindAll();

            foreach(TourNotifications notification in tourNotifications)
            {
                if(notification.TourGuidence.Id.Equals(tourNotification.TourGuidence.Id) && notification.IsNotified.Equals(tourNotification.IsNotified) && notification.User.Username.Equals(tourNotification.User.Username))
                {
                    //notification.IsNotified = true;
                }
            }

            Save(tourNotifications);
        }

    }
}
