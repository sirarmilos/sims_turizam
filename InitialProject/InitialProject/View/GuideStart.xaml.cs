using InitialProject.Model;
using InitialProject.Repository;
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
    /// Interaction logic for GuideStart.xaml
    /// </summary>
    public partial class GuideStart : Window, INotifyPropertyChanged
    {
        public static Tour tour { get; set; }

        public static Tour tourFiltered { get; set; }

        public static ObservableCollection<Tour> tourAgeStats { get; set; }

        public ObservableCollection<int> ageCount { get; set; }

        private ObservableCollection<double> voucherPercentage;

        private readonly TourGuidenceService tourGuidenceService;

        private readonly TourService tourService;

       // private readonly TourRepository tourRepository;

        private readonly VoucherService voucherService;

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }


        public string WithVoucher
        {
            get { return voucherPercentage[0].ToString(); }
        }

        public string WithoutVoucher
        {
            get { return voucherPercentage[1].ToString(); }
        }

        public string Under18
        {
            get { return ageCount[0].ToString(); }
        }

        public string From18To50
        {
            get { return ageCount[1].ToString(); }
        }

        public string Above50
        {
            get { return ageCount[2].ToString(); }
        }

        public GuideStart(string username)
        {
            InitializeComponent();
            DataContext = this;
            Guide = username;
            voucherService = new VoucherService();
            tourGuidenceService = new TourGuidenceService();
            tourService = new TourService();
            tour = tourGuidenceService.FindMostVisitedAllTime();
            int year = 2021;
            tourFiltered = tourGuidenceService.FindMostVisitedByYear(year);
            //tourAgeStats = tourRepository.GetById(3);
            tourAgeStats = new ObservableCollection<Tour>(tourService.FindAll());
            //ageCount = new ObservableCollection<int>(tourRepository.GetGuestNumber(tourAgeStats.Id));
           // voucherPercentage = new ObservableCollection<double>(/*tourRepository.GetVoucherPercentage(tourAgeStats.Id)*/);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GoToAddNewTour(object sender, RoutedEventArgs e)
        {
            AddNewTour window = new AddNewTour(Guide);
            window.Show();
        }

        private void GoToShowTourGuidences(object sender, RoutedEventArgs e)
        {
            ShowTourGuidences window = new ShowTourGuidences(Guide);
            window.Show();
        }

        private void GoToFutureTours(object sender, RoutedEventArgs e)
        {
            FutureTours window = new FutureTours();
            window.Show();
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

        private void SelectTour(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem != null)
            {
                Tour tour = comboBox.SelectedItem as Tour;
                DisplayStatistics(tour.Id);
            }
        }

        private void DisplayStatistics(int id)
        {
            voucherPercentage = new ObservableCollection<double>(voucherService.GetVoucherPercentage(id));
            ageCount = new ObservableCollection<int>(tourService.FindGuestNumber(id, Guide));
            OnPropertyChanged(nameof(WithVoucher));
            OnPropertyChanged(nameof(WithoutVoucher));
            OnPropertyChanged(nameof(Under18));
            OnPropertyChanged(nameof(From18To50));
            OnPropertyChanged(nameof(Above50));
        }

        private void GoToShowReviews(object sender, RoutedEventArgs e)
        {
            ShowReviewsGuest2 window = new ShowReviewsGuest2(Guide);
            window.Show();
        }

        private void ShowKeyPoints(object sender, RoutedEventArgs e)
        {
            TourGuidence tourGuidence = tourGuidenceService.FindById(1);
            ShowKeyPoints window = new ShowKeyPoints(tourGuidence);
            window.Show();
        }

        private void GoToSearchAndShowTourRequests(object sender, RoutedEventArgs e)
        {
            SearchAndShowTourRequests window = new SearchAndShowTourRequests();
            window.Show();
        }

        private void GoToMostPopularTour(object sender, RoutedEventArgs e)
        {
            ShowMostPopularTour window = new ShowMostPopularTour();
            window.Show();
        }

        private void GoToTourRequests(object sender, RoutedEventArgs e)
        {
            SearchAndShowTourRequests window = new SearchAndShowTourRequests();
            window.Show();
        }

        private void GoToCreateTourRequestStats(object sender, RoutedEventArgs e)
        {
            CreateTourForRequestStatistics window = new CreateTourForRequestStatistics(Guide);
            window.Show();
        }

        private void GoToRequestStatistics(object sender, RoutedEventArgs e)
        {
            SearchAndShowRequestStatistics window = new SearchAndShowRequestStatistics(Guide);
            window.Show();
        }

    }
}
