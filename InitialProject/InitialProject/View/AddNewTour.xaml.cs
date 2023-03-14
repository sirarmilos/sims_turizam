using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
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

        private string tourName;
        private string tourCountry;
        private string tourCity;
        private string tourAddress;
        private decimal tourLatitude;
        private decimal tourLongitude;
        private string description;
        private Languages languages;
        private int maxGuests;
        private List<TourKeyPoints> tourKeyPoints;
        private TourKeyPoints tourKeyPoint;
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

        public Languages Languages
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
            get { return maxGuests;}
            set
            {
                maxGuests = value;  
                OnPropertyChanged();    
            }
        }

        public List<TourKeyPoints> TourKeyPoints
        {
            get;
            set;
        }

        public TourKeyPoints TourKeyPoint
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

        public List<Location> KeyPointLocation
        {
            get;
            set;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public AddNewTour()
        {
            InitializeComponent();
            DataContext = this;
            tourRepository = new TourRepository();
            TourKeyPoints = new List<TourKeyPoints>();
            TourDates = new List<DateTime>();
            Images = new List<string>();
            KeyPointLocation = new List<Location>();  
        }

        private void SaveTour(object sender, RoutedEventArgs e)
        {
            tourRepository.Save(TourName, TourCountry, TourCity, TourAddress, TourLatitude, TourLongitude, Description, Languages, MaxGuests, TourKeyPoints, KeyPointName, KeyPointCountry, KeyPointCity, KeyPointAddress, KeyPointLatitude, KeyPointLongitude, TourDates, Duration, Images);
        }

        private void AddImageToList(object sender, RoutedEventArgs e)
        {
            Images.Add(Image.ToString());
        }

        private void AddDateToList(object sender, RoutedEventArgs e)
        {
            TourDates.Add(TourDate);
        }

        private void AddTourKeyPoints(object sender, RoutedEventArgs e)
        {
            locationIdCounter++;
            tourKeyPointsIdCounter++;
            Location newLocation = new Location(locationIdCounter, KeyPointCountry, KeyPointCity, KeyPointAddress, KeyPointLatitude, KeyPointLongitude);
            TourKeyPoints newTourKeyPoint = new TourKeyPoints(tourKeyPointsIdCounter, KeyPointName, newLocation);
            TourKeyPoints.Add(newTourKeyPoint);
            // mozda treba lokaciju uneti
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            locationIdCounter = tourRepository.NextIdLocation();
            tourKeyPointsIdCounter = tourRepository.NextIdTourKeyPoints();
        }
    }

    
}
