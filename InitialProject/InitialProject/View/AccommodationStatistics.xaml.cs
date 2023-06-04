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

        public List<CancelledReservationsNotificationDTO> UnreadCancelledReservationsToDelete
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

        private void OwnerHomePageLogin_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OwnerHomePageLogin_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OwnerHomePageLogin window = new OwnerHomePageLogin(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void AccommodationStart_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AccommodationStart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AccommodationStart window = new AccommodationStart(OwnerUsername);
            window.Show();
            Close();
        }

        private void ShowOwnerManageBookingMoveRequests_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ShowOwnerManageBookingMoveRequests_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void ShowAndCancellationRenovation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ShowAndCancellationRenovation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowAndCancellationRenovation window = new ShowAndCancellationRenovation(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void RateGuests_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RateGuests_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RateGuests window = new RateGuests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void ShowGuestReviews_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ShowGuestReviews_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowGuestReviews window = new ShowGuestReviews(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void OwnerForum_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OwnerForum_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OwnerForum window = new OwnerForum(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void Logout_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Logout_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (GlobalOwnerClass.NotificationRead == true)
            {
                accommodationService.MarkAsReadNotificationsCancelledReservations(UnreadCancelledReservationsToDelete);
            }

            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

        private void Notifications_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Notifications_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GlobalOwnerClass.NotificationRead = true;
            notifications.IsSubmenuOpen = true;
            rateGuestsNotifications.Focus();
            accommodationService.MarkAsReadNotificationsForums(OwnerUsername);
        }

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

            UnreadCancelledReservationsToDelete = accommodationService.FindUnreadCancelledReservations(OwnerUsername);

            if(UnreadCancelledReservationsToDelete.Count == 0)
            {
                UnreadCancelledReservations.Add("There are no new canceled reservations");
            }
            else
            {
                foreach(CancelledReservationsNotificationDTO temporaryCanceledReservationsNotificationDTO in UnreadCancelledReservationsToDelete.ToList())
                {
                    UnreadCancelledReservations.Add(temporaryCanceledReservationsNotificationDTO.AccommodationName + ": " + temporaryCanceledReservationsNotificationDTO.ReservationStartDate.ToShortDateString() + " - " + temporaryCanceledReservationsNotificationDTO.ReservationEndDate.ToShortDateString());
                }
            }

            forumNotifications.Header = "Number of new forums: " + accommodationService.FindNumberOfNewForums(OwnerUsername) + ".";
        }

        private void SetDefaultValue()
        {
            Years = accommodationService.FindAccommodationYears(ShowStatisticsAccommodationDTO.Id);

            SelectedYear = "all year";

            if(Years.Count == 1)
            {
                SetDefaultEmptyValue();
            }
            else
            {
                SetDefaultExistingValue();
            }
        }

        private void SetDefaultEmptyValue()
        {
            Years.Clear();
            labelDataNotFound.Visibility = Visibility.Visible;
            labelReservationTitle.Visibility = Visibility.Hidden;
            labelCanceledReservationTitle.Visibility = Visibility.Hidden;
            labelRescheduledReservationTitle.Visibility = Visibility.Hidden;
            labelRenovationRecommendationTitle.Visibility = Visibility.Hidden;
            chartReservation.Visibility = Visibility.Hidden;
            chartCanceledReservation.Visibility = Visibility.Hidden;
            chartRescheduledReservation.Visibility = Visibility.Hidden;
            chartRenovationRecommendation.Visibility = Visibility.Hidden;
            cbSelectPeriod.IsEnabled = false;
            MostBusyPeriodTime = "-";
        }

        private void SetDefaultExistingValue()
        {
            labelDataNotFound.Visibility = Visibility.Hidden;

            AccommodationStatisticsDataDTOs = accommodationService.FindAccommodationYearStatistics(ShowStatisticsAccommodationDTO.Id, Years);

            MostBusyPeriodTime = accommodationService.FindMostBusyYear(ShowStatisticsAccommodationDTO.Id, Years).ToString();

            SetChartYearValue();
        }

        private void SetChartYearValue()
        {
            ChartPeriodTime = Years.FindAll(x => x.Equals("all year") == false);

            SetChartYearReservation();
            SetChartYearCanceledReservation();
            SetChartYearRescheduledReservation();
            SetChartYearRenovationRecommendation();
        }

        private void SetChartYearReservation()
        {
            SeriesCollectionReservation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationReservationCount(AccommodationStatisticsDataDTOs))
                }
            };

            labelXAxisReservation.Title = "Years";
            labelXAxisReservation.Labels = ChartPeriodTime;
            chartReservation.Series = SeriesCollectionReservation;
        }

        private void SetChartYearCanceledReservation()
        {
            SeriesCollectionCanceledReservation = new SeriesCollection()
            {
                new ColumnSeries
                {
                    Title = "Number of canceled reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationCanceledReservationCount(AccommodationStatisticsDataDTOs))
                }
            };

            labelXAxisCanceledReservation.Title = "Years";
            labelXAxisCanceledReservation.Labels = ChartPeriodTime;
            chartCanceledReservation.Series = SeriesCollectionCanceledReservation;
        }

        private void SetChartYearRescheduledReservation()
        {
            SeriesCollectionRescheduledReservation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of rescheduled reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationRescheduledReservationCount(AccommodationStatisticsDataDTOs))
                }
            };

            labelXAxisRescheduledReservation.Title = "Years";
            labelXAxisRescheduledReservation.Labels = ChartPeriodTime;
            chartRescheduledReservation.Series = SeriesCollectionRescheduledReservation;
        }

        private void SetChartYearRenovationRecommendation()
        {
            SeriesCollectionRenovationRecommedation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of renovation recommedations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationRenovationRecommendationsCount(AccommodationStatisticsDataDTOs))
                }
            };

            labelXAxisRenovationRecommendation.Title = "Years";
            labelXAxisRenovationRecommendation.Labels = ChartPeriodTime;
            chartRenovationRecommendation.Series = SeriesCollectionRenovationRecommedation;
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
            ChartPeriodTime = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "Septmeber", "October", "November", "December" };

            SetChartMonthReservation();
            SetChartMonthCanceledReservation();
            SetChartMonthRescheduledReservation();
            SetChartMonthRenovationRecommendation();
        }

        private void SetChartMonthReservation()
        {
            SeriesCollectionReservation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationReservationMonthCount(AccommodationStatisticsDataDTOs))
                }
            };

            labelXAxisReservation.Title = "Months";
            labelXAxisReservation.Labels = ChartPeriodTime;
            chartReservation.Series = SeriesCollectionReservation;
        }

        private void SetChartMonthCanceledReservation()
        {
            SeriesCollectionCanceledReservation = new SeriesCollection()
            {
                new ColumnSeries
                {
                    Title = "Number of canceled reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationCanceledReservationMonthCount(AccommodationStatisticsDataDTOs))
                }
            };

            labelXAxisCanceledReservation.Title = "Months";
            labelXAxisCanceledReservation.Labels = ChartPeriodTime;
            chartCanceledReservation.Series = SeriesCollectionCanceledReservation;
        }

        private void SetChartMonthRescheduledReservation()
        {
            SeriesCollectionRescheduledReservation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of rescheduled reservations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationRescheduledReservationMonthCount(AccommodationStatisticsDataDTOs))
                }
            };

            labelXAxisRescheduledReservation.Title = "Months";
            labelXAxisRescheduledReservation.Labels = ChartPeriodTime;
            chartRescheduledReservation.Series = SeriesCollectionRescheduledReservation;
        }

        private void SetChartMonthRenovationRecommendation()
        {
            SeriesCollectionRenovationRecommedation = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of renovation recommedations",
                    DataLabels = true,
                    Values = new ChartValues<int>(accommodationService.FindAccommodationRenovationRecommendationsMonthCount(AccommodationStatisticsDataDTOs))
                }
            };

            labelXAxisRenovationRecommendation.Title = "Months";
            labelXAxisRenovationRecommendation.Labels = ChartPeriodTime;
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
    }
}
