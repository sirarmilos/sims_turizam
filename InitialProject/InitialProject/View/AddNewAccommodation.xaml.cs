using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AddNewAccommodation.xaml
    /// </summary>
    public partial class AddNewAccommodation : Window
    {
        public Accommodation Accommodation { get; set; }

        private readonly AccommodationRepository accommodationRepository;

        private string name;
        private string country;
        private string city;
        private string address;
        private decimal latitude;
        private decimal longitude;
        private string type;
        private int maxGuests;
        private int minDaysReservation;
        private int leftCancelationDays;
        private Location location;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }

        public decimal Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged();
            }
        }

        public decimal Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged();
            }
        }

        public int MaxGuests
        {
            get { return maxGuests; }
            set
            {
                maxGuests = value;
                OnPropertyChanged();
            }
        }

        public int MinDaysReservation
        {
            get { return minDaysReservation; }
            set
            {
                minDaysReservation = value;
                OnPropertyChanged();
            }
        }

        public int LeftCancelationDays
        {
            get { return leftCancelationDays; }
            set
            {
                leftCancelationDays = value;
                OnPropertyChanged();
            }
        }

        public Location Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddNewAccommodation()
        {
            InitializeComponent();
            DataContext = this;
            accommodationRepository = new AccommodationRepository();
        }

        private void SaveAccommodation(object sender, RoutedEventArgs e)
        {
            /*if (Accommodation != null)
            {*/
                /*Accommodation.Name = Name;
                Accommodation.Location.Country = Country;
                Accommodation.Location.City = City;
                Accommodation.Location.Address = Address;
                Accommodation.Location.Latitude = Latitude;
                Accommodation.Location.Longitude = Longitude;
                Accommodation.Type = Type;
                Accommodation.MaxGuests = MaxGuests;
                Accommodation.MinDaysReservation = MinDaysReservation;
                Accommodation.LeftCancelationDays = LeftCancelationDays;*/
                // id u funkciji
                Accommodation newAccommodation = new Accommodation(Name, Country, City, Address, Latitude, Longitude, Type, MaxGuests, MinDaysReservation, LeftCancelationDays);
                //Accommodation newAccommodation = new Accommodation(Name, Country, City, Address, Latitude, Longitude, Type, MaxGuests, MinDaysReservation, LeftCancelationDays);

            Accommodation savedAccommodation = accommodationRepository.Save(newAccommodation);
            //string[] csvValues = { Id.ToString(), Name.ToString(), Location.Id.ToString(), Type.ToString(), MaxGuests.ToString(), MinDaysReservation.ToString(), LeftCancelationDays.ToString() };

            //}  
        }
    }
}
