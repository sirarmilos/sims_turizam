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
        private bool apartment;
        private bool home;
        private bool hut;
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
            ImagesView = new ObservableCollection<ImagesView>();
            SelectedImage = null;
            rbApartment.IsChecked = true;
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
                accommodationRepository.Save(AccommodationName, Country, City, Address, Latitude, Longitude, Type, MaxGuests, MinDaysReservation, LeftCancelationDays, Images);
            }
        }

        private void AddImageToList(object sender, RoutedEventArgs e)
        {
            //if(CheckErrorImageExists)
            Images.Add(Image.ToString());
            ImagesView.Add(new ImagesView { Id = Guid.NewGuid().ToString(), ImageUrl = Image.ToString() });
            tbImage.Text = "";
        }

        private void RemoveImageFromList(object sender, RoutedEventArgs e)
        {
            dgImages.Items.Refresh();

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

        private void CheckErrorLatitude(object sender, TextChangedEventArgs e)
        {
            if (Convert.ToDecimal(latitude) < -90 || Convert.ToDecimal(latitude) > 90)
            {
                MessageBox.Show("The latitude must be between -90 and 90", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbLatitude.Focus();
                tbLatitude.Text = "0";
            }
        }

        private void CheckErrorLongitude(object sender, TextChangedEventArgs e)
        {
            if (longitude < -180 || longitude > 180)
            {
                MessageBox.Show("The longitude must be between -180 and 180", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbLongitude.Focus();
                tbLongitude.Text = "0";
            }
        }

        private void CheckErrorMaxGuests(object sender, RoutedEventArgs e)
        {
            if (MaxGuests <= 0)
            {
                MessageBox.Show("Maximum number of guests must be an integer greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbMaxGuests.Focus();
                tbMaxGuests.Text = "1";
            }
        }

        private void CheckErrorMinDaysReservation(object sender, TextChangedEventArgs e)
        {
            if (MinDaysReservation <= 0)
            {
                MessageBox.Show("Minimum number of days for reservation must be an integer greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbMinDaysReservation.Focus();
                tbMinDaysReservation.Text = "1";
            }
        }

        private void CheckErrorLeftCancelationDays(object sender, TextChangedEventArgs e)
        {
            if(Convert.ToInt32(LeftCancelationDays) <= 0)
            {
                MessageBox.Show("Left cancelation days must be an integer greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbLeftCancelationDays.Focus();
                tbLeftCancelationDays.Text = "1";
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

        private bool CheckErrorAllFieldsFilled()
        {
            if(tbName.Text.Length > 0 && tbCountry.Text.Length > 0 && tbCity.Text.Length > 0 && tbAddress.Text.Length > 0 && tbLatitude.Text.Length > 0 && tbLongitude.Text.Length > 0 && tbMaxGuests.Text.Length > 0 && tbMinDaysReservation.Text.Length > 0 && tbLeftCancelationDays.Text.Length > 0)
            {
                return true;
            }

            return false;
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

    }
}
