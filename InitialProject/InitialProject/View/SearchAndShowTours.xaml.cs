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
using System.Diagnostics.Metrics;
using System.Globalization;
using InitialProject.Service;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for SearchAndShowTours.xaml
    /// </summary>
    /// 

    public partial class SearchAndShowTours : Window
    {

        private readonly TourRepository tourRepository;

        private readonly TourService tourService = new TourService();

        private readonly string username;

        public  List<TourDisplayDTO> tourDisplayDTOs {  get; set; }

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



        public SearchAndShowTours(string username)
        {
            InitializeComponent();
            Initializecblang();
            DataContext = this;
            tourRepository = new TourRepository();

            tourDisplayDTOs = new List<TourDisplayDTO>();
            tourDisplayDTOs = tourService.GetToursForDisplay();
            this.username = username;
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

        private void SearchAndShow(object sender, RoutedEventArgs e)
        {
            tourDisplayDTOs = tourService.SearchAndShow(City,Country,Duration,Languagee,MaxGuests);
            listTours.ItemsSource = tourDisplayDTOs;
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
                    TourReservation tourReservation = new TourReservation(tour,username);
                    tourReservation.Show();
                }
            }
            catch
            { }
        }
    }

    
}
