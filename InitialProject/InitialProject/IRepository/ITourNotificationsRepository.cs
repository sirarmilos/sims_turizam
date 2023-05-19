using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ITourNotificationsRepository
    {
        List<TourNotifications> FindAll();

        void Save(List<TourNotifications> tourNotifications);

        void Save(TourNotifications tourNotification);

        void Update();

        void UpdateIsNotified(TourNotifications tourNotification);
    }
}
