using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for Guest2ComplexTourRequestCreation.xaml
    /// </summary>
    public partial class Guest2ComplexTourRequestCreation : Page
    {
        private readonly TourRequestService tourRequestService = new TourRequestService();
        private readonly LocationService locationService = new LocationService();
        private readonly UserService userService = new UserService();

        private List<TourRequest> tourRequests;

        public ObservableCollection<TourRequest> requests { get; set; }

        private string Username;

        private string city;
        private string country;
        private string description;
        private Language language;
        private string maxGuests;

        private DateTime startDate;
        private DateTime endDate;

        public string City
        {
            get { return city; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(Country) || string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(MaxGuests))
                {
                    createRequestButton.IsEnabled = false;
                }
                else
                {
                    createRequestButton.IsEnabled = true;
                }
                city = value;
                OnPropertyChanged(nameof(City));
            }

        }

        public string Country
        {
            get { return country; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(MaxGuests))
                {
                    createRequestButton.IsEnabled = false;
                }
                else
                {
                    createRequestButton.IsEnabled = true;
                }
                country = value;
                OnPropertyChanged(nameof(Country));
            }

        }

        public string Description
        {
            get { return description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(Country) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(MaxGuests))
                {
                    createRequestButton.IsEnabled = false;
                }
                else
                {
                    createRequestButton.IsEnabled = true;
                }
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

        public string MaxGuests
        {
            get { return maxGuests; }
            set
            {
                if (int.TryParse(value.ToString(), out int result))
                {
                    if (int.Parse(value) < 0)
                    {
                        maxGuests = value;
                        maxGuestsWarning.Visibility = Visibility.Visible;
                        createRequestButton.IsEnabled = false;
                    }
                    else
                    {
                        maxGuests = value;
                        maxGuestsWarning.Visibility = Visibility.Hidden;
                        createRequestButton.IsEnabled = true;
                    }
                }
                else
                {
                    maxGuests = value;
                    maxGuestsWarning.Visibility = Visibility.Visible;
                    createRequestButton.IsEnabled = false;
                }

                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(Country) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Description))
                {
                    createRequestButton.IsEnabled = false;
                }


                OnPropertyChanged(nameof(MaxGuests));
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (EndDate < value)
                {
                    createRequestButton.IsEnabled = false;
                    datePickerWarningLabel.Visibility = Visibility.Visible;
                }


                else
                {
                    createRequestButton.IsEnabled = true;
                    datePickerWarningLabel.Visibility = Visibility.Hidden;
                }


                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (StartDate > value)
                {
                    createRequestButton.IsEnabled = false;
                    datePickerWarningLabel.Visibility = Visibility.Visible;
                }


                else
                {
                    createRequestButton.IsEnabled = true;
                    datePickerWarningLabel.Visibility = Visibility.Hidden;
                }

                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guest2ComplexTourRequestCreation(string username)
        {
            InitializeComponent();
            this.Username = username;
            DataContext = this;

            tourRequests = new List<TourRequest>();
            InitializeCbLang();
            requests = new ObservableCollection<TourRequest>();

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            finnishButton.IsEnabled = false;
            createRequestButton.IsEnabled = false;
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

            LocationDto locationDto = new LocationDto(City, Country, "", 0, 0);
            Location location = locationService.Save(locationDto);
            User user = userService.FindByUsername(Username);

            tourRequest.User = user;
            tourRequest.Location = location;
            tourRequest.Description = Description;
            tourRequest.Language = Languagee;
            tourRequest.GuestNumber = int.Parse(MaxGuests);
            tourRequest.StartDate = StartDate;
            tourRequest.EndDate = EndDate;
            tourRequest.Status = "pending";
            tourRequest.CreationDate = DateTime.Now;
            tourRequest.ComplexTourRequestId = 2;
            tourRequest.AcceptedDate = "";

            requests.Add(tourRequest);


            tbCity.Text = "";
            tbCountry.Text = "";
            tbDesc.Text = "";
            tbGuests.Text = "";
            toDate.SelectedDate = DateTime.Now;
            fromDate.SelectedDate = DateTime.Now;
            cbLanguage.SelectedIndex = -1;

            if(requests.Count>=2)
            {
                finnishButton.IsEnabled = true;
            }

            maxGuestsWarning.Visibility = Visibility.Hidden;
            datePickerWarningLabel.Visibility = Visibility.Hidden;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tourRequestService.SaveList(requests.ToList());
            requests.Clear();
            Guest2ComplexRequestSuccess guest2ComplexRequestSuccess = new Guest2ComplexRequestSuccess();
            NavigationService.Navigate(guest2ComplexRequestSuccess);

        }


    }
}
