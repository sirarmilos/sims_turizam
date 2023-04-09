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
        private readonly ReviewRepository reviewRepository;

        private readonly ReservationRepository reservationRepository;

        private readonly RateGuestRepository rateGuestRepository;

        private List<Review> reviews;

        private List<Review> allReviews;

        private List<RateGuest> rateGuests;

        private List<RateGuest> allRateGuests;

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

        public List<Review> AllReviews
        {
            get;
            set;
        }

        public List<RateGuest> RateGuests
        {
            get;
            set;
        }

        public List<RateGuest> AllRateGuests
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
            AllReviews = new List<Review>();
            RateGuests = new List<RateGuest>();
            AllRateGuests = new List<RateGuest>();
            ShowGuestReviewsDTOs = new List<ShowGuestReviewsDTO>();

            FindAllOwnerReviews();
            FindAllRatedGuest();
            FindAllowedReviews();

            dgShowGuestReviews.ItemsSource = ShowGuestReviewsDTOs;
        }

        private void FindAllOwnerReviews()
        {
            AllReviews = reviewRepository.FindAllReviews();

            FindReservationsForReviews();

            foreach(Review temporaryReview in AllReviews.ToList())
            {
                if (temporaryReview.Reservation.Accommodation.OwnerUsername.Equals(Owner) == true)
                {
                    Reviews.Add(temporaryReview);
                }
            }
        }

        private void FindReservationsForReviews()
        {
            List<Reservation> allReservations = new List<Reservation>();
            allReservations = reservationRepository.FindAllReservations();

            foreach (Review temporaryReview in AllReviews.ToList())
            {
                foreach (Reservation temporaryReservation in allReservations.ToList())
                {
                    if (temporaryReview.Reservation.ReservationId == temporaryReservation.ReservationId)
                    {
                        temporaryReview.Reservation = temporaryReservation;
                        break;
                    }
                }
            }
        }

        private void FindAllRatedGuest()
        {
            AllRateGuests = rateGuestRepository.FindAllRateGuests();

            FindReservationsForRateGuestsReview();

            foreach(RateGuest temporaryRateGuest in AllRateGuests.ToList())
            {
                if(temporaryRateGuest.Reservation.Accommodation.OwnerUsername.Equals(Owner) == true)
                {
                    RateGuests.Add(temporaryRateGuest);
                }
            }
        }

        public void FindReservationsForRateGuestsReview()
        {
            List<Reservation> allReservations = new List<Reservation>();
            allReservations = reservationRepository.FindAllReservations();

            foreach (RateGuest temporaryRateGuest in AllRateGuests.ToList())
            {
                foreach (Reservation temporaryReservation in allReservations.ToList())
                {
                    if (temporaryRateGuest.Reservation.ReservationId == temporaryReservation.ReservationId)
                    {
                        temporaryRateGuest.Reservation = temporaryReservation;
                        break;
                    }
                }
            }
        }

        private void FindAllowedReviews()
        {
            foreach(RateGuest temporaryRateGuest in RateGuests.ToList())
            {
                foreach(Review temporaryReview in Reviews.ToList())
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

        private void GoToShowOwnerManageBookingMoveRequests(object sender, RoutedEventArgs e)
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(Owner);
            window.Show();
            Close();
        }

        void LoadingRowForDgShowGuestReviews(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

    }
}
