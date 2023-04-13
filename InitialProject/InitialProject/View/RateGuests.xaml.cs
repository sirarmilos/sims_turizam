﻿using InitialProject.DTO;
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
    /// <summary>
    /// Interaction logic for RateGuests.xaml
    /// </summary>
    public partial class RateGuests : Window
    {
        private readonly RateGuestsService rateGuestsService;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        private int cleanliness;
        private int followRules;
        private int behavior;
        private bool cash;
        private bool creditCard;
        private bool deferredPayment;
        private string typePayment;
        private int communicativeness;
        private string comment;

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

        public RateGuests(string owner)
        {
            InitializeComponent();

            Owner = owner;

            DataContext = this;

            rateGuestsService = new RateGuestsService(Owner);

            RateGuestsDTOs = new List<RateGuestsDTO>();

            RateGuestsDTOs = rateGuestsService.FindAllGuestsForRate();

            if (RateGuestsDTOs.Count == 0)
            {
                MessageBox.Show("All guests are rated.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("There are " + RateGuestsDTOs.Count + " guests left for you to rate.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            SetDefaultValue();
        }

        private void SaveRateGuest(object sender, RoutedEventArgs e)
        {
            SaveNewRateGuestDTO saveNewRateGuestDTO = new SaveNewRateGuestDTO(SelectedGuest.ReservationId, Cleanliness, FollowRules, Behavior, TypePayment, Communicativeness, Comment);

            rateGuestsService.SaveNewRateGuest(saveNewRateGuestDTO);

            RateGuestsDTOs.Remove(SelectedGuest);
            dgRateGuests.Items.Refresh();

            SetDefaultValue();

            MessageBox.Show("You have successfully rated a guest", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            if (RateGuestsDTOs.Count == 0)
            {
                MessageBox.Show("All guests are rated.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        private void CancelRate(object sender, RoutedEventArgs e)
        {
            SetDefaultValue();
            dgRateGuests.SelectedItem = null;
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
            buttonRate.IsEnabled = false;
        }

        private void RateButtonEnable(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedGuest == null)
            {
                buttonRate.IsEnabled = false;
            }
            else
            {
                buttonRate.IsEnabled = true;
            }
        }

        void LoadingRowForDgRateGuests(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var label = (Label)sender;
                Keyboard.Focus(label.Target);
            }
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
