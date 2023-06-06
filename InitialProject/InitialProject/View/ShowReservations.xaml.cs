//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Reflection.PortableExecutable;
//using System.Windows;
//using System.Windows.Input;
//using GalaSoft.MvvmLight.Command;
//using InitialProject.DTO;
//using InitialProject.Model;
//using InitialProject.Service;
//using InitialProject.ViewModel;

using InitialProject.DTO;
using InitialProject.Service;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Prism.Commands;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Controls;
using InitialProject.Model;
using System.Windows.Navigation;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.ComponentModel;

namespace InitialProject.View
{
    public partial class ShowReservations : Page, INotifyPropertyChanged
    {
        private ReservationService reservationService;
        private string accommodationName;
        private DateTime startDate;
        private DateTime endDate;
        private int guestsNumber;
        private string guest1;


        public int GuestsNumber
        {
            get { return guestsNumber; }
            set
            {
                guestsNumber = value;
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
            }
        }

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public string AccommodationName
        {
            get { return accommodationName; }
            set
            {
                accommodationName = value;
            }
        }

        public ObservableCollection<ShowReservationDTO> ShowReservationDTOs
        {
            get;
            set;
        }

        //private bool notification;
        //public bool Notification
        //{
        //    get { return notification; }
        //    set
        //    {
        //        notification = value;
        //    }
        //}



        //public string NotificationEnable
        //{
        //    get;
        //    set;
        //}


        public ICommand CancelCommand { get; set; }

        public ICommand RescheduleCommand { get; set; }

        //public DelegateCommand GoToGuest1StartCommand { get; }

        //public DelegateCommand GoToSearchAndShowAccommodationsCommand { get; }

        //public DelegateCommand GoToCreateReviewCommand { get; }

        //public DelegateCommand GoToGuest1RequestsCommand { get; }

        //public DelegateCommand GoToLogoutCommand { get; }

        //public DelegateCommand CheckNotificationCommand { get; }
        private bool notification;
        public bool Notification
        {
            get { return notification; }
            set
            {
                notification = value;
            }
        }

        private Brush _labelColor;
        public Brush LabelColor
        {
            get { return _labelColor; }
            set
            {
                _labelColor = value;
                OnPropertyChanged(nameof(LabelColor));
            }
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ShowReservations(string username, Page page)
        {
            InitializeComponent();
            this.DataContext = this;
            Guest1 = username;
            reservationService = new ReservationService(Guest1);

            //GoToGuest1StartCommand = new DelegateCommand(GoToGuest1Start);
            //GoToSearchAndShowAccommodationsCommand = new DelegateCommand(GoToSearchAndShowAccommodations);
            //GoToCreateReviewCommand = new DelegateCommand(GoToCreateReview);
            //GoToGuest1RequestsCommand = new DelegateCommand(GoToGuest1Requests);
            //GoToLogoutCommand = new DelegateCommand(GoToLogout);

            SetUsernameHeader();

            ValidationMessage.Visibility = Visibility.Collapsed;

            //CheckNotificationCommand = new DelegateCommand(CheckNotification);

            ShowReservationDTOs = new ObservableCollection<ShowReservationDTO>(reservationService.FindAll(Guest1));
            CancelCommand = new RelayCommand<ShowReservationDTO>(Cancel);
            RescheduleCommand = new RelayCommand<ShowReservationDTO>(Reschedule);

            SetComboBoxes(page);
        }

        private void SetUsernameHeader()
        {
            Notification = reservationService.Guest1HasNotification();
            CheckNotification();
            usernameAndSuperGuest.Text = $"{Guest1}";
            superGuest.Text = $"{CheckSuperType()}";
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (reservationService.IsSuperGuest(Guest1))
            {
                superType = "(Super guest)";
            }

            return superType;
        }

        private void Cancel(ShowReservationDTO showReservationDTO)
        {
            bool isReservationCancelEligible = reservationService.IsRemoved(showReservationDTO);

            if (showReservationDTO.StartDate <= DateTime.Now)
            {
                //MessageBox.Show("Reservation is not eligible for cancellation since the start date is in the past.", "Error",
                    //MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!isReservationCancelEligible)
            {
                ValidationMessage.Text = "Reservation is not eligible for cancellation. Since the number of left cancelation days is 0.";
                ValidationMessage.Visibility = Visibility.Visible;
                ValidationBorder.Visibility = Visibility.Visible;
                LabelColor = Brushes.Red;
                //MessageBox.Show("Reservation is not eligible for cancellation. Since the number of left cancelation days is 0.", "Error",
                //MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ShowReservationDTOs.Remove(showReservationDTO);

            ValidationMessage.Text = "Reservation has been successfully canceled.";
            ValidationMessage.Visibility = Visibility.Visible;
            ValidationBorder.Visibility = Visibility.Visible;
            LabelColor = Brushes.Green;
        }

        private void Reschedule(ShowReservationDTO showReservationDTO)
        {

            if (showReservationDTO.StartDate < DateTime.Now)
            {
                MessageBox.Show("Reservation is not eligible for rescheduling since it already started.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NavigationService?.Navigate(new CreateReservationReschedulingRequest(showReservationDTO, Guest1, "ShowReservations", this));
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

        private void GoToShowOwnerReviews(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowOwnerReviews(Guest1, this));
        }

        //private void GoToGuest1Start(object sender, RoutedEventArgs e)
        //{
        //    NavigationService?.Navigate(new Guest1Start(Guest1, this));
        //}

        private void GoToSearchAndShowAccommodations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SearchAndShowAccommodations(Guest1, this));
        }

        private void GoToShowReservations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowReservations(Guest1, this));
        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateReview(Guest1, this));
        }

        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1Requests(Guest1, this));
        }

        private void GoToShowGuest1Notifications(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowGuest1Notifications(Guest1, this));
        }

        private void GoToForum(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1Forum(Guest1, this));
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
            else if (page is Guest1RequestPreview guest1RequestPreviewPage)
            {
                var comboBox = guest1RequestPreviewPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = guest1RequestPreviewPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1Requests guest1RequestsPage)
            {
                var comboBox = guest1RequestsPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = guest1RequestsPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is ShowGuest1Notifications showGuest1NotificationsPage)
            {
                var comboBox = showGuest1NotificationsPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = showGuest1NotificationsPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
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
            else if (page is ShowReservations showReservationsPage)
            {
                var comboBox = showReservationsPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = showReservationsPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
        }
    }

    public class DateTimeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                if (parameter is string parameterValue)
                {
                    if (parameterValue == "Past")
                    {
                        return dateTime < DateTime.Now ? Visibility.Collapsed : Visibility.Visible;
                    }
                    else if (parameterValue == "Future")
                    {
                        return dateTime > DateTime.Now ? Visibility.Collapsed : Visibility.Visible;
                    }
                }
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}




