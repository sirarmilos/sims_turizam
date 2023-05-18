//using InitialProject.Model;
//using InitialProject.Repository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using InitialProject.Service;
//using System.Reflection;

//namespace InitialProject.View
//{

//    public partial class Guest1Start : Page
//    {

//        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;
//        private string guest1;


//        public string Guest1
//        {
//            get { return guest1; }
//            set
//            {
//                guest1 = value;
//            }
//        }

//        private bool notification;
//        public bool Notification
//        {
//            get { return notification; }
//            set
//            {
//                notification = value;
//            }
//        }

//        private void CheckNotification()
//        {
//            if (Notification)
//            {
//                NotificationMenuItem.Visibility = Visibility.Visible;
//            }
//            else
//            {
//                NotificationMenuItem.Visibility = Visibility.Collapsed;
//            }
//        }

//        public Guest1Start(string username, Page page)
//        {
//            InitializeComponent();
//            reservationReschedulingRequestService = new ReservationReschedulingRequestService(username);
//            Guest1 = username;

//            Notification = reservationReschedulingRequestService.Guest1HasNotification();
//            CheckNotification();
//            usernameAndSuperGuest.Header = Guest1 + CheckSuperType();
//        }

//        private string CheckSuperType()
//        {
//            string superType = string.Empty;

//            if (reservationReschedulingRequestService.IsSuperGuest(Guest1))
//            {
//                superType = " (Super guest)";
//            }

//            return superType;
//        }

//        private void GoToShowOwnerReviews(object sender, RoutedEventArgs e)
//        {
//            NavigationService?.Navigate(new ShowOwnerReviews(Guest1));
//        }

//        private void GoToGuest1Start(object sender, RoutedEventArgs e)
//        {
//            NavigationService?.Navigate(new Guest1Start(Guest1));
//        }

//        private void GoToSearchAndShowAccommodations(object sender, RoutedEventArgs e)
//        {
//            NavigationService?.Navigate(new SearchAndShowAccommodations(Guest1));
//        }

//        private void GoToShowReservations(object sender, RoutedEventArgs e)
//        {
//            NavigationService?.Navigate(new ShowReservations(Guest1));
//        }

//        private void GoToCreateReview(object sender, RoutedEventArgs e)
//        {
//            NavigationService?.Navigate(new CreateReview(Guest1));
//        }

//        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
//        {
//            NavigationService?.Navigate(new Guest1Requests(Guest1, this));
//        }
//        private void GoToShowGuest1Notifications(object sender, RoutedEventArgs e)
//        {
//            NavigationService?.Navigate(new ShowGuest1Notifications(Guest1));
//        }

//        private void GoToLogout(object sender, RoutedEventArgs e)
//        {
//            Window currentWindow = Window.GetWindow(this);

//            LoginForm window = new LoginForm();
//            window.Show();
//            currentWindow.Close();
//        }

//    }
//}
