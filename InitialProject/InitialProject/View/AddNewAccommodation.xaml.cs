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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AddNewAccommodation.xaml
    /// </summary>
    public partial class AddNewAccommodation : Window
    {
        public Accommodation Accommodation { get; set; }

        private readonly AccommodationRepository accommodationRepository;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
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
        private List<string> images;

        private string maxGuestsCheck;
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

        public List<string> Images
        {
            get;
            set;
        }

        public ObservableCollection<ImagesView> ImagesView
        {
            get;
            set;
        }

        public ImagesView SelectedImage
        {
            get;
            set;
        }

        public string MaxGuestsCheck
        {
            get { return maxGuestsCheck; }
            set
            {
                maxGuestsCheck = value;
                OnPropertyChanged();
            }
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

        public AddNewAccommodation(string owner)
        {
            InitializeComponent(); 
            Owner = owner;
            DataContext = this;
            accommodationRepository = new AccommodationRepository();
            Images = new List<string>();
            ImagesView = new ObservableCollection<ImagesView>();
            SelectedImage = null;
            rbApartment.IsChecked = true;
            LeftCancelationDaysCheck = "1";
            tbLeftCancelationDays.Text = "1";
        }

        private void SaveAccommodation(object sender, RoutedEventArgs e)
        {
            if (CheckErrorAllFieldsFilled() == false)
            {
                MessageBox.Show("You must fill in all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(CheckErrorImagesNumber() == false)
            {
                MessageBox.Show("You must enter at least one accommodation image", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbImage.Focus();
            }
            else
            {
                if(accommodationRepository.Save(AccommodationName, Owner, Country, City, Address, Latitude, Longitude, Type, (int)MaxGuests, (int)MinDaysReservation, (int)LeftCancelationDays, Images) == true)
                {
                    ClearAllFields();
                }
                else
                {
                    Name = "";
                    tbName.Focus();
                }
            }
        }

        private void AddImageToList(object sender, RoutedEventArgs e)
        {
            if(CheckErrorUrlExists() == true)
            {
                if (CheckErrorImageAlreadyExists() == true)
                {
                    Images.Add(Image.ToString());
                    ImagesView.Add(new ImagesView { Id = Guid.NewGuid().ToString(), ImageUrl = Image.ToString() });//
                }
                else
                {
                    MessageBox.Show("You have already added this image.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("The image with the specified url does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            tbImage.Text = "";
            tbImage.Focus();
        }

        private void RemoveImageFromList(object sender, RoutedEventArgs e)
        {
            dgImages.Items.Refresh();

            if(SelectedImage == null)
            {
                MessageBox.Show("Select the image you want to remove", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (Images.Count > 0)
                {
                    SelectedImage = (ImagesView)dgImages.SelectedItem;
                    Images.Remove(SelectedImage.ImageUrl);
                    ImagesView.Remove(SelectedImage);
                }
                else
                {
                    MessageBox.Show("There are currently no added images that you can remove", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CheckErrorMaxGuests(object sender, TextChangedEventArgs e)
        {
            if (MaxGuestsCheck.Equals("") == false)
            {
                int checkOut;
                bool check = int.TryParse(MaxGuestsCheck, out checkOut);

                if (check == false || checkOut <= 0)
                {
                    MessageBox.Show("Maximum number of guests must be an integer greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    tbMaxGuests.Clear();
                    MaxGuestsCheck = "";
                }
                else
                {
                    MaxGuests = Convert.ToInt32(MaxGuestsCheck.ToString());
                }
            }
        }

        private void CheckErrorMinDaysReservation(object sender, TextChangedEventArgs e)
        {
            if (MinDaysReservationCheck.Equals("") == false)
            {
                int checkOut;
                bool check = int.TryParse(MinDaysReservationCheck, out checkOut);

                if (check == false || checkOut <= 0)
                {
                    MessageBox.Show("Minimum number of days for reservation must be an integer greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    tbMinDaysReservation.Clear();
                    MinDaysReservationCheck = "";
                }
                else
                {
                    MinDaysReservation = Convert.ToInt32(MinDaysReservationCheck.ToString());
                }
            }
        }

        private void CheckErrorLeftCancelationDays(object sender, TextChangedEventArgs e)
        {
            if (LeftCancelationDaysCheck.Equals("") == false)
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

        private bool CheckErrorImagesNumber()
        {
            if(Images.Count <= 0)
            {
                return false;
            }

            return true;
        }

        private bool CheckErrorUrlExists()
        {
            Uri checkUri;
            bool check = Uri.TryCreate(Image.ToString(), UriKind.Absolute, out checkUri)
                && (checkUri.Scheme == Uri.UriSchemeHttp || checkUri.Scheme == Uri.UriSchemeHttps);

            return check;
        }
        private bool CheckErrorImageAlreadyExists()
        {
            foreach(string temporaryImage in Images)
            {
                if (temporaryImage.Equals(Image.ToString()) == true)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckErrorAllFieldsFilled()
        {
            if(tbName.Text.Length > 0 && tbCountry.Text.Length > 0 && tbCity.Text.Length > 0 && tbAddress.Text.Length > 0 && tbMaxGuests.Text.Length > 0 && tbMinDaysReservation.Text.Length > 0 && tbLeftCancelationDays.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private void ClearAllFields()
        {
            tbName.Text = "";
            tbCountry.Text = "";
            tbCity.Text = "";
            tbAddress.Text = "";
            sliderLatitude.Value = 0;
            sliderLongitude.Value = 0;
            rbApartment.IsChecked = true;
            rbHome.IsChecked = false;
            rbHut.IsChecked = false;
            tbMaxGuests.Text = "";
            tbMinDaysReservation.Text = "";
            tbLeftCancelationDays.Text = "";
            Images = new List<string>();
            ImagesView = new ObservableCollection<ImagesView>();
            dgImages.ItemsSource = ImagesView;
            dgImages.Items.Refresh();
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
    }
}
