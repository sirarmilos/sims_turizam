using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2PageTours.xaml
    /// </summary>
    public partial class Guest2PageTours : Page
    {
        private readonly TourService tourService = new TourService();


        private string city;
        private string country;
        private int duration;
        private Language language;
        private int maxGuests;


        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));
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

        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged(nameof(Duration));
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guest2PageTours()
        {
            InitializeComponent();
            InitializeCbLang();
            DataContext = this;
            listTours.ItemsSource = tourService.GetToursForDisplay();

            createReservationButton.IsEnabled = false;
        }

        public void InitializeCbLang()
        {
            foreach (string lang in (Enum.GetNames(typeof(Language))))
            {
                cbLanguage.Items.Add(lang);
            }
            cbLanguage.SelectedIndex = 0;
        }

        private void listTours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            createReservationButton.IsEnabled = true;
        }



        private void CreateReservation(object sender, RoutedEventArgs e)
        {

        }

        private void SearchTours(object sender, RoutedEventArgs e)
        {
           listTours.ItemsSource = tourService.SearchAndShow(City, Country, Duration, Languagee, MaxGuests);
        }
    }
}
