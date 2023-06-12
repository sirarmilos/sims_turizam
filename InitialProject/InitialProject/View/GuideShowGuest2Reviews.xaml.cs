using InitialProject.Service;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
using InitialProject.Model;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for GuideShowGuest2Reviews.xaml
    /// </summary>
    public partial class GuideShowGuest2Reviews : Window
    {
        private ObservableCollection<Dto.RateGuideDisplayDto> guideRates;
        public ObservableCollection<Dto.RateGuideDisplayDto> GuideRates
        {
            get { return guideRates; }
            set
            {
                guideRates = value;
                OnPropertyChanged("GuideRates");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private readonly RateGuideService rateGuideService;

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }
        public GuideShowGuest2Reviews(string guideUsername)
        {
            InitializeComponent();
            DataContext = this;
            Guide = guideUsername;
            rateGuideService = new RateGuideService();
            GuideRates = new ObservableCollection<Dto.RateGuideDisplayDto>(rateGuideService.FindForDisplay(Guide));
            itemsControlShowGuideComments.ItemsSource = GuideRates;
            InitializePieChart();

        }

        private void DeleteRateLogical(object sender, RoutedEventArgs e)
        {

            if (sender is Button button && button.DataContext is Dto.RateGuideDisplayDto selectedRate)
            {
                rateGuideService.UpdateIsDeleted(selectedRate.UserId, selectedRate.tourGuidenceId);
                GuideRates = new ObservableCollection<Dto.RateGuideDisplayDto>(rateGuideService.FindForDisplay(Guide));
                itemsControlShowGuideComments.ItemsSource = GuideRates;
            }
            else
            {
                MessageBox.Show("Please select one review!");
            }
        }

        private void GoToPreviousPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void InitializePieChart()
        {
            VoucherService voucherService = new VoucherService();

            pieChart.Series.Clear();

            pieChart.Series.Add(new PieSeries
            {
                Title = "with Voucher",

                Values = new ChartValues<double> { voucherService.GetWithVoucherPercentage(1) },
                DataLabels = true
            });

            pieChart.Series.Add(new PieSeries
            {
                Title = "without voucher",
                Values = new ChartValues<double> { voucherService.GetWithoutVoucherPercentage(1) },
                DataLabels = true
            });

            pieChart.LegendLocation = LegendLocation.Right;
            //pieChart.DefaultLegend.Visibility = Visibility.Visible;
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
