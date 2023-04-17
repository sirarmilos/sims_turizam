using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using GalaSoft.MvvmLight.Command;
using InitialProject.Service;
using InitialProject.DTO;

namespace InitialProject.View
{
    public partial class SearchAndShowAccommodations : Window
    {
        AccommodationService accommodationService;
        public Accommodation Accommodation { get; set; }

        private AccommodationRepository accommodationRepository; // izbaci

        private string accommodationName;
        private string country;
        private string city;
        private string type;
        private int? maxGuests;
        private int? minDaysReservation;
        private int leftCancelationDays;
        private string image;
        private List<string> images;
        private bool apartment;
        private bool allTypes;
        private bool home;
        private bool hut;
        private string guest1;
        private bool notification;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        public List<string> Images
        {
            get;
            set;
        }

        public string AccommodationName
        {
            get { return accommodationName; }
            set
            {
                accommodationName = value;
                OnPropertyChanged();
            }
        }

        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged(nameof(Country));

            }
        }

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public int? MaxGuests
        {
            get { return maxGuests; }
            set
            {
                maxGuests = value;
                OnPropertyChanged();
            }
        }

        public int? MinDaysReservation
        {
            get { return minDaysReservation; }
            set
            {
                minDaysReservation = value;
                OnPropertyChanged();
            }
        }

        public bool Apartment
        {
            get { return apartment; }
            set
            {
                if (value != apartment)
                {
                    apartment = value;
                    OnPropertyChanged(nameof(apartment));
                    if (value) Type = "Apartment";
                }
            }
        }

        public bool AllTypes
        {
            get { return allTypes; }
            set
            {
                if (value != allTypes)
                {
                    allTypes = value;
                    OnPropertyChanged(nameof(allTypes));
                    if (value) Type = null;
                }
            }
        }

        public bool Home
        {
            get { return home; }
            set
            {
                if (value != home)
                {
                    home = value;
                    OnPropertyChanged(nameof(Home));
                    if (value) Type = "Home";
                }
            }
        }

        public bool Hut
        {
            get { return hut; }
            set
            {
                if (value != hut)
                {
                    hut = value;
                    OnPropertyChanged(nameof(Hut));
                    if (value) Type = "Hut";
                }
            }
        }

        public string Type
        {
            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        public ICommand SeeAvailabilityCommand { get; set; }

        private void AllowOnlyCharacters(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, 0) && !char.IsWhiteSpace(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void AllowOnlyDigits(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        public bool Notification
        {
            get { return notification; }
            set
            {
                notification = value;
            }
        }

        private void CheckNotification()
        {
            if (Notification)
            {
                NotificationMenuItem.Visibility = Visibility.Visible;
            }
            else
            {
                NotificationMenuItem.Visibility = Visibility.Collapsed;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public SearchAndShowAccommodations(string username)
        {
            InitializeComponent();

            Guest1 = username;
            DataContext = this;

            accommodationService = new AccommodationService(Guest1);
            Notification = accommodationService.Guest1HasNotification();
            CheckNotification();

            accommodationRepository = new AccommodationRepository();
            Images = new List<string>();
            SeeAvailabilityCommand = new RelayCommand<Accommodation>(SeeAvailability);
        }


        private void Search(object sender, RoutedEventArgs e)
        {
            if ((MaxGuests != null) && (MaxGuests == 0))
            {
                MessageBox.Show("You can't use zero as number of guests.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                ListAccommodations.ItemsSource = null;
                return;
            }

            SearchAndShowAccommodationDTO searchShowAndAccommodationDTO = 
                new SearchAndShowAccommodationDTO(AccommodationName, Country, City, Type, MaxGuests, MinDaysReservation);
            ListAccommodations.ItemsSource =  accommodationService.FindAll(searchShowAndAccommodationDTO);
        }

        private void SeeAvailability(Accommodation accommodation)
        {
            AccommodationReservation window = new AccommodationReservation(accommodation, Guest1);
            window.Show();
            Close();
        }

        private void GoToGuest1Start(object sender, RoutedEventArgs e)
        {
            Guest1Start window = new Guest1Start(Guest1);
            window.Show();
            Close();
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
