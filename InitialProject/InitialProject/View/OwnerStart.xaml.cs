using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for OwnerStart.xaml
    /// </summary>
    public partial class OwnerStart : Window
    {
        private readonly ReviewRepository reviewRepository;

        private readonly ReservationRepository reservationRepository;

        private readonly UserRepository userRepository;

        private List<Review> allReviews;

        private List<Review> reviews;

        private List<Reservation> allReservations;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        private List<Review> AllReviews
        {
            get;
            set;
        }

        private List<Review> Reviews
        {
            get;
            set;
        }

        private List<Reservation> AllReservations
        {
            get;
            set;
        }

        public OwnerStart(string username)
        {
            InitializeComponent();
            Owner = username;

            reviewRepository = new ReviewRepository();
            reservationRepository = new ReservationRepository();
            userRepository = new UserRepository();

            AllReviews = new List<Review>();
            AllReservations = new List<Reservation>();

            usernameAndSuperOwner.Header = Owner;

            CheckSuperOwner();
        }

        private void CheckSuperOwner()
        {
            AllReviews = reviewRepository.FindAll();
            AllReservations = reservationRepository.FindAllReservations();
            Reviews = new List<Review>();

            FindGuestReviews();

            if (Reviews.Count >= 50)
            {

                if(FindAverageGuestReviews() > new decimal(4.5))
                {
                    usernameAndSuperOwner.Header = Owner + " " + "(Super owner)";
                }
            }

            UpdateUsers();
        }

        private void FindGuestReviews()
        {
            foreach (Review temporaryReview in AllReviews.ToList())
            {
                foreach (Reservation temporaryReservation in AllReservations.ToList())
                {
                    if (temporaryReview.Reservation.ReservationId == temporaryReservation.ReservationId)
                    {
                        temporaryReview.Reservation = temporaryReservation;
                        if (temporaryReview.Reservation.Accommodation.OwnerUsername.Equals(Owner) == true)
                        {
                            Reviews.Add(temporaryReview);
                        }
                    }
                }
            }
        }

        private decimal FindAverageGuestReviews()
        {
            decimal sumAverageGuestReview = 0;

            foreach (Review temporaryReview in Reviews.ToList())
            {
                sumAverageGuestReview += (temporaryReview.Cleanliness + temporaryReview.Staff + temporaryReview.Comfort + temporaryReview.ValueForMoney) / new Decimal(4.0);
            }

            Trace.WriteLine(sumAverageGuestReview / Reviews.Count);

            return sumAverageGuestReview / Reviews.Count;
        }

        private void UpdateUsers()
        {
            List<User> allUsers = userRepository.FindAllUsers();

            foreach (User temporaryUser in allUsers.ToList())
            {
                if (temporaryUser.Username.Equals(Owner) == true)
                {
                    if (usernameAndSuperOwner.Header.Equals(Owner) == true)
                    {
                        temporaryUser.SuperType = "no_super";
                    }
                    else
                    {
                        temporaryUser.SuperType = "super";
                    }
                    break;
                }
            }

            userRepository.UpdateUsers(allUsers);
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
    }
}
