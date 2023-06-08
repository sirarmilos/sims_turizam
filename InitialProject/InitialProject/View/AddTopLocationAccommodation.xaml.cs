using InitialProject.DTO;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AddTopLocationAccommodation.xaml
    /// </summary>
    public partial class AddTopLocationAccommodation : Window
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

        public AddTopLocationAccommodation(string ownerUsername, string mostPopularLocation)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;
            Country = mostPopularLocation.Split(", ")[0];
            City = mostPopularLocation.Split(", ")[1];

            DataContext = this;

            accommodationService = new AccommodationService();

            Images = new ObservableCollection<string>();

            SelectedImage = string.Empty;

            SetDefaultValue();

            tbAccommodationName.Focus();
        }

        private void SetDefaultValue()
        {
            LeftCancelationDaysCheck = "1";
            tbAccommodationName.Text = string.Empty;
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

            textBlockErrorAccommodationName1.Visibility = Visibility.Hidden;
            textBlockErrorMaxGuests1.Visibility = Visibility.Hidden;
            textBlockErrorMinDaysReservation1.Visibility = Visibility.Hidden;
            textBlockErrorLeftCancelationDays1.Visibility = Visibility.Hidden;
            textBlockErrorImage1.Visibility = Visibility.Hidden;
            textBlockErrorImage2.Visibility = Visibility.Hidden;
            textBlockErrorImage3.Visibility = Visibility.Hidden;
        }

        private void ChooseImage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ChooseImage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                Image = openFileDialog.FileName;
                tbImage.Text = openFileDialog.FileName;
                buttonAddImage.Focus();
            }
        }

        private void AddImage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbImage.Text) == true)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void AddImage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckUrlExists() == false)
            {
                textBlockErrorImage1.Visibility = Visibility.Visible;
            }
            else if (CheckImageExist() == false)
            {
                Images.Add(Image.ToString());
                tbImage.Text = string.Empty;
                textBlockErrorImage0.Visibility = Visibility.Hidden;
            }
            else
            {
                textBlockErrorImage2.Visibility = Visibility.Visible;
            }

            tbImage.Focus();
        }

        private bool CheckUrlExists()
        {
            return (Uri.TryCreate(Image.ToString(), UriKind.Absolute, out Uri checkUri) && (checkUri.Scheme == Uri.UriSchemeHttp || checkUri.Scheme == Uri.UriSchemeHttps)) || CheckImageExtension();
        }

        public bool CheckImageExtension()
        {
            if (Image.Length < 5)
            {
                return false;
            }

            string imageExtension = Image.Substring(Image.Length - 4);
            return (imageExtension.Equals(".png") == true) || (imageExtension.Equals(".jpg") == true) || (imageExtension.Equals(".jpeg") == true) || (imageExtension.Equals(".jpe") == true) || (imageExtension.Equals(".bmp") == true) || (imageExtension.Equals(".gif") == true);
        }

        private bool CheckImageExist()
        {
            return Images.Any(x => x.Equals(Image) == true);
        }

        private void RemoveImage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedImage == null)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void RemoveImage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Images.Remove(SelectedImage);

            if (Images.Count == 0)
            {
                textBlockErrorImage0.Visibility = Visibility.Visible;
            }
        }

        private void SaveAccommodation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveAccommodation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckErrorAllFieldsFilled() == true)
            {
                MessageBox.Show("You must fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (CheckErrorImagesNumber() == false) //
            {
                textBlockErrorImage3.Visibility = Visibility.Visible;
                tbImage.Focus();
            }
            else if (accommodationService.IsAccommodationNameExist(AccommodationName) == true)
            {
                textBlockErrorAccommodationName1.Visibility = Visibility.Visible;
                tbAccommodationName.Focus();
            }
            else
            {
                SaveNewAccommodationDTO saveNewAccommodationDTO = new SaveNewAccommodationDTO(AccommodationName, OwnerUsername, Country, City, Address, Latitude, Longitude, Type, (int)MaxGuests, (int)MinDaysReservation, (int)LeftCancelationDays, Images);

                accommodationService.SaveNewAccommodation(saveNewAccommodationDTO);

                SetDefaultValue();

                MessageBox.Show("New accommodation has been successfully added.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
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

        private void Cancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Cancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
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

        private void CheckErrorAccommodationName(object sender, TextChangedEventArgs e)
        {
            textBlockErrorAccommodationName1.Visibility = Visibility.Hidden;

            if (AccommodationName.Equals(string.Empty) == true)
            {
                textBlockErrorAccommodationName0.Visibility = Visibility.Visible;
            }
            else
            {
                textBlockErrorAccommodationName0.Visibility = Visibility.Hidden;
            }
        }

        private void CheckErrorAddress(object sender, TextChangedEventArgs e)
        {
            if (Address.Equals(string.Empty) == true)
            {
                textBlockErrorAddress.Visibility = Visibility.Visible;
            }
            else
            {
                textBlockErrorAddress.Visibility = Visibility.Hidden;
            }
        }

        private void CheckErrorMaxGuests(object sender, TextChangedEventArgs e)
        {
            if (MaxGuestsCheck.Equals(string.Empty) == true)
            {
                textBlockErrorMaxGuests0.Visibility = Visibility.Visible;
                textBlockErrorMaxGuests1.Visibility = Visibility.Hidden;
            }
            else
            {
                textBlockErrorMaxGuests0.Visibility = Visibility.Hidden;

                int checkOut;
                bool check = int.TryParse(MaxGuestsCheck, out checkOut);

                if (check == false || checkOut <= 0)
                {
                    textBlockErrorMaxGuests1.Visibility = Visibility.Visible;
                }
                else
                {
                    textBlockErrorMaxGuests1.Visibility = Visibility.Hidden;
                    MaxGuests = Convert.ToInt32(MaxGuestsCheck.ToString());
                }
            }
        }

        private void CheckErrorMinDaysReservation(object sender, TextChangedEventArgs e)
        {
            if (MinDaysReservationCheck.Equals(string.Empty) == true)
            {
                textBlockErrorMinDaysReservation0.Visibility = Visibility.Visible;
                textBlockErrorMinDaysReservation1.Visibility = Visibility.Hidden;
            }
            else
            {
                textBlockErrorMinDaysReservation0.Visibility = Visibility.Hidden;

                int checkOut;
                bool check = int.TryParse(MinDaysReservationCheck, out checkOut);

                if (check == false || checkOut <= 0)
                {
                    textBlockErrorMinDaysReservation1.Visibility = Visibility.Visible;
                }
                else
                {
                    textBlockErrorMinDaysReservation1.Visibility = Visibility.Hidden;
                    MinDaysReservation = Convert.ToInt32(MinDaysReservationCheck.ToString());
                }
            }
        }

        private void CheckErrorLeftCancelationDays(object sender, TextChangedEventArgs e)
        {
            if (LeftCancelationDaysCheck.Equals(string.Empty) == true)
            {
                textBlockErrorLeftCancelationDays0.Visibility = Visibility.Visible;
                textBlockErrorLeftCancelationDays1.Visibility = Visibility.Hidden;
            }
            else
            {
                textBlockErrorLeftCancelationDays0.Visibility = Visibility.Hidden;

                int checkOut;
                bool check = int.TryParse(LeftCancelationDaysCheck, out checkOut);

                if (check == false || checkOut <= 0)
                {
                    textBlockErrorLeftCancelationDays1.Visibility = Visibility.Visible;
                }
                else
                {
                    textBlockErrorLeftCancelationDays1.Visibility = Visibility.Hidden;
                    LeftCancelationDays = Convert.ToInt32(LeftCancelationDaysCheck.ToString());
                }
            }
        }

        private void CheckErrorImage(object sender, TextChangedEventArgs e)
        {
            textBlockErrorImage1.Visibility = Visibility.Hidden;
            textBlockErrorImage2.Visibility = Visibility.Hidden;
            textBlockErrorImage3.Visibility = Visibility.Hidden;
        }

        private void SliderLatitudeValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Latitude = Math.Round((decimal)sliderLatitude.Value, 2);
        }

        private void SliderLongitudeValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Longitude = Math.Round((decimal)sliderLongitude.Value, 2);
        }

        void LoadingRowForDgImages(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var label = (Label)sender;
                Keyboard.Focus(label.Target);
            }
        }
    }
}
