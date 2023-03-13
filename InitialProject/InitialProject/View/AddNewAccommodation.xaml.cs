using InitialProject.Model;
using InitialProject.Repository;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AddNewAccommodation.xaml
    /// </summary>
    public partial class AddNewAccommodation : Window
    {
        public Accommodation Accommodation { get; set; }

        private readonly AccommodationRepository accommodationRepository;

        private string accommodationName;
        private string country;
        private string city;
        private string address;
        private decimal latitude;
        private decimal longitude;
        private string type;
        private int maxGuests;
        private int minDaysReservation;
        private int leftCancelationDays;
        private string image;
        private List<string> images;

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
            Images = new List<string>();
        }

        private void SaveAccommodation(object sender, RoutedEventArgs e)
        {
            accommodationRepository.Save(AccommodationName, Country, City, Address, Latitude, Longitude, Type, MaxGuests, MinDaysReservation, LeftCancelationDays, Images);
        }

        private void AddImageToList(object sender, RoutedEventArgs e)
        {
            Images.Add(Image.ToString());
            tbImage.Text = "";
        }

        private void CheckErrorMaxGuests(object sender, RoutedEventArgs e)
        {
            /*if(MaxGuests <= 0)
            {
                MessageBox.Show("Max Guests must be an integer greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbMaxGuests.Text = "1";
                tbMaxGuests.Focus();
            
            }*/
        }

    }
}
