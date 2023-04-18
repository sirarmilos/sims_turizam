using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InitialProject.Service;

namespace InitialProject.View
{

    public partial class Guest1Start : Window
    {

        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;
        private string guest1;


        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        private bool notification;
        public bool Notification
        {
            get { return notification; }
            set
            {
                notification = value;
            }
        }

        private void CheckNotification()
        {
            if (Notification)
            {
                NotificationMenuItem.Visibility = Visibility.Visible;
            }
            else
            {
                NotificationMenuItem.Visibility = Visibility.Collapsed;
            }
        }

        public Guest1Start(string username)
        {
            InitializeComponent();
            reservationReschedulingRequestService = new ReservationReschedulingRequestService(username);
            Guest1 = username;

            Notification = reservationReschedulingRequestService.Guest1HasNotification();
            CheckNotification();
        }

        private void GoToSearchAndShowAccommodations(object sender, RoutedEventArgs e)
        {
            SearchAndShowAccommodations window = new SearchAndShowAccommodations(Guest1);
            window.Show();
            Close();
        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            CreateReview window = new CreateReview(Guest1);
            window.Show();
            Close();
        }

        private void GoToShowReservations(object sender, RoutedEventArgs e)
        {
            ShowReservations window = new ShowReservations(Guest1);
            window.Show();
            Close();
        }

        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
        {
            Guest1Requests window = new Guest1Requests(Guest1);
            window.Show();
            Close(); 
        }


        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

    }
}
