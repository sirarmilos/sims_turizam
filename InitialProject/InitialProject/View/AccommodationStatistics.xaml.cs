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

using LiveCharts;
using LiveCharts.Wpf;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AccommodationStatistics.xaml
    /// </summary>
    public partial class AccommodationStatistics : Window
    {
        private readonly AccommodationService accommodationService;

        public string OwnerUsername
        {
            get;
            set;
        }

        public List<string> UnreadCancelledReservations
        {
            get;
            set;
        }

        public ShowStatisticsAccommodationDTO ShowStatisticsAccommodationDTO
        {
            get;
            set;
        }

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

        public List<AccommodationStatisticsDataDTO> AccommodationStatisticsDataDTOs
        {
            get;
            set;
        }

        public string MostBusyPeriodTime
        {
            get;
            set;
        }

        public SeriesCollection SeriesCollectionReservation { get; set; }

        public SeriesCollection SeriesCollectionCanceledReservation { get; set; }

        public SeriesCollection SeriesCollectionRescheduledReservation { get; set; }

        public SeriesCollection SeriesCollectionRenovationRecommedation { get; set; }

        public List<string> ChartPeriodTime { get; set; }

        public AccommodationStatistics(string ownerUsername, string ownerHeader, ShowStatisticsAccommodationDTO showStatisticsAccommodationDTO)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            accommodationService = new AccommodationService(OwnerUsername);

            ShowStatisticsAccommodationDTO = showStatisticsAccommodationDTO;

            SetDefaultValue();

            SetMenu(ownerHeader);
        }

        private void SetMenu(string ownerHeader)
        {
            usernameAndSuperOwner.Header = ownerHeader;

            rateGuestsNotifications.Header = "Number of unrated guests: " + accommodationService.FindNumberOfUnratedGuests(OwnerUsername) + ".";

            UnreadCancelledReservations = new List<string>();

            UnreadCancelledReservations = accommodationService.FindUnreadCancelledReservations(OwnerUsername);
        }

        private void SetDefaultValue()
        {
            SelectedYear = null;

            Years = new List<string>();

            Years = accommodationService.FindAccommodationYears(ShowStatisticsAccommodationDTO.Id);

            if(Years.Count == 1)
            {
                Years.Clear();
                labelDataNotFound.Visibility = Visibility.Visible;
                cbSelectYear.IsEnabled = false;
                MostBusyPeriodTime = "-";
                chartReservation.Visibility = Visibility.Hidden;
                chartCanceledReservation.Visibility = Visibility.Hidden;
                chartRescheduledReservation.Visibility = Visibility.Hidden;
                chartRenovationRecommendation.Visibility = Visibility.Hidden;
            }
            else
            {
                labelDataNotFound.Visibility = Visibility.Hidden;
                AccommodationStatisticsDataDTOs = new List<AccommodationStatisticsDataDTO>();

                AccommodationStatisticsDataDTOs = accommodationService.FindAccommodationYearStatistics(ShowStatisticsAccommodationDTO.Id, Years);

                MostBusyPeriodTime = accommodationService.FindMostBusyYear(ShowStatisticsAccommodationDTO.Id, Years).ToString();

                SetChartYearValue();
            }
        }

        private void SetChartYearValue()
        {
            ChartPeriodTime = Years.FindAll(x => x.Equals("all year") == false);

            SeriesCollectionReservation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "number of reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationReservationCount(AccommodationStatisticsDataDTOs))
                }
            };

            SeriesCollectionCanceledReservation = new SeriesCollection()
            {
                new ColumnSeries
                {
                    Title = "number of canceled reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationCanceledReservationCount(AccommodationStatisticsDataDTOs))
                }
            };

            SeriesCollectionRescheduledReservation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "number of rescheduled reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationRescheduledReservationCount(AccommodationStatisticsDataDTOs))
                }
            };

            SeriesCollectionRenovationRecommedation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "number of renovation recommedations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationRenovationRecommendationsCount(AccommodationStatisticsDataDTOs))
                }
            };

            labelXAxisReservation.Title = "Years";
            labelXAxisCanceledReservation.Title = "Years";
            labelXAxisRescheduledReservation.Title = "Years";
            labelXAxisRenovationRecommendation.Title = "Years";

            labelXAxisReservation.Labels = ChartPeriodTime;
            labelXAxisCanceledReservation.Labels = ChartPeriodTime;
            labelXAxisRescheduledReservation.Labels = ChartPeriodTime;
            labelXAxisRenovationRecommendation.Labels = ChartPeriodTime;

            chartReservation.Series = SeriesCollectionReservation;
            chartCanceledReservation.Series = SeriesCollectionCanceledReservation;
            chartRescheduledReservation.Series = SeriesCollectionRescheduledReservation;
            chartRenovationRecommendation.Series = SeriesCollectionRenovationRecommedation;
        }

        private void ReadCancelledReservationNotification(object sender, RoutedEventArgs e)
        {
            string viewedCancelledReservation = ((MenuItem)sender).Header.ToString();

            if (viewedCancelledReservation.Equals("There are currently no new booking cancellations.") == false)
            {
                accommodationService.SaveViewedCancelledReservation(FindDTO(viewedCancelledReservation));

                UnreadCancelledReservations = accommodationService.FindUnreadCancelledReservations(OwnerUsername);

                cancelledReservationsNotificationsList.DataContext = UnreadCancelledReservations;
            }
        }

        private CancelledReservationsNotificationDTO FindDTO(string viewedCancelledReservation)
        {
            string accommodationName = viewedCancelledReservation.Split(":")[0];
            DateTime reservationStartDate = Convert.ToDateTime(viewedCancelledReservation.Split(" ")[1]);
            DateTime reservationEndDate = Convert.ToDateTime(viewedCancelledReservation.Split(" ")[3]);

            CancelledReservationsNotificationDTO cancelledReservationsNotificationDTO = new CancelledReservationsNotificationDTO(accommodationName, reservationStartDate, reservationEndDate);

            return cancelledReservationsNotificationDTO;
        }

        private void ShowStatistics(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedYear.Equals("all year") == true)
            {
                AccommodationStatisticsDataDTOs = accommodationService.FindAccommodationYearStatistics(ShowStatisticsAccommodationDTO.Id, Years);

                MostBusyPeriodTime = accommodationService.FindMostBusyYear(ShowStatisticsAccommodationDTO.Id, Years).ToString();
                labelMostBusyPeriodTime.Content = MostBusyPeriodTime; //

                SetChartYearValue();
            }
            else
            {
                AccommodationStatisticsDataDTOs = accommodationService.FindAccommodationMonthStatistics(ShowStatisticsAccommodationDTO.Id, Convert.ToInt32(SelectedYear));

                MostBusyPeriodTime = accommodationService.FindMostBusyMonth(ShowStatisticsAccommodationDTO.Id, Convert.ToInt32(SelectedYear));
                labelMostBusyPeriodTime.Content = MostBusyPeriodTime; //

                SetChartMonthValue();
            }
        }

        public void SetChartMonthValue()
        {
            List<string> months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "Septmeber", "October", "November", "December" };

            ChartPeriodTime = months;

            /* labelXAxisReservation = new LiveCharts.Wpf.Axis()
            {
                Separator = new LiveCharts.Wpf.Separator { Step = 1 },
            };

            labelXAxisCanceledReservation = new LiveCharts.Wpf.Axis()
            {
                Separator = new LiveCharts.Wpf.Separator { Step = 1 },
            };

            labelXAxisRescheduledReservation = new LiveCharts.Wpf.Axis()
            {
                Separator = new LiveCharts.Wpf.Separator { Step = 1 },
            };

            labelXAxisRenovationRecommendation = new LiveCharts.Wpf.Axis()
            {
                Separator = new LiveCharts.Wpf.Separator { Step = 1 },
            };*/

            /* LiveCharts.Wpf.Axis ax = new LiveCharts.Wpf.Axis()
            {
                Separator = new LiveCharts.Wpf.Separator { Step = 1 },
            };*/

            SeriesCollectionReservation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "number of reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationReservationMonthCount(AccommodationStatisticsDataDTOs))
                }
            };

            SeriesCollectionCanceledReservation = new SeriesCollection()
            {
                new ColumnSeries
                {
                    Title = "number of canceled reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationCanceledReservationMonthCount(AccommodationStatisticsDataDTOs))
                }
            };

            SeriesCollectionRescheduledReservation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "number of rescheduled reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationRescheduledReservationMonthCount(AccommodationStatisticsDataDTOs))
                }
            };

            SeriesCollectionRenovationRecommedation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "number of renovation recommedations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationRenovationRecommendationsMonthCount(AccommodationStatisticsDataDTOs))
                }
            };

            labelXAxisReservation.Title = "Months";
            labelXAxisCanceledReservation.Title = "Months";
            labelXAxisRescheduledReservation.Title = "Months";
            labelXAxisRenovationRecommendation.Title = "Months";

            labelXAxisReservation.Labels = ChartPeriodTime;
            labelXAxisCanceledReservation.Labels = ChartPeriodTime;
            labelXAxisRescheduledReservation.Labels = ChartPeriodTime;
            labelXAxisRenovationRecommendation.Labels = ChartPeriodTime;

            chartReservation.Series = SeriesCollectionReservation;
            chartCanceledReservation.Series = SeriesCollectionCanceledReservation;
            chartRescheduledReservation.Series = SeriesCollectionRescheduledReservation;
            chartRenovationRecommendation.Series = SeriesCollectionRenovationRecommedation;
        }

        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var label = (System.Windows.Controls.Label)sender; // Label
                Keyboard.Focus(label.Target);
            }
        }

        void LoadingRowForDgImages(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void GoToOwnerHomePageLogin(object sender, RoutedEventArgs e)
        {
            OwnerHomePageLogin window = new OwnerHomePageLogin(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToAccommodationStart(object sender, RoutedEventArgs e)
        {
            AccommodationStart window = new AccommodationStart(OwnerUsername);
            window.Show();
            Close();
        }

        private void GoToShowOwnerManageBookingMoveRequests(object sender, RoutedEventArgs e)
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToShowAndCancellationRenovation(object sender, RoutedEventArgs e)
        {
            ShowAndCancellationRenovation window = new ShowAndCancellationRenovation(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToRateGuests(object sender, RoutedEventArgs e)
        {
            RateGuests window = new RateGuests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToShowGuestReviews(object sender, RoutedEventArgs e)
        {
            ShowGuestReviews window = new ShowGuestReviews(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToOwnerForum(object sender, RoutedEventArgs e)
        {
            OwnerForum window = new OwnerForum(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToOwnerHomePageNotLogin(object sender, RoutedEventArgs e)
        {
            OwnerHomePageNotLogin window = new OwnerHomePageNotLogin();
            window.Show();
            Close();
        }
    }
}
