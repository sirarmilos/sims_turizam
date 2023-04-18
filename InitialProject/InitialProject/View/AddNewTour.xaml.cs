using InitialProject.Dto;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AddNewTour.xaml
    /// </summary>
    public partial class AddNewTour : Window
    {
        private int tourKeyPointsIdCounter = -1;
        private int locationIdCounter = -1;
        public Tour Tour { get; set; }

        private readonly TourRepository tourRepository;

        private readonly LocationRepository locationRepository;

        private readonly TourKeyPointRepository tourKeyPointRepository;

        private readonly TourGuidenceRepository tourGuidenceRepository;

        private string tourName;
        private string tourCountry;
        private string tourCity;
        private string tourAddress;
        private decimal tourLatitude;
        private decimal tourLongitude;
        private string description;
        private Language languages;

        private int maxGuests;
        private string maxGuestsCheck;

        private List<TourKeyPoint> tourKeyPoints;

        private List<TourKeyPointDto> tourKeyPointDtos;

        private List<TourGuidence> tourGuidences;

        private List<TourGuidenceDto> tourGuidenceDtos;

        private TourKeyPoint tourKeyPoint;
        private string keyPointName;
        private string keyPointCountry;
        private string keyPointCity;
        private string keyPointAddress;
        private decimal keyPointLatitude;
        private decimal keyPointLongitude;
        private List<DateTime> tourDates;
        private DateTime tourDate;
        private int duration;
        private string image;
        private List<string> images;
        private List<Location> keyPointLocation;

        private int counter;

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }

        public string TourName
        {
            get { return tourName; }
            set
            {
                tourName = value;
                OnPropertyChanged();
            }
        }

        public string TourCountry
        {
            get { return tourCountry; }
            set
            {
                tourCountry = value;
                OnPropertyChanged();
            }
        }

        public string TourCity
        {
            get { return tourCity; }
            set
            {
                tourCity = value;
                OnPropertyChanged();
            }
        }

        public string TourAddress
        {
            get { return tourAddress; }
            set
            {
                tourAddress = value;
                OnPropertyChanged();
            }
        }

        public decimal TourLatitude
        {
            get { return tourLatitude; }
            set
            {
                tourLatitude = value;
                OnPropertyChanged();
            }
        }

        public decimal TourLongitude
        {
            get { return tourLongitude; }
            set
            {
                tourLongitude = value;
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

        public Language Languages
        {
            get { return languages; }
            set
            {
                languages = value;
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

        public List<TourKeyPoint> TourKeyPoints
        {
            get;
            set;
        }

        public TourKeyPoint TourKeyPoint
        {
            get { return tourKeyPoint; }
            set
            {
                tourKeyPoint = value;
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

        public decimal KeyPointLatitude
        {
            get { return keyPointLatitude; }
            set
            {
                keyPointLatitude = value;
                OnPropertyChanged();
            }
        }

        public decimal KeyPointLongitude
        {
            get { return keyPointLongitude; }
            set
            {
                keyPointLongitude = value;
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

        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
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

        public List<Location> KeyPointLocation
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



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public AddNewTour(string guide)
        {
            InitializeComponent();
            DataContext = this;

            Guide = guide;

            locationRepository = new LocationRepository();
            tourRepository = new TourRepository();
            tourKeyPointRepository = new TourKeyPointRepository();
            tourGuidenceRepository = new TourGuidenceRepository();

            tourKeyPointDtos = new List<TourKeyPointDto>();
            tourKeyPoints = new List<TourKeyPoint>();
            tourGuidences = new List<TourGuidence>();
            tourGuidenceDtos = new List<TourGuidenceDto>();

            Images = new List<string>();
            ImagesView = new ObservableCollection<ImagesView>();
            SelectedImage = null;

            TourDates = new List<DateTime>();
            KeyPointLocation = new List<Location>();
            tbDate.Text = "01/01/0001 00:00:00";
            counter = tourKeyPointRepository.NextId() - 1;
        }

        private void SaveTour(object sender, RoutedEventArgs e)
        {
            LocationDto locationDto = new LocationDto(TourCountry, TourCity, TourAddress, TourLatitude, TourLongitude);
            Location location = locationRepository.Save(locationDto);

            TourDto tourDto = new TourDto(TourName, location, Description, Languages, maxGuests, Duration, Images, Guide);
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
        }

        private void AddDateToList(object sender, RoutedEventArgs e)
        {

            //kreiranje tourGuidence
            //TourGuidence tourGuidence = tourGuidenceRepository.Save(TourDate);
            //tourGuidences.Add(tourGuidence);
            //TourGuidence tourGuidence = new(tourGuidenceRepository.NextIdTourGuidence(), null, TourDate, false, false, false);
            //tourGuidences.Add(tourGuidence);

            TourGuidenceDto tgDTO = new(TourDate);
            tourGuidenceDtos.Add(tgDTO);

            /*foreach (TourKeyPoint t in tourKeyPoints)
            {
                t.TourGuidence = tourGuidence;
                tourKeyPointRepository.Update(t);
            }*/
            /*foreach (TourKeyPoint t in tourKeyPoints)
            {
                t.TourGuidence = tourGuidence;
                tourKeyPointRepository.SaveKeyPoint(t);
            }*/

            // TourDates.Add(TourDate);
        }

        private void AddTourKeyPoints(object sender, RoutedEventArgs e)
        {
            LocationDto locationDto = new(KeyPointCountry, KeyPointCity, KeyPointAddress, KeyPointLatitude, KeyPointLongitude);
            Location location = locationRepository.Save(locationDto);

            TourKeyPointDto tourKeyPointDto = new(KeyPointName, location);
            tourKeyPointDtos.Add(tourKeyPointDto);
            //TourKeyPoint tourKeyPoint = tourKeyPointRepository.Save(tourKeyPointDto);
            //TourKeyPoint tourKeyPoint = new(counter++, tourKeyPointDto.TourKeyPointName, tourKeyPointDto.Location, null, false);
            //tourKeyPoints.Add(tourKeyPoint);

            /*foreach (TourGuidence t in tourGuidences)
            {
                TourKeyPointDto tourKeyPointDto = new(KeyPointName, location,t);
                TourKeyPoint tourKeyPoint = tourKeyPointRepository.Save(tourKeyPointDto);
                tourKeyPoints.Add(tourKeyPoint);
            }*/
        }

        private void AddImageToList(object sender, RoutedEventArgs e)
        {
            if (CheckErrorUrlExists() == true)
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

            if (SelectedImage == null)
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

        private bool CheckErrorImagesNumber()
        {
            if (Images.Count <= 0)
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
            foreach (string temporaryImage in Images)
            {
                if (temporaryImage.Equals(Image.ToString()) == true)
                {
                    return false;
                }
            }
            return true;
        }

        void LoadingRowForDgImages(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void SliderLatitudeValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TourLatitude = Math.Round((decimal)sliderLatitude.Value, 2);
        }

        private void SliderLongitudeValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TourLongitude = Math.Round((decimal)sliderLongitude.Value, 2);
        }

        private void SliderKeyPointLatitudeValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            KeyPointLatitude = Math.Round((decimal)sliderKeyPointLatitude.Value, 2);
        }

        private void SliderKeyPointLongitudeValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            KeyPointLongitude = Math.Round((decimal)sliderKeyPointLongitude.Value, 2);
        }


        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var label = (Label)sender;
                Keyboard.Focus(label.Target);
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
    }
}
