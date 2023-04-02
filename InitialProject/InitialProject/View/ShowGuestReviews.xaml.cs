using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
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
    /// <summary>
    /// Interaction logic for ShowGuestReviews.xaml
    /// </summary>
    public partial class ShowGuestReviews : Window
    {
        public ShowGuestReviews showGuestReviews { get; set; }

        private readonly ReviewRepository reviewRepository;

        private readonly ReservationRepository reservationRepository;

        private readonly RateGuestRepository rateGuestRepository;

        private List<Review> reviews;

        private List<RateGuest> rateGuests;

        private List<ShowGuestReviewsDTO> showGuestReviewsDTOs;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        public List<Review> Reviews
        {
            get;
            set;
        }

        public List<RateGuest> RateGuests
        {
            get;
            set;
        }

        public List<ShowGuestReviewsDTO> ShowGuestReviewsDTOs
        {
            get { return showGuestReviewsDTOs; }
            set
            {
                showGuestReviewsDTOs = value;
            }
        }

        public ShowGuestReviews(string owner)
        {
            InitializeComponent();
            Owner = owner;
            DataContext = this;
            reviewRepository = new ReviewRepository();
            reservationRepository = new ReservationRepository();
            rateGuestRepository = new RateGuestRepository();
            Reviews = new List<Review>();
            RateGuests = new List<RateGuest>();
            ShowGuestReviewsDTOs = new List<ShowGuestReviewsDTO>();
            FindAllOwnerReviews();
            FindAllRatedGuest();
            FindAllowedReviews();
            dgShowGuestReviews.ItemsSource = ShowGuestReviewsDTOs;
        }

        private void FindAllOwnerReviews()
        {
            Reviews = reviewRepository.FindAllReviews();

            Reviews = reservationRepository.FindReservationsForReviews(Reviews);

            List<Review> temporaryReviews = new List<Review>(Reviews);

            foreach(Review temporaryReview in temporaryReviews)
            {
                if (temporaryReview.Reservation.Accommodation.OwnerUsername.Equals(Owner) == false)
                {
                    Reviews.Remove(temporaryReview);
                }
            }
        }

        private void FindAllRatedGuest()
        {
            RateGuests = rateGuestRepository.FindAllRateGuests();

            RateGuests = reservationRepository.FindReservationsForRateGuestsReview(RateGuests);

            List<RateGuest> temporaryRateGuests = new List<RateGuest>(RateGuests);

            foreach(RateGuest temporaryRateGuest in temporaryRateGuests)
            {
                if(temporaryRateGuest.Reservation.Accommodation.OwnerUsername.Equals(Owner) == false)
                {
                    RateGuests.Remove(temporaryRateGuest);
                }
            }
        }

        private void FindAllowedReviews()
        {
            foreach(RateGuest temporaryRateGuest in RateGuests)
            {
                foreach(Review temporaryReview in Reviews)
                {
                    if(temporaryRateGuest.Reservation.ReservationId == temporaryReview.Reservation.ReservationId)
                    {
                        ShowGuestReviewsDTO showGuestReviewsDTO = new ShowGuestReviewsDTO(temporaryReview.Reservation.Accommodation.AccommodationName, temporaryReview.Reservation.GuestUsername, temporaryReview.Cleanliness, temporaryReview.Staff, temporaryReview.Comfort, temporaryReview.ValueForMoney, temporaryReview.Comment);
                        ShowGuestReviewsDTOs.Add(showGuestReviewsDTO);
                    }
                }
            }
        }

        private void GoToAddNewAccommodation(object sender, RoutedEventArgs e)
        {
            AddNewAccommodation window = new AddNewAccommodation(Owner);
            window.Show();
        }

        private void GoToRateGuests(object sender, RoutedEventArgs e)
        {
            RateGuests window = new RateGuests(Owner);
            if (window.dgRateGuests.Items.Count > 0)
            {
                window.Show();
            }
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

        private void GoToShowGuestReviews(object sender, RoutedEventArgs e)
        {
            ShowGuestReviews window = new ShowGuestReviews(Owner);
            window.Show();
            Close();
        }

        void LoadingRowForDgShowGuestReviews(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

    }
}
