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
using System.IO;
using InitialProject.Service;
using InitialProject.DTO;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AddNewAccommodation.xaml
    /// </summary>
    public partial class AddNewAccommodation : Window
    {
        private readonly AccommodationService accommodationService;

        public string OwnerUsername
        {
            get;
            set;
        }

        private string accommodationName;
        private string country;
        private string city;
        private string address;
        private decimal latitude;
        private decimal longitude;
        private bool apartment;
        private bool home;
        private bool hut;
        private string type;
        private int? maxGuests;
        private int? minDaysReservation;
        private int? leftCancelationDays;
        private string image;

        private string minDaysReservationCheck;
        private string leftCancelationDaysCheck;

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

        public int? LeftCancelationDays
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

        public ObservableCollection<string> Images
        {
            get;
            set;
        }

        public string SelectedImage
        {
            get;
            set;
        }

        public string MinDaysReservationCheck
        {
            get { return minDaysReservationCheck; }
            set
            {
                minDaysReservationCheck = value;
                OnPropertyChanged();
            }
        }

        public string LeftCancelationDaysCheck
        {
            get { return leftCancelationDaysCheck; }
            set
            {
                leftCancelationDaysCheck = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddNewAccommodation(string ownerUsername)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            accommodationService = new AccommodationService();

            Images = new ObservableCollection<string>();

            SelectedImage = string.Empty;

            SetDefaultValue();
        }

        private void SaveAccommodation(object sender, RoutedEventArgs e)
        {
            if (CheckErrorAllFieldsFilled() == true)
            {
                MessageBox.Show("You must fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(CheckErrorImagesNumber() == false)
            {
                MessageBox.Show("You must enter at least one accommodation image.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                tbImage.Focus();
            }
            else if(accommodationService.IsAccommodationNameExist(AccommodationName) == true)
            {
                MessageBox.Show("Accommodation with this name already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                Name = string.Empty;
                tbName.Focus();
            }
            else
            {
                SaveNewAccommodationDTO saveNewAccommodationDTO = new SaveNewAccommodationDTO(AccommodationName, OwnerUsername, Country, City, Address, Latitude, Longitude, Type, (int)MaxGuests, (int)MinDaysReservation, (int)LeftCancelationDays, Images);

                accommodationService.SaveNewAccommodation(saveNewAccommodationDTO);

                SetDefaultValue();

                MessageBox.Show("New accommodation has been successfully added.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CheckErrorAllFieldsFilled()
        {
            return string.IsNullOrEmpty(AccommodationName) || string.IsNullOrEmpty(Country) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Address) || !MaxGuests.HasValue || !MinDaysReservation.HasValue || !LeftCancelationDays.HasValue;
        }

        private bool CheckErrorImagesNumber()
        {
            return !(Images.Count <= 0);
        }

        private void SetDefaultValue()
        {
            LeftCancelationDaysCheck = "1";
            tbName.Text = string.Empty;
            tbCountry.Text = string.Empty;
            tbCity.Text = string.Empty;
            tbAddress.Text = string.Empty;
            sliderLatitude.Value = 0;
            sliderLongitude.Value = 0;
            rbApartment.IsChecked = true;
            rbHome.IsChecked = false;
            rbHut.IsChecked = false;
            tbMaxGuests.Text = string.Empty;
            tbMinDaysReservation.Text = string.Empty;
            tbLeftCancelationDays.Text = "1";
            Images = new ObservableCollection<string>();
            dgImages.ItemsSource = Images;
            buttonRemoveImage.IsEnabled = false;
        }

        private void AddImageToList(object sender, RoutedEventArgs e)
        {
            if(CheckUrlExists() == false)
            {
                MessageBox.Show("The image with the specified url does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(CheckImageExist() == false)
            {
                Images.Add(Image.ToString());
            }
            else
            {
                MessageBox.Show("You have already added this image.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            tbImage.Text = string.Empty;
            tbImage.Focus();
        }

        private bool CheckUrlExists()
        {
            return Uri.TryCreate(Image.ToString(), UriKind.Absolute, out Uri checkUri) && (checkUri.Scheme == Uri.UriSchemeHttp || checkUri.Scheme == Uri.UriSchemeHttps);
        }
        private bool CheckImageExist()
        {
            return Images.Any(x => x.Equals(Image) == true);
        }

        private void RemoveImageFromList(object sender, RoutedEventArgs e)
        {
            Images.Remove(SelectedImage);
        }

        private void RemoveImageButtonEnable(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedImage == null)
            {
                buttonRemoveImage.IsEnabled = false;
            }
            else
            {
                buttonRemoveImage.IsEnabled = true;
            }
        }

        private void CheckErrorMinDaysReservation(object sender, TextChangedEventArgs e)
        {
            if (MinDaysReservationCheck.Equals(string.Empty) == false)
            {
                int checkOut;
                bool check = int.TryParse(MinDaysReservationCheck, out checkOut);

                if (check == false || checkOut <= 0)
                {
                    MessageBox.Show("Minimum number of days for reservation must be an integer greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    tbMinDaysReservation.Clear();
                    MinDaysReservationCheck = string.Empty;
                }
                else
                {
                    MinDaysReservation = Convert.ToInt32(MinDaysReservationCheck.ToString());
                }
            }
        }

        private void CheckErrorLeftCancelationDays(object sender, TextChangedEventArgs e)
        {
            if (LeftCancelationDaysCheck.Equals(string.Empty) == false)
            {
                int checkOut;
                bool check = int.TryParse(LeftCancelationDaysCheck, out checkOut);

                if (check == false || checkOut <= 0)
                {
                    MessageBox.Show("Number of days before the reservation until which it is possible to cancel it must be an integer greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    tbLeftCancelationDays.Clear();
                    LeftCancelationDaysCheck = "1";
                    tbLeftCancelationDays.Text = "1";
                }
                else
                {
                    LeftCancelationDays = Convert.ToInt32(LeftCancelationDaysCheck.ToString());
                }
            }
        }

        void LoadingRowForDgImages(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 1)
            {
                var label = (Label)sender;
                Keyboard.Focus(label.Target);
            }
        }

        private void SliderLatitudeValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Latitude = Math.Round((decimal)sliderLatitude.Value, 2);
        }

        private void SliderLongitudeValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Longitude = Math.Round((decimal)sliderLongitude.Value, 2);
        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
