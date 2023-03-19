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
    /// Interaction logic for RateGuests.xaml
    /// </summary>
    public partial class RateGuests : Window
    {
        public RateGuest rateGuest { get; set; }

        private readonly ReservationRepository reservationRepository;

        private readonly RateGuestRepository rateGuestRepository;

        private List<Reservation> reservations;
        private List<RateGuest> rateTheGuests;
        private List<RateGuestsDTO> rateGuestsDTOs;

        private RateGuestsDTO selectedGuest;

        private int cleanliness;
        private int followRules;
        private int behavior;
        private bool cash;
        private bool creditCard;
        private bool deferredPayment;
        private string typePayment;
        private int communicativeness;
        private string comment;

        public List<Reservation> Reservations
        {
            get;
            set;
        }

        public List<RateGuest> RateTheGuests
        {
            get;
            set;
        }

        public List<Reservation> GuestsWaitingForRate
        {
            get;
            set;
        }

        public List<RateGuestsDTO> RateGuestsDTOs
        {
            get;
            set;
        }

        public RateGuestsDTO SelectedGuest
        {
            get;
            set;
        }

        public int Cleanliness
        {
            get { return cleanliness; }
            set
            {
                cleanliness = value;
            }
        }

        public int FollowRules
        {
            get { return followRules; }
            set
            {
                followRules = value;
            }
        }

        public int Behavior
        {
            get { return behavior; }
            set
            {
                behavior = value;
            }
        }

        public bool Cash
        {
            get { return cash; }
            set
            {
                if (value != cash)
                {
                    cash = value;
                    if (value) TypePayment = "Cash";
                }
            }
        }

        public bool CreditCard
        {
            get { return creditCard; }
            set
            {
                if (value != creditCard)
                {
                    creditCard = value;
                    if (value) TypePayment = "CreditCard";
                }
            }
        }

        public bool DeferredPayment
        {
            get { return deferredPayment; }
            set
            {
                if (value != deferredPayment)
                {
                    deferredPayment = value;
                    if (value) TypePayment = "DeferredPayment";
                }
            }
        }

        public string TypePayment
        {
            get { return typePayment; }
            set
            {
                if (value != typePayment)
                {
                    typePayment = value;
                }
            }
        }

        public int Communicativeness
        {
            get { return communicativeness; }
            set
            {
                communicativeness = value;
            }
        }

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
            }
        }

        public RateGuests()
        {
            InitializeComponent();
            DataContext = this;
            reservationRepository = new ReservationRepository();
            rateGuestRepository = new RateGuestRepository();
            Reservations = reservationRepository.FindAllReservations();
            RateTheGuests = reservationRepository.FindAllRateGuests();
            RateGuestsDTOs = new List<RateGuestsDTO>();
            FindGuestsForRate();
            dgRateGuests.ItemsSource = RateGuestsDTOs;

            if (RateGuestsDTOs.Count == 0)
            {
                MessageBox.Show("All guests are rated.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("There are " + RateGuestsDTOs.Count + " guests left for you to rate.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            buttonCancelSelection.Visibility = Visibility.Hidden;
            TurnOffVisibility();
            SetDefaultValue();
        }

        void LoadingRowForDgRateGuests(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void FindGuestsForRate()
        {
            foreach (Reservation temporaryReservation in Reservations)
            {
                int indicator = 0;

                foreach (RateGuest temporaryRateGuests in RateTheGuests)
                {
                    if (temporaryReservation.ReservationId.Equals(temporaryRateGuests.Reservation.ReservationId))
                    {
                        indicator = 1;
                        break;
                    }
                }

                if (indicator == 0)
                {
                    var differenceBetweenDates = DateTime.Now.Subtract(temporaryReservation.EndDate);
                    int days = differenceBetweenDates.Days;

                    if (days <= 5 && days >= 0)
                    {
                        int daysLeft = 5 - days;
                        string deadline;

                        if (daysLeft == 0)
                        {
                            deadline = "This is the last day to rate a guest.";
                        }
                        else
                        {
                            deadline = "You have " + daysLeft + " more days to rate the guest.";
                        }

                        RateGuestsDTO rateGuestDTO = new RateGuestsDTO(temporaryReservation.ReservationId, temporaryReservation.GuestUsername, deadline);
                        RateGuestsDTOs.Add(rateGuestDTO);
                    }
                }
            }
        }

        private void ShowInputForRateGuest(object sender, RoutedEventArgs e)
        {
            if(SelectedGuest == null)
            {
                MessageBox.Show("Select the guest you want to rate", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                dgRateGuests.IsHitTestVisible = false;
                TurnOnVisibility();
                buttonRateGuest.Visibility = Visibility.Hidden;
            }
        }

        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var label = (Label)sender;
                Keyboard.Focus(label.Target);
            }
        }

        private void TurnOffVisibility()
        {
            labelCleanliness.Visibility = Visibility.Hidden;
            tbCleanliness.Visibility = Visibility.Hidden;
            sliderCleanliness.Visibility = Visibility.Hidden;

            labelFollowRules.Visibility = Visibility.Hidden;
            tbFollowRules.Visibility = Visibility.Hidden;
            sliderFollowRules.Visibility = Visibility.Hidden;

            labelBehavior.Visibility = Visibility.Hidden;
            tbBehavior.Visibility = Visibility.Hidden;
            sliderBehavior.Visibility = Visibility.Hidden;

            gbTypePayment.Visibility = Visibility.Hidden;

            labelCommunicativeness.Visibility = Visibility.Hidden;
            tbCommunicativeness.Visibility = Visibility.Hidden;
            sliderCommunicativeness.Visibility = Visibility.Hidden;

            labelComment.Visibility = Visibility.Hidden;
            tbComment.Visibility = Visibility.Hidden;

            buttonConfirm.Visibility = Visibility.Hidden;
        }

        private void TurnOnVisibility()
        {
            labelCleanliness.Visibility = Visibility.Visible;
            tbCleanliness.Visibility = Visibility.Visible;
            sliderCleanliness.Visibility = Visibility.Visible;

            labelFollowRules.Visibility = Visibility.Visible;
            tbFollowRules.Visibility = Visibility.Visible;
            sliderFollowRules.Visibility = Visibility.Visible;

            labelBehavior.Visibility = Visibility.Visible;
            tbBehavior.Visibility = Visibility.Visible;
            sliderBehavior.Visibility = Visibility.Visible;

            gbTypePayment.Visibility = Visibility.Visible;

            labelCommunicativeness.Visibility = Visibility.Visible;
            tbCommunicativeness.Visibility = Visibility.Visible;
            sliderCommunicativeness.Visibility = Visibility.Visible;

            labelComment.Visibility = Visibility.Visible;
            tbComment.Visibility = Visibility.Visible;

            buttonConfirm.Visibility = Visibility.Visible;

        }

        private void SetDefaultValue()
        {
            rbCash.IsChecked = true;
            sliderCleanliness.Value = 3;
            sliderFollowRules.Value = 3;
            sliderBehavior.Value = 3;
            sliderCommunicativeness.Value = 3;
            Comment = "";
            tbComment.Text = "";
        }

        private void SliderCleanlinessValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Cleanliness = Convert.ToInt32(sliderCleanliness.Value);
        }

        private void SliderFollowRulesValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            FollowRules = Convert.ToInt32(sliderFollowRules.Value);
        }

        private void SliderBehaviorValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Behavior = Convert.ToInt32(sliderBehavior.Value);
        }

        private void SliderCommunicativenessValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Communicativeness = Convert.ToInt32(sliderCommunicativeness.Value);
        }

        private void SaveRateGuest(object sender, RoutedEventArgs e)
        {
            Reservation reservation = new Reservation();

            foreach (Reservation temporaryReservation in Reservations)
            {
                if(temporaryReservation.ReservationId.Equals(SelectedGuest.ReservationId) == true)
                {
                    reservation = temporaryReservation;
                    break;
                }
            }

            rateGuestRepository.Save(reservation, Cleanliness, FollowRules, Behavior, TypePayment, Communicativeness, Comment);

            RateGuestsDTOs.Remove(SelectedGuest);

            dgRateGuests.Items.Refresh();

            dgRateGuests.IsHitTestVisible = true;
            buttonCancelSelection.Visibility = Visibility.Hidden;
            buttonRateGuest.Visibility = Visibility.Visible;
            TurnOffVisibility();
            SetDefaultValue();

            MessageBox.Show("You have successfully rated a guest", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            if (RateGuestsDTOs.Count == 0)
            {
                dgRateGuests.Visibility = Visibility.Hidden;
                buttonRateGuest.Visibility = Visibility.Hidden;
                MessageBox.Show("All guests are rated.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        private void CancelSelection(object sender, RoutedEventArgs e)
        {
            SelectedGuest = null;
            buttonRateGuest.Visibility = Visibility.Visible;
            TurnOffVisibility();
            dgRateGuests.UnselectAllCells();
            SetDefaultValue();
            dgRateGuests.IsHitTestVisible = true;
            buttonCancelSelection.Visibility = Visibility.Hidden;
        }

        private void CancelButtonVisibility(object sender, SelectionChangedEventArgs e)
        {
            buttonCancelSelection.Visibility = Visibility.Visible;
        }
    }
}
