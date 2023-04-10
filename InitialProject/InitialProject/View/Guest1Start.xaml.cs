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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest1Start.xaml
    /// </summary>
    public partial class Guest1Start : Window
    {

        private string guest1;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        //public OwnerStart(string username)
        //{
        //    InitializeComponent();
        //    Owner = username;

        //    reviewRepository = new ReviewRepository();
        //    reservationRepository = new ReservationRepository();
        //    userRepository = new UserRepository();

        //    AllReviews = new List<Review>();
        //    AllReservations = new List<Reservation>();

        //    usernameAndSuperOwner.Header = Owner;

        //    CheckSuperOwner();
        //}

        public Guest1Start(string username)
        {
            InitializeComponent();
            Guest1 = username;

        }

        private void GoToSearchAndShowAccommodation(object sender, RoutedEventArgs e)
        {
            SearchAndShowAccommodations window = new SearchAndShowAccommodations(Guest1);
            window.Show();
        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            CreateReview window = new CreateReview(Guest1);
            window.Show();
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

    }
}
