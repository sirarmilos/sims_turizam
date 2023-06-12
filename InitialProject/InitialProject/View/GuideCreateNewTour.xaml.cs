using InitialProject.Dto;
using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.IO;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for GuideCreateNewTour.xaml
    /// </summary>
    public partial class GuideCreateNewTour : Window
    {

        private int currentRow = 1;
        
        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }
        private string image;

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

        private string tourName;
        private string country;
        private string city;
        private string address;
        private decimal latitude;
        private decimal longitude;
        private int maxGuests;
        private Language languages;
        private string description;
        private int duration;

        private string keyPointName;
        private string keyPointCountry;
        private string keyPointCity;
        private string keyPointAddress;
        private DateTime tourDate;

        public string TourName
        {
            get { return tourName; }
            set
            {
                tourName = value;
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

        public int MaxGuests
        {
            get { return maxGuests; }
            set
            {
                maxGuests = value;
                OnPropertyChanged();
            }
        }

        public Language Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged();
            }

        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged();
            }
        }

        public string KeyPointName
        {
            get { return keyPointName; }
            set
            {
                keyPointName = value;
                OnPropertyChanged();
            }
        }

        public string KeyPointCountry
        {
            get { return keyPointCountry; }
            set
            {
                keyPointCountry = value;
                OnPropertyChanged();
            }
        }

        public string KeyPointCity
        {
            get { return keyPointCity; }
            set
            {
                keyPointCity = value;
                OnPropertyChanged();
            }
        }

        public string KeyPointAddress
        {
            get { return keyPointAddress; }
            set
            {
                keyPointAddress = value;
                OnPropertyChanged();
            }
        }

        public List<DateTime> TourDates
        {
            get;
            set;
        }

        public DateTime TourDate
        {
            get { return tourDate; }
            set
            {
                tourDate = value;
                OnPropertyChanged();
            }
        }

        public Location Location
        {
            get;
            set;
        }

        public List<Location> KeyPointLocation
        {
            get;
            set;
        }

        public TourRequest TourRequest { get; set; }


        private List<TourKeyPointDto> tourKeyPointDtos;
        private List<TourGuidenceDto> tourGuidenceDtos;
        private List<TourKeyPoint> tourKeyPoints;
        private List<TourGuidence> tourGuidences;

        public List<string> Years
        {
            get;
            set;
        }

        public string SelectedYear
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GuideCreateNewTour(string guideUsername)
        {
            InitializeComponent();
            DataContext = this;
            Guide = guideUsername;
            Images = new ObservableCollection<string>();
            SelectedImage = string.Empty;
            SetDefaultValue();

            LocationRepository locationRepository = new LocationRepository();
            TourRepository tourRepository = new TourRepository();
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();

            tourKeyPointDtos = new List<TourKeyPointDto>();
            tourKeyPoints = new List<TourKeyPoint>();
            tourGuidences = new List<TourGuidence>();
            tourGuidenceDtos = new List<TourGuidenceDto>();

            TourDates = new List<DateTime>();
            KeyPointLocation = new List<Location>();
            Location = null;

            Years = new List<string>();
            Years.Add("ENGLISH");
            Years.Add("SERBIAN");
            Years.Add("GERMAN");
            Years.Add("MANDARINSKI");
            SelectedYear = "ENGLISH";
        }

        public GuideCreateNewTour(string guide, TourRequest tourRequest, DateTime selectedStartByGuide)
        {
            InitializeComponent();
            DataContext = this;
            Guide = guide;
            TourRequest = tourRequest;
            Images = new ObservableCollection<string>();
            SelectedImage = string.Empty;
            SetDefaultValue();

            LocationRepository locationRepository = new LocationRepository();
            TourRepository tourRepository = new TourRepository();
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();

            tourKeyPointDtos = new List<TourKeyPointDto>();
            tourKeyPoints = new List<TourKeyPoint>();
            tourGuidences = new List<TourGuidence>();
            tourGuidenceDtos = new List<TourGuidenceDto>();

            TourDates = new List<DateTime>();
            KeyPointLocation = new List<Location>();
            Location = null;

            Country = TourRequest.Location.Country;
            City = TourRequest.Location.City;
            tbCountry.IsReadOnly = true;
            tbCity.IsReadOnly = true;
            tbCity.Text = TourRequest.Location.City;
            tbCountry.Text = TourRequest.Location.Country;
            

            //Languages = TourRequest.Language;
            //tbLanguages.Text = TourRequest.Language.ToString();
            //tbLanguages.IsReadOnly = true;

            MaxGuests = TourRequest.GuestNumber;
            tbMaxGuests.Text = TourRequest.GuestNumber.ToString();
            tbMaxGuests.IsReadOnly = true;

            TourDate = selectedStartByGuide;
            date.Text = selectedStartByGuide.ToString();

            Location = tourRequest.Location;

            Years = new List<string>();
            Years.Add("ENGLISH");
            Years.Add("SERBIAN");
            Years.Add("GERMAN");
            Years.Add("MANDARINSKI");
            SelectedYear = TourRequest.Language.ToString();
            cb1.IsEnabled = false;
        }

        public GuideCreateNewTour(string guide, Location location)
        {
            InitializeComponent();
            DataContext = this;
            Guide = guide;
            Location = location;
            Images = new ObservableCollection<string>();
            SelectedImage = string.Empty;
            SetDefaultValue();

            LocationRepository locationRepository = new LocationRepository();
            TourRepository tourRepository = new TourRepository();
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();

            tourKeyPointDtos = new List<TourKeyPointDto>();
            tourKeyPoints = new List<TourKeyPoint>();
            tourGuidences = new List<TourGuidence>();
            tourGuidenceDtos = new List<TourGuidenceDto>();

            TourDates = new List<DateTime>();
            KeyPointLocation = new List<Location>();
            //Location = null;

            Country = Location.Country;
            City = Location.City;
            tbCountry.IsReadOnly = true;
            tbCity.IsReadOnly = true;
            tbCity.Text = Location.City;
            tbCountry.Text = Location.Country;
            date.Text = DateTime.Now.Date.ToString();

            Years = new List<string>();
            Years.Add("ENGLISH");
            Years.Add("SERBIAN");
            Years.Add("GERMAN");
            Years.Add("MANDARINSKI");
            SelectedYear = "ENGLISH";
        }

        public GuideCreateNewTour(string guide, Language language)
        {
            InitializeComponent();
            DataContext = this;
            Guide = guide;
            Images = new ObservableCollection<string>();
            SelectedImage = string.Empty;
            SetDefaultValue();

            LocationRepository locationRepository = new LocationRepository();
            TourRepository tourRepository = new TourRepository();
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();

            tourKeyPointDtos = new List<TourKeyPointDto>();
            tourKeyPoints = new List<TourKeyPoint>();
            tourGuidences = new List<TourGuidence>();
            tourGuidenceDtos = new List<TourGuidenceDto>();

            TourDates = new List<DateTime>();
            KeyPointLocation = new List<Location>();
            Location = null;

            //Languages = language;
            //tbLanguages.IsReadOnly = true;
            //tbLanguages.Text = Languages.ToString();
            date.Text = DateTime.Now.Date.ToString();

            Years = new List<string>();
            Years.Add("ENGLISH");
            Years.Add("SERBIAN");
            Years.Add("GERMAN");
            Years.Add("MANDARINSKI");
            SelectedYear = language.ToString();
            cb1.IsEnabled = false;
        }

        private void SetDefaultValue()
        {
            Images = new ObservableCollection<string>();
            dgImages.ItemsSource = Images;
            buttonRemoveImage.IsEnabled = false;

            tbName.Text = string.Empty;
            tbCountry.Text = string.Empty;
            tbCity.Text = string.Empty;
            tbAddress.Text = string.Empty;
            sliderLatitude.Value = 0;
            sliderLongitude.Value = 0;
            tbMaxGuests.Text = string.Empty;
            //tbLanguages.Text = string.Empty;
            //tbLanguages.Text = "ALL";
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

        private void GoToMostPopularTour(object sender, RoutedEventArgs e)
        {
            ShowMostPopularTour window = new ShowMostPopularTour();
            window.Show();
            Close();
        }

        private void GoToAllTourOccurences(object sender, RoutedEventArgs e)
        {
            AllTourOccurences window = new AllTourOccurences();
            window.Show();
            Close();
        }

        private void GoToHomePage(object sender, RoutedEventArgs e)
        {
            TourGuidenceService tourGuidenceService = new TourGuidenceService();

            TourGuidence tg = tourGuidenceService.CheckIfStartedAndNotFinished();
            if (tg != null)
            {
                GuideStart2 window = new GuideStart2(Guide, tg);
                window.Show();
                Close();
            }
            else
            {
                GuideStart1 window = new GuideStart1(Guide);
                window.Show();
                Close();
            }
        }

        private void PlaceNewKeyPoint(object sender, RoutedEventArgs e)
        {
            // Add new RowDefinition with specific height
            textBoxContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });

            // Add new box3-like TextBox
            TextBox newBox = new TextBox
            {
                HorizontalAlignment = box3.HorizontalAlignment,
                VerticalAlignment = box3.VerticalAlignment,
                Width = box3.Width,
                Height = box3.Height,
                IsReadOnly = box3.IsReadOnly,
                Margin = box3.Margin
            };

            Grid.SetRow(newBox, currentRow);
            textBoxContainer.Children.Add(newBox);

            // Add new labels
            Label[] newLabels = new Label[4];
            string[] labelContents = new string[] { "Name:", "Country:", "City:", "Address:" };

            for (int i = 0; i < newLabels.Length; i++)
            {
                newLabels[i] = new Label
                {
                    Content = labelContents[i],
                    HorizontalAlignment = lab1.HorizontalAlignment,
                    VerticalAlignment = lab1.VerticalAlignment,
                    Width = lab1.Width,
                    Height = lab1.Height,
                    Margin = new Thickness(30, i * 30, 0, 0) // Update the margin to position the labels properly
                };

                Grid.SetRow(newLabels[i], currentRow);
                textBoxContainer.Children.Add(newLabels[i]);
            }

            // Add new textboxes
            TextBox[] newTextboxes = new TextBox[4];

            for (int i = 0; i < newTextboxes.Length; i++)
            {
                newTextboxes[i] = new TextBox
                {
                    HorizontalAlignment = tx1.HorizontalAlignment,
                    TextWrapping = tx1.TextWrapping,
                    VerticalAlignment = tx1.VerticalAlignment,
                    Width = tx1.Width,
                    Margin = new Thickness(180, i * 30, 0, 0) // Update the margin to position the textboxes properly
                };

                Grid.SetRow(newTextboxes[i], currentRow);
                textBoxContainer.Children.Add(newTextboxes[i]);
            }

            currentRow++;
        }

        private void OnUpload(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpb;*.jpeg)|*.png;*.jpg;*.jpeg";
            if(openFileDialog.ShowDialog() == true)
            {
                string selectedImagepath = openFileDialog.FileName;
                string destinationFolderPath = @"C:\Users\PC\Documents\GitHub\sims_turizam\InitialProject\InitialProject\Resources\Images";
                string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, System.IO.Path.GetFileName(selectedImagepath));

                File.Copy(selectedImagepath, destinationFilePath, true);
                MessageBox.Show("Image successfully added");


            }
        }

        private void ChooseImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                Image = openFileDialog.FileName;
                tbImage.Text = openFileDialog.FileName;
                tbImage.Focus();
            }
        }

        private void AddImageToList(object sender, RoutedEventArgs e)
        {
            if (CheckUrlExists() == false)
            {
                MessageBox.Show("The image with the specified url does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (CheckImageExist() == false)
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
            return (Uri.TryCreate(Image.ToString(), UriKind.Absolute, out Uri checkUri) && (checkUri.Scheme == Uri.UriSchemeHttp || checkUri.Scheme == Uri.UriSchemeHttps)) || CheckImageExtension();
        }

        public bool CheckImageExtension()
        {
            string imageExtension = Image.Substring(Image.Length - 4);
            return (imageExtension.Equals(".png") == true) || (imageExtension.Equals(".jpg") == true) || (imageExtension.Equals(".jpeg") == true) || (imageExtension.Equals(".jpe") == true) || (imageExtension.Equals(".bmp") == true) || (imageExtension.Equals(".gif") == true);
        }

        private bool CheckImageExist()
        {
            return Images.Any(x => x.Equals(Image) == true);
        }

        void LoadingRowForDgImages(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private bool CheckErrorImagesNumber()
        {
            return !(Images.Count <= 0);
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

        private void SliderLatitudeValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Latitude = Math.Round((decimal)sliderLatitude.Value, 1);
        }

        private void SliderLongitudeValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Longitude = Math.Round((decimal)sliderLongitude.Value, 1);
        }

        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var label = (Label)sender;
                Keyboard.Focus(label.Target);
            }
        }

        private void AllowOnlyDigits(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void AllowOnlyCharacters(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, 0) && !char.IsWhiteSpace(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void SaveTour(object sender, RoutedEventArgs e)
        {

            if (CheckErrorAllFieldsFilled() == true)
            {
                MessageBox.Show("You must fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (CheckErrorImagesNumber() == false)
            {
                MessageBox.Show("You must enter at least one tour image.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                tbImage.Focus();
            }
            else
            {
                Location location = new Location();
                LocationRepository locationRepository = new LocationRepository();
                TourRepository tourRepository = new TourRepository();
                TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();
                TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();

                if (Location != null)
                {
                    location = Location;
                }
                else
                {
                    LocationDto locationDto = new LocationDto(Country, City, Address, Latitude, Longitude);
                    location = locationRepository.Save(locationDto);
                }


                TourDto tourDto = new TourDto(TourName, location, Description, (Language)Enum.Parse(typeof(Language), SelectedYear), maxGuests, Duration, Images.ToList(), Guide);
                Tour tour = tourRepository.Save(tourDto);


                foreach (TourGuidenceDto t in tourGuidenceDtos)
                {
                    TourGuidence tourGuidence = new(tourGuidenceRepository.NextId(), tour, t.StartTime, false, false, false);
                    //t.Tour = tour;
                    //tourGuidenceRepository.Update(t);
                    tourGuidenceRepository.SaveToFile(tourGuidence);
                    foreach (TourKeyPointDto tkp in tourKeyPointDtos)
                    {
                        TourKeyPoint tourKeyPoint = new(tourKeyPointRepository.NextId(), tkp.TourKeyPointName, tkp.Location, tourGuidence, false);
                        //tkp.TourGuidence = t;
                        tourKeyPointRepository.SaveToFile(tourKeyPoint);
                    }
                }

                //SetDefaultValue();

                MessageBox.Show("New tour has been successfully added.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                GuideCreateNewTour window = new GuideCreateNewTour(Guide);
                window.Show();
                Close();
                //Close();
            }
        }

        private void AddDateToList(object sender, RoutedEventArgs e)
        {

            TourGuidenceDto tgDTO = new(TourDate);
            tourGuidenceDtos.Add(tgDTO);
        }

        private void AddTourKeyPoints(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(KeyPointName) || string.IsNullOrEmpty(KeyPointCountry) || string.IsNullOrEmpty(KeyPointCity) || string.IsNullOrEmpty(KeyPointAddress))
            {
                MessageBox.Show("Fill data for key points.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LocationRepository locationRepository = new LocationRepository();
            LocationDto locationDto = new(KeyPointCountry, KeyPointCity, KeyPointAddress, 5, 5);
            Location location = locationRepository.Save(locationDto);

            TourKeyPointDto tourKeyPointDto = new(KeyPointName, location);
            tourKeyPointDtos.Add(tourKeyPointDto);

        }

        private bool CheckErrorAllFieldsFilled()
        {
            return string.IsNullOrEmpty(TourName) || string.IsNullOrEmpty(Country) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Address) || MaxGuests<=0  || Duration <= 0 || string.IsNullOrEmpty(Languages.ToString()) || string.IsNullOrEmpty(Description) || tourGuidenceDtos.Count==0 || tourKeyPointDtos.Count==0 ; // || !MinDaysReservation.HasValue || !LeftCancelationDays.HasValue;
        }

        private void GoToCreateNewTour(object sender, RoutedEventArgs e)
        {
            GuideCreateNewTour window = new GuideCreateNewTour(Guide);
            window.Show();
            Close();
        }

        private void GoToTourRequests(object sender, RoutedEventArgs e)
        {
            SearchAndShowTourRequests window = new SearchAndShowTourRequests(Guide);
            window.Show();
            Close();
        }
    }
}
