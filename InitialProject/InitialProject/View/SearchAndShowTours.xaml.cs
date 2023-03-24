using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
    /// Interaction logic for SearchAndShowTours.xaml
    /// </summary>
    public partial class SearchAndShowTours : Window
    {
        //public static ObservableCollection<Tour> tours { get; set; }

        private readonly TourRepository tourRepository;

        private List<Tour> tours { get; set; }

        public Tour tour { get; set; }

        private string tourName;
        private string country;
        private string city;
        private string description;
        private Language language;
        private int maxGuests;
        private List<TourKeyPoint> tourKeyPoints;
        private List<DateTime> dates;
        private int duration;
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

        public List<string> Images { get; set; }

        public string TourName
        {
            get {  return tourName; }
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
                OnPropertyChanged(nameof(Country));
            }

        }

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));
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

        public Language Languagee
        {
            get { return language; }
            set
            {
                language = value;
                OnPropertyChanged(nameof(Languagee));
            }
        }

        public int MaxGuests
        {
            get { return maxGuests; }
            set
            { 
                maxGuests = value;
                OnPropertyChanged(nameof(MaxGuests));
            }
        }

        public List<TourKeyPoint> TourKeyPoints { get; set; }

        public List<string> TourDate { get; set; }

        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged(nameof(Duration));
            }

        }

        public SearchAndShowTours()
        {
            InitializeComponent();
            Initializecblang();
            DataContext = this;
            tourRepository = new TourRepository();
            Images = new List<string>();

            tours = tourRepository.SearchAndShow(City, Country, Duration, Languagee, MaxGuests);

            TourDisplayDTO tddto = new TourDisplayDTO();
            List<TourDisplayDTO> toursDisplay = new List<TourDisplayDTO>();
            tours = tourRepository.SearchAndShow();

            foreach (Tour tour in tours)
            {
                toursDisplay.Add(tddto.CreateDTO(tour));
            }

            listTours.ItemsSource = toursDisplay;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Initializecblang()
        {
            foreach(string lang in (Enum.GetNames(typeof(Language))))
            {
                cblang.Items.Add(lang);
            }
            cblang.SelectedIndex = 0;
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            TourDisplayDTO tddto = new TourDisplayDTO();
            List<TourDisplayDTO> toursDisplay = new List<TourDisplayDTO>();
            tours = tourRepository.SearchAndShow(City, Country, Duration, Languagee, MaxGuests);


            foreach(Tour tour in tours)
            {
                toursDisplay.Add(tddto.CreateDTO(tour));
            }

            listTours.ItemsSource = toursDisplay;
     
        }

        private void CreateReservation(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listTours.SelectedItems.Count != 1)
                {
                    MessageBox.Show("Morate da odaberete jedan red!");
                }
                else
                {
                    TourDisplayDTO tour = new TourDisplayDTO();
                    tour = (TourDisplayDTO)listTours.SelectedItems[0];
                    TourReservation tourReservation = new TourReservation(tour.TourName);
                    tourReservation.Show();
                }
            }
            catch
            { }
        }
    }
}
