using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
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
        private readonly UserService userService;

        public string OwnerUsername
        {
            get;
            set;
        }

        public OwnerStart(string username)
        {
            InitializeComponent();

            OwnerUsername = username;

            userService = new UserService();

            SetMenu();
        }

        private void SetMenu()
        {
            usernameAndSuperOwner.Header = OwnerUsername + CheckSuperType();

            rateGuestsNotifications.Header = "Number of unrated guests: " + userService.FindNumberOfUnratedGuests(OwnerUsername) + ".";
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (userService.FindSuperTypeByOwnerName(OwnerUsername).Equals("super") == true)
            {
                superType = " (Super owner)";
            }

            return superType;
        }

        private void GoToAddNewAccommodation(object sender, RoutedEventArgs e)
        {
            AddNewAccommodation window = new AddNewAccommodation(OwnerUsername);
            window.ShowDialog();
        }

        private void GoToRateGuests(object sender, RoutedEventArgs e)
        {
            RateGuests window = new RateGuests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToShowGuestReviews(object sender, RoutedEventArgs e)
        {
            ShowGuestReviews window = new ShowGuestReviews(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToShowOwnerManageBookingMoveRequests(object sender, RoutedEventArgs e)
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
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
