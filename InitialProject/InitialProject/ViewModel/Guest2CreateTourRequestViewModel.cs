using GalaSoft.MvvmLight.CommandWpf;
using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.ViewModel
{
    public class Guest2CreateTourRequestViewModel : INotifyPropertyChanged
    {
        private readonly TourRequestService tourRequestService = new TourRequestService();
        private readonly LocationService locationService = new LocationService();
        private readonly UserService userService = new UserService();

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
                    guest2TourRequestCreation.createRequestButton.IsEnabled = false;
                }
                else
                {
                    guest2TourRequestCreation.createRequestButton.IsEnabled = true;
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
                    guest2TourRequestCreation.createRequestButton.IsEnabled = false;
                }
                else
                {
                    guest2TourRequestCreation.createRequestButton.IsEnabled = true;
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
                    guest2TourRequestCreation.createRequestButton.IsEnabled = false;
                }
                else
                {
                    guest2TourRequestCreation.createRequestButton.IsEnabled = true;
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
                        guest2TourRequestCreation.maxGuestsWarning.Visibility = Visibility.Visible;
                        guest2TourRequestCreation.createRequestButton.IsEnabled = false;
                    }
                    else
                    {
                        maxGuests = value;
                        guest2TourRequestCreation.maxGuestsWarning.Visibility = Visibility.Hidden;
                        guest2TourRequestCreation.createRequestButton.IsEnabled = true;
                    }
                }
                else
                {
                    maxGuests = value;
                    guest2TourRequestCreation.maxGuestsWarning.Visibility = Visibility.Visible;
                    guest2TourRequestCreation.createRequestButton.IsEnabled = false;
                }

                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(Country) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Description))
                {
                    guest2TourRequestCreation.createRequestButton.IsEnabled = false;
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
                    guest2TourRequestCreation.createRequestButton.IsEnabled = false;
                    guest2TourRequestCreation.datePickerWarningLabel.Visibility = Visibility.Visible;
                }

                else
                {
                    guest2TourRequestCreation.createRequestButton.IsEnabled = true;
                    guest2TourRequestCreation.datePickerWarningLabel.Visibility = Visibility.Hidden;
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
                    guest2TourRequestCreation.createRequestButton.IsEnabled = false;
                    guest2TourRequestCreation.datePickerWarningLabel.Visibility = Visibility.Visible;
                }

                else
                {
                    guest2TourRequestCreation.createRequestButton.IsEnabled = true;
                    guest2TourRequestCreation.datePickerWarningLabel.Visibility = Visibility.Hidden;
                }

                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }


        public ICommand CreateRequestCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;


        private Guest2TourRequestCreation guest2TourRequestCreation;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guest2CreateTourRequestViewModel(Guest2TourRequestCreation guest2TourRequestCreation)
        {
            this.guest2TourRequestCreation = guest2TourRequestCreation;
            InitializeCbLang();
            this.Username = UserClass.Username;

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            guest2TourRequestCreation.createRequestButton.IsEnabled = false;
            CreateRequestCommand = new RelayCommand(CreateTourRequest);
        }


        public void InitializeCbLang()
        {
            foreach (string lang in (Enum.GetNames(typeof(Language))))
            {
                guest2TourRequestCreation.cbLanguage.Items.Add(lang);
            }
            guest2TourRequestCreation.cbLanguage.SelectedIndex = 0;
        }

        private void CreateTourRequest()
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
            tourRequest.ComplexTourRequestId = 0;
            tourRequest.AcceptedDate = "";

            bool result = tourRequestService.SaveTourRequest(tourRequest);

            Guest2TourRequestCreationSucces guest2TourRequestCreationSucces = new Guest2TourRequestCreationSucces();
            guest2TourRequestCreation.NavigationService.Navigate(guest2TourRequestCreationSucces);

        }
    }
}
