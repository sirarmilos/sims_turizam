using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2TourRequestCreation.xaml
    /// </summary>
    public partial class Guest2TourRequestCreation : Page
    {
        private readonly TourRequestService tourRequestService = new TourRequestService();
        private readonly LocationService locationService = new LocationService();
        private readonly UserService userService = new UserService();

        private string Username;

        private string city;
        private string country;
        private string description;
        private Language language;
        private int maxGuests;

        private DateTime startDate;
        private DateTime endDate;

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));
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

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Country));
            }

        }


        public Language Languagee
        {
            get { return language; }
            set
            {
                language = value;
                OnPropertyChanged(nameof(Languagee));
            }
        }

        public int MaxGuests
        {
            get { return maxGuests; }
            set
            {
                maxGuests = value;
                OnPropertyChanged(nameof(MaxGuests));
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set 
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guest2TourRequestCreation(string Username)
        {
            InitializeComponent();
            InitializeCbLang();
            this.Username = Username;
            DataContext = this;
        }

        public void InitializeCbLang()
        {
            foreach (string lang in (Enum.GetNames(typeof(Language))))
            {
                cbLanguage.Items.Add(lang);
            }
            cbLanguage.SelectedIndex = 0;
        }

        private void CreateTourRequest(object sender, RoutedEventArgs e)
        {
            TourRequest tourRequest = new TourRequest();

            LocationDto locationDto = new LocationDto(City,Country,"",0,0);
            Location location = locationService.Save(locationDto);
            User user = userService.FindByUsername(Username);

            tourRequest.User = user;
            tourRequest.Location = location;
            tourRequest.Description = Description;
            tourRequest.Language = Languagee;
            tourRequest.GuestNumber = MaxGuests;
            tourRequest.StartDate = StartDate;
            tourRequest.EndDate = EndDate;
            tourRequest.Status = "pending";
            tourRequest.CreationDate = DateTime.Now;

            bool result = tourRequestService.SaveTourRequest(tourRequest);
        }
    }
}
