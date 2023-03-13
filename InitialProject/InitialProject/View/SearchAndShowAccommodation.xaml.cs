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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for SearchAndShowAccommodation.xaml
    /// </summary>
    public partial class SearchAndShowAccommodation : Window
    {
        public Accommodation Accommodation { get; set; }

        private AccommodationRepository accommodationRepository;

        private string accommodationName;
        private string country;
        private string city;
        private string type;
        private int maxGuests;
        private int minDaysReservation;
        private int leftCancelationDays;
        private string image;
        private List<string> images;

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
                //Validate();

            }
        }

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));
                //Validate();
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

        private bool apartment;
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

        private bool home;
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

        private bool hut;
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


        /*public bool IsValid { get; private set; } = false;

        private void Validate()
        {
            if (!string.IsNullOrEmpty(City) && string.IsNullOrEmpty(Country))
            {
                IsValid = false;
            }
            else if (string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(Country))
            {
                IsValid = false;
            }
            else
            {
                IsValid = true;
            }
        }*/

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public SearchAndShowAccommodation()
        {
            InitializeComponent();
            DataContext = this;
            accommodationRepository = new AccommodationRepository();
            Images = new List<string>();
        }


        private void Search(object sender, RoutedEventArgs e)
        {
            listAccommodations.ItemsSource = accommodationRepository.FindAll(AccommodationName, Country, City, Type, MaxGuests, MinDaysReservation);
        }

       
    }

}
