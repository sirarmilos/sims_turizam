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

    public partial class ShowOwnerReviews : Page
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

        public ShowOwnerReviews(string guest1, Page page)
        {
            InitializeComponent();

            Guest1 = guest1;

            DataContext = this;

            reviewService = new ReviewService(Guest1);

            ShowOwnerReviewsDTOs = new List<ShowOwnerReviewsDTO>();

            ShowOwnerReviewsDTOs = reviewService.FindAllOwnerReviews();

            usernameAndSuperGuest.Header = Guest1 + CheckSuperType();
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (reviewService.IsSuperGuest(Guest1))
            {
                superType = " (Super guest)";
            }

            return superType;
        }

        void LoadingRowForDgShowOwnerReviews(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void GoToShowOwnerReviews(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowOwnerReviews(Guest1, this));
        }

        //private void GoToGuest1Start(object sender, RoutedEventArgs e)
        //{
        //    NavigationService?.Navigate(new Guest1Start(Guest1, this));
        //}

        private void GoToSearchAndShowAccommodations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SearchAndShowAccommodations(Guest1, this));
        }

        private void GoToShowReservations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowReservations(Guest1, this));
        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateReview(Guest1, this));
        }

        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1Requests(Guest1, this));
        }

        private void GoToShowGuest1Notifications(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowGuest1Notifications(Guest1, this));
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);

            LoginForm window = new LoginForm();
            window.Show();
            currentWindow.Close();
        }

    }
}
