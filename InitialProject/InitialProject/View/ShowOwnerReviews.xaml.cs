using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace InitialProject.View
{

    public partial class ShowOwnerReviews : Page
    {
        private readonly ReviewService reviewService;

        private string guest1;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public List<ShowOwnerReviewsDTO> ShowOwnerReviewsDTOs
        {
            get;
            set;
        }

        private void CheckNotification()
        {
            if (Notification)
            {
                NotificationMenuItemImageNotificationBell.Visibility = Visibility.Visible;
                NotificationMenuItemImageRegularBell.Visibility = Visibility.Collapsed;
            }
            else
            {
                NotificationMenuItemImageNotificationBell.Visibility = Visibility.Collapsed;
                NotificationMenuItemImageRegularBell.Visibility = Visibility.Visible;
            }
        }

        private bool notification;
        public bool Notification
        {
            get { return notification; }
            set
            {
                notification = value;
            }
        }

        public ShowOwnerReviews(string guest1, Page page)
        {
            InitializeComponent();

            Guest1 = guest1;

            DataContext = this;

            reviewService = new ReviewService(Guest1);

            ShowOwnerReviewsDTOs = new List<ShowOwnerReviewsDTO>();

            // charts
            SetFirstChart(); // sporo

            SetSecondChart(); // sporo

            ShowOwnerReviewsDTOs = reviewService.FindAllOwnerReviews(); // sporo

            SetUsernameHeader();

            SetComboBoxes(page);
        }

        public List<string> ChartPeriodTime { get; set; }
        public SeriesCollection SeriesCollectionNumberOfReviewsPerMonth { get; set; }



        public void SetFirstChart()
        {
            // set periods
            ChartPeriodTime = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            SeriesCollectionNumberOfReviewsPerMonth = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of reviews",
                    DataLabels = true,
                    Values = new ChartValues<int>(reviewService.FindNumberOfRatesInLastYearPerMonth(Guest1))
                }
            };

            labelXAxisReviewsFirstChart.Labels = ChartPeriodTime;
            firstChart.Series = SeriesCollectionNumberOfReviewsPerMonth;
        }

        public SeriesCollection ScatterReviewsAverage { get; set; }
        public void SetSecondChart()
        {
            // Set periods
            ChartPeriodTime = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            ScatterReviewsAverage = new SeriesCollection
            {
                new ScatterSeries
                {
                    Title = "Average of reviews",
                    DataLabels = true,
                    // Values = new ChartValues<int>(reviewService.FindNumberOfRatesInLastYearPerMonth(Guest1))
                    Values = new ChartValues<double>(reviewService.FindAvgRateInLastYearPerMonth(Guest1))
                }
            };

            labelXAxisReviewsSecondChart.Labels = ChartPeriodTime;
            secondChart.Series = ScatterReviewsAverage;
        }

        /*
        public SeriesCollection SeriesCollectionReviewsAverage { get; set; }
        public void SetSecondChart()
        {
	        //set periods
	        ChartPeriodTime = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

	        SeriesCollectionReviewsAverage = new SeriesCollection
	        {
		        new ColumnSeries
		        {
			        Title = "Average of reviews",
			        DataLabels = true,
			        //Values = new ChartValues<int>(reviewService.FindNumberOfRatesInLastYearPerMonth(Guest1))
			        Values = new ChartValues<double>(reviewService.FindAvgRateInLastYearPerMonth(Guest1))
		        }
	        };

	        labelXAxisReviewsSecondChart.Labels = ChartPeriodTime;
	        secondChart.Series = SeriesCollectionReviewsAverage;
        }


		<Label x:Name="labelRenovationRecommendationTitle" Grid.Column="2" Grid.Row="2" Content="Number of renovation recommendation" HorizontalAlignment="Center" VerticalAlignment="Top" Margin="0,325,23,0" FontSize="16"/>
        <lvc:CartesianChart x:Name="chartRenovationRecommendation" Series="{Binding SeriesCollectionRenovationRecommedation}" Grid.Column="2" Grid.Row="2" Grid.RowSpan="2" HorizontalAlignment="Left" Height="220" VerticalAlignment="Bottom" Width="350" Margin="42,0,0,154" FontSize="14">

	        <lvc:CartesianChart.AxisX>
		        <lvc:Axis x:Name="labelXAxisRenovationRecommendation" Labels="{Binding ChartPeriodTime}"></lvc:Axis>
	        </lvc:CartesianChart.AxisX>

	        <lvc:CartesianChart.AxisY>
		        <lvc:Axis Title="Renovation recommedation count" MinValue="0" MaxValue="10"></lvc:Axis>
	        </lvc:CartesianChart.AxisY>

        </lvc:CartesianChart> 
        */


        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (reviewService.IsSuperGuest(Guest1))
            {
                superType = " (Super guest)";
            }

            return superType;
        }

        private void SetUsernameHeader()
        {
            Notification = reviewService.Guest1HasNotification();
            CheckNotification();
            usernameAndSuperGuest.Text = $"{Guest1}";
            superGuest.Text = $"{CheckSuperType()}";
        }


        void LoadingRowForDgShowOwnerReviews(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private bool comboBoxClicked = false;
        private bool itemClicked = false;

        private void CBPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            comboBoxClicked = true;
        }

        private void CBCreateReviewDropDownClosed(object sender, EventArgs e)
        {
            if (comboBoxClicked && itemClicked)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

                if (selectedItem.Content.ToString() == "Create review")
                {
                    GoToCreateReview(sender, null);
                }
                else if (selectedItem.Content.ToString() == "Reviews")
                {
                    GoToShowOwnerReviews(sender, null);
                }
                else if (selectedItem.Content.ToString() == "Requests")
                {
                    GoToGuest1Requests(sender, null);
                }
            }

            comboBoxClicked = false;
            itemClicked = false;
        }

        private void CBSuperGuestDropDownClosed(object sender, EventArgs e)
        {
            if (comboBoxClicked && itemClicked)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

                if (selectedItem.Content.ToString() == "Super-guest")
                {
                    GoToSearchAndShowAccommodations(sender, null);
                }
                else if (selectedItem.Content.ToString() == "Logout")
                {
                    GoToLogout(sender, null);
                }
            }

            comboBoxClicked = false;
            itemClicked = false;
        }

        private void CBItemPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            itemClicked = true;
        }

        private void GoToAnywhereAnytime(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1AnywhereAnytime(Guest1, this));
        }

        private void GoToShowOwnerReviews(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowOwnerReviews(Guest1, this));
        }

        //private void GoToGuest1Start(object sender, RoutedEventArgs e)
        //{
        //    NavigationService?.Navigate(new Guest1Start(Guest1, this));
        //}

        private void GoToForum(object sender, RoutedEventArgs e)
        {
            //NavigationService?.Navigate(new Guest1Forum(Guest1, this));
        }

        private void GoToSearchAndShowAccommodations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SearchAndShowAccommodations(Guest1, this));
        }

        private void GoToGuest1GenerateReport(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1GenerateReport(Guest1, this));
        }

        private void GoToShowReservations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowReservationsView(Guest1, this, this.NavigationService));


        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateReview(Guest1, this));
        }

        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1RequestsView(Guest1, this, this.NavigationService));
        }

        private void GoToShowGuest1Notifications(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowGuest1NotificationsView(Guest1, this, this.NavigationService));
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);

            LoginForm window = new LoginForm();
            window.Show();
            currentWindow.Close();
        }

        private void SetComboBoxes(Page page)
        {
            if (page is SearchAndShowAccommodations searchAndShowPage)
            {
                var comboBox = searchAndShowPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = searchAndShowPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is AccommodationReservation accommodationReservationPage)
            {
                //var comboBox = accommodationReservationPage.CBCreateReview;
                //if (comboBox != null)
                //{
                //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                //}

                //comboBox = accommodationReservationPage.CBSuperGuest;
                //if (comboBox != null)
                //{
                //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                //}
            }
            else if (page is CreateReservationReschedulingRequest createReschedulingRequestPage)
            {
                var comboBox = createReschedulingRequestPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = createReschedulingRequestPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is CreateReview createReviewPage)
            {
                //var comboBox = createReviewPage.CBCreateReview;
                //if (comboBox != null)
                //{
                //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                //}

                //comboBox = createReviewPage.CBSuperGuest;
                //if (comboBox != null)
                //{
                //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                //}
            }
            //else if (page is Guest1RequestPreview guest1RequestPreviewPage)
            //{
            //    var comboBox = guest1RequestPreviewPage.CBCreateReview;
            //    if (comboBox != null)
            //    {
            //        CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    }

            //    comboBox = guest1RequestPreviewPage.CBSuperGuest;
            //    if (comboBox != null)
            //    {
            //        CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    }
            //}
            //else if (page is Guest1Requests guest1RequestsPage)
            //{
            //    var comboBox = guest1RequestsPage.CBCreateReview;
            //    if (comboBox != null)
            //    {
            //        CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    }

            //    comboBox = guest1RequestsPage.CBSuperGuest;
            //    if (comboBox != null)
            //    {
            //        CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    }
            //}
            //else if (page is ShowGuest1Notifications showGuest1NotificationsPage)
            //{
            //    var comboBox = showGuest1NotificationsPage.CBCreateReview;
            //    if (comboBox != null)
            //    {
            //        CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    }

            //    comboBox = showGuest1NotificationsPage.CBSuperGuest;
            //    if (comboBox != null)
            //    {
            //        CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    }
            //}
            else if (page is ShowOwnerReviews showOwnerReviewsPage)
            {
                //var comboBox = showOwnerReviewsPage.CBCreateReview;
                //if (comboBox != null)
                //{
                //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                //}

                //comboBox = showOwnerReviewsPage.CBSuperGuest;
                //if (comboBox != null)
                //{
                //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                //}
            }
            //else if (page is ShowReservations showReservationsPage)
            //{
            //    var comboBox = showReservationsPage.CBCreateReview;
            //    if (comboBox != null)
            //    {
            //        CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    }

            //    comboBox = showReservationsPage.CBSuperGuest;
            //    if (comboBox != null)
            //    {
            //        CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    }
            //}
        }

    }
}
