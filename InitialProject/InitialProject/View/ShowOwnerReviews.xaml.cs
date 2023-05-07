using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public partial class ShowOwnerReviews : Window
    {
        private readonly ReviewService reviewService;

        private string guest1;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public List<ShowOwnerReviewsDTO> ShowOwnerReviewsDTOs
        {
            get;
            set;
        }

        public ShowOwnerReviews(string guest1/*, string guestHeader*/)
        {
            InitializeComponent();

            Guest1 = guest1;

            DataContext = this;

            reviewService = new ReviewService(Guest1);

            ShowOwnerReviewsDTOs = new List<ShowOwnerReviewsDTO>();

            ShowOwnerReviewsDTOs = reviewService.FindAllOwnerReviews();

            //usernameAndSuperGuest.Header = guestHeader; ovo ce mi trebati za super guesta
        }

        void LoadingRowForDgShowOwnerReviews(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
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
