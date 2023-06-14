using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using Prism.Commands;
using GalaSoft.MvvmLight.Command;

namespace InitialProject.View
{
    public partial class Guest1RequestPreviewViewModel : Page, INotifyPropertyChanged
    {
        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;
        private string accommodationName;
        private DateTime oldStartDate;
        private DateTime oldEndDate;
        private DateTime newStartDate;
        private DateTime newEndDate;
        private string guest1;

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

        public DateTime OldStartDate
        {
            get { return oldStartDate; }
            set
            {
                oldStartDate = value;
            }
        }

        public DateTime OldEndtDate
        {
            get { return oldEndDate; }
            set
            {
                oldEndDate = value;
            }
        }

        public DateTime NewStartDate
        {
            get { return newStartDate; }
            set
            {
                newStartDate = value;
            }
        }

        public DateTime NewEndDate
        {
            get { return newEndDate; }
            set
            {
                newEndDate = value;
            }
        }

        public Guest1RebookingRequestsDTO Guest1RebookingRequestsDTO
        {
            get;
            set;
        }


        public string Caller { get; set; }


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

        private Visibility isNotificationVisible;

        public Visibility IsNotificationVisible
        {
            get { return isNotificationVisible; }
            set
            {
                isNotificationVisible = value;
                RaisePropertyChanged();
            }
        }

        private Visibility isRegularBellVisible;

        public Visibility IsRegularBellVisible
        {
            get { return isRegularBellVisible; }
            set
            {
                isRegularBellVisible = value;
                RaisePropertyChanged();
            }
        }


        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NavigationService NavService { get; set; }

        private void CheckNotification()
        {
            if (Notification)
            {
                IsNotificationVisible = Visibility.Visible;
                IsRegularBellVisible = Visibility.Collapsed;
            }
            else
            {
                IsNotificationVisible = Visibility.Collapsed;
                IsRegularBellVisible = Visibility.Visible;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Page ViewPage { get; set; }

        public Guest1RequestPreviewViewModel(string username, Guest1RequestPreviewView guest1RequestPreviewView, Page page, NavigationService navService, Guest1RebookingRequestsDTO guest1RebookingRequestsDTO, string callingWindow)
        {
            Guest1 = username;

            Caller = callingWindow;

            reservationReschedulingRequestService = new ReservationReschedulingRequestService(Guest1);

            //Guest1RebookingRequestsDTOs = reservationReschedulingRequestService.FindAllByGuest1Username();
            Guest1RebookingRequestsDTO = guest1RebookingRequestsDTO;

            SetUsernameHeader();

            SetComboBoxes(page);



            SetUsernameHeader();

            SetComboBoxes(page);

            NavService = navService;

            ViewPage = guest1RequestPreviewView;

            ComboBoxCommand = new DelegateCommand<object>(ComboBoxAction);
            ComboBoxSGCommand = new DelegateCommand<object>(ComboBoxSuperGuestAction);

            GoToShowReservationsCommand = new RelayCommand(GoToShowReservations);
            GoToAnywhereAnytimeCommand = new RelayCommand(GoToAnywhereAnytime);
            GoToSearchAndShowAccommodationsCommand = new RelayCommand(GoToSearchAndShowAccommodations);
            GoToForumCommand = new RelayCommand(GoToForum);

            BackCommand = new RelayCommand(ReturnBackToCaller);
        }


        public ICommand BackCommand { get; set; }
        public void ReturnBackToCaller()
        {
            if (Caller.Equals("Guest1Requests"))
                GoToGuest1Requests(null, null);
            else
                GoToShowGuest1Notifications();
        }




        private string usernameAndSuperGuestText;

        public string UsernameAndSuperGuestText
        {
            get { return usernameAndSuperGuestText; }
            set
            {
                usernameAndSuperGuestText = value;
                RaisePropertyChanged();
            }
        }

        private string superGuestText;

        public string SuperGuestText
        {
            get { return superGuestText; }
            set
            {
                superGuestText = value;
                RaisePropertyChanged();
            }
        }

        private void SetUsernameHeader()
        {
            Notification = reservationReschedulingRequestService.Guest1HasNotification();
            CheckNotification();
            UsernameAndSuperGuestText = Guest1;
            SuperGuestText = CheckSuperType();
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (reservationReschedulingRequestService.IsSuperGuest(Guest1))
            {
                superType = "(Super guest)";
            }

            return superType;
        }


        // CB 1 -----------------------------------------------
        // view model

        public DelegateCommand<object> ComboBoxCommand { get; }
        public DelegateCommand<object> ComboBoxSGCommand { get; }


        private void ComboBoxAction(object parameter)
        {
            if (parameter != null)
            {
                if (parameter.Equals("Create review"))
                {
                    SelectedComboBox1Index = 0;
                    GoToCreateReview(null, null);
                }
                else if (parameter.Equals("Reviews"))
                {
                    SelectedComboBox1Index = 1;
                    GoToShowOwnerReviews(null, null);
                }
                else if (parameter.Equals("Requests"))
                {
                    SelectedComboBox1Index = 2;
                    GoToGuest1Requests(null,null);
                }
            }
        }

        private void ComboBoxSuperGuestAction(object parameter)
        {
            if (parameter != null)
            {
                if (parameter.Equals("Super-guest"))
                {
                    GoToShowSuperGuest(null, null);

                }
                else if (parameter.Equals("Logout"))
                {
                    GoToLogout(null, null);

                }
            }
        }


        public ICommand GoToShowOwnerReviewsCommand { get; set; }
        private void GoToShowOwnerReviews(object sender, RoutedEventArgs e)
        {
            NavService?.Navigate(new ShowOwnerReviewsView(Guest1, this, NavService));

        }

        public ICommand GoToForumCommand { get; set; }
        private void GoToForum()
        {
            NavService.Navigate(new Guest1ForumView(Guest1, this, NavService));
        }

        public ICommand GoToSearchAndShowAccommodationsCommand { get; set; }
        private void GoToSearchAndShowAccommodations()
        {
            NavService?.Navigate(new SearchAndShowAccommodations(Guest1, this));
        }


        public ICommand GoToGuest1CreateForumCommand { get; set; }

        private void GoToGuest1CreateForum()
        {
            NavService?.Navigate(new Guest1CreateForum(Guest1, this));
        }


        private void GoToForumPreview(object sender, RoutedEventArgs e)
        {
            NavService?.Navigate(new Guest1ForumPreview(Guest1, this));
        }

        public ICommand GoToAnywhereAnytimeCommand { get; set; }
        private void GoToAnywhereAnytime()
        {
            NavService?.Navigate(new Guest1AnywhereAnytime(Guest1, this));
        }

        public ICommand GoToShowReservationsCommand { get; set; }
        private void GoToShowReservations()
        {
            NavService?.Navigate(new ShowReservationsView(Guest1, this, NavService));
        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            NavService?.Navigate(new CreateReview(Guest1, this));
        }

        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
        {
            NavService?.Navigate(new Guest1RequestsView(Guest1, this, NavService));
        }

        private DelegateCommand goToShowGuest1NotificationsCommand;
        public DelegateCommand GoToShowGuest1NotificationsCommand => goToShowGuest1NotificationsCommand ?? (goToShowGuest1NotificationsCommand = new DelegateCommand(GoToShowGuest1Notifications));
        private void GoToShowGuest1Notifications()
        {
            NavService?.Navigate(new ShowGuest1NotificationsView(Guest1, this, NavService));
        }

        private void GoToShowSuperGuest(object sender, RoutedEventArgs e)
        {
            NavService?.Navigate(new ShowSuperGuest(Guest1, this));
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(ViewPage);

            LoginForm window = new LoginForm();
            window.Show();
            currentWindow.Close();
        }

        private int selectedComboBox1Index;
        public int SelectedComboBox1Index
        {
            get { return selectedComboBox1Index; }
            set
            {
                if (selectedComboBox1Index != value)
                {
                    selectedComboBox1Index = value;
                    OnPropertyChanged();
                }
            }
        }

        private int selectedComboBox2Index;
        public int SelectedComboBox2Index
        {
            get { return selectedComboBox2Index; }
            set
            {
                if (selectedComboBox2Index != value)
                {
                    selectedComboBox2Index = value;
                    OnPropertyChanged();
                }
            }
        }


        private void SetComboBoxes(Page page)
        {
            if (page is SearchAndShowAccommodations searchAndShowPage)
            {
                var comboBox = searchAndShowPage.CBCreateReview;
                if (comboBox != null)
                {
                    SelectedComboBox1Index = comboBox.SelectedIndex;
                }

                comboBox = searchAndShowPage.CBSuperGuest;
                if (comboBox != null)
                {
                    SelectedComboBox2Index = comboBox.SelectedIndex;
                }
            }
            else if (page is AccommodationReservation accommodationReservationPage)
            {
                var comboBox = accommodationReservationPage.CBCreateReview;
                if (comboBox != null)
                {
                    SelectedComboBox1Index = comboBox.SelectedIndex;
                }

                comboBox = accommodationReservationPage.CBSuperGuest;
                if (comboBox != null)
                {
                    SelectedComboBox2Index = comboBox.SelectedIndex;
                }
            }
            else if (page is CreateReservationReschedulingRequest createReschedulingRequestPage)
            {
                var comboBox = createReschedulingRequestPage.CBCreateReview;
                if (comboBox != null)
                {
                    SelectedComboBox1Index = comboBox.SelectedIndex;
                }

                comboBox = createReschedulingRequestPage.CBSuperGuest;
                if (comboBox != null)
                {
                    SelectedComboBox2Index = comboBox.SelectedIndex;
                }
            }
            else if (page is CreateReview createReviewPage)
            {
                var comboBox = createReviewPage.CBCreateReview;
                if (comboBox != null)
                {
                    SelectedComboBox1Index = comboBox.SelectedIndex;
                }

                comboBox = createReviewPage.CBSuperGuest;
                if (comboBox != null)
                {
                    SelectedComboBox2Index = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1GenerateReport guest1GenerateReport)
            {
                var comboBox = guest1GenerateReport.CBCreateReview;
                if (comboBox != null)
                {
                    SelectedComboBox1Index = comboBox.SelectedIndex;
                }

                comboBox = guest1GenerateReport.CBSuperGuest;
                if (comboBox != null)
                {
                    SelectedComboBox2Index = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1AnywhereAnytime anywhereAnytime)
            {
                var comboBox = anywhereAnytime.CBCreateReview;
                if (comboBox != null)
                {
                    SelectedComboBox1Index = comboBox.SelectedIndex;
                }

                comboBox = anywhereAnytime.CBSuperGuest;
                if (comboBox != null)
                {
                    SelectedComboBox2Index = comboBox.SelectedIndex;
                }
            }
            else if (page is ShowSuperGuest showSuperGuest)
            {
                var comboBox = showSuperGuest.CBCreateReview;
                if (comboBox != null)
                {
                    SelectedComboBox1Index = comboBox.SelectedIndex;
                }

                comboBox = showSuperGuest.CBSuperGuest;
                if (comboBox != null)
                {
                    SelectedComboBox2Index = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1CreateForum guest1CreateForum)
            {
                var comboBox = guest1CreateForum.CBCreateReview;
                if (comboBox != null)
                {
                    SelectedComboBox1Index = comboBox.SelectedIndex;
                }

                comboBox = guest1CreateForum.CBSuperGuest;
                if (comboBox != null)
                {
                    SelectedComboBox2Index = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1ForumPreview guest1ForumPreview)
            {
                var comboBox = guest1ForumPreview.CBCreateReview;
                if (comboBox != null)
                {
                    SelectedComboBox1Index = comboBox.SelectedIndex;
                }

                comboBox = guest1ForumPreview.CBSuperGuest;
                if (comboBox != null)
                {
                    SelectedComboBox2Index = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1RequestPreviewViewModel guest1RequestPreviewPage) // 6. MVVM 
            {
                SelectedComboBox1Index = guest1RequestPreviewPage.SelectedComboBox1Index;
                SelectedComboBox2Index = guest1RequestPreviewPage.SelectedComboBox2Index;
            }
            else if (page is Guest1RequestsViewModel guest1RequestsPage) // 5. MVVM
            {
                SelectedComboBox1Index = guest1RequestsPage.SelectedComboBox1Index;
                SelectedComboBox2Index = guest1RequestsPage.SelectedComboBox2Index;
            }
            else if (page is ShowGuest1NotificationsViewModel showGuest1NotificationsPage) // 2. MVVM
            {
                SelectedComboBox1Index = showGuest1NotificationsPage.SelectedComboBox1Index;
                SelectedComboBox2Index = showGuest1NotificationsPage.SelectedComboBox2Index;
            }
            else if (page is ShowOwnerReviewsViewModel showOwnerReviewsPage) // 4. MVVM
            {
                SelectedComboBox1Index = showOwnerReviewsPage.SelectedComboBox1Index;
                SelectedComboBox2Index = showOwnerReviewsPage.SelectedComboBox2Index;
            }
            else if (page is ShowReservationsViewModel showReservationsPage) // 1. MVVM
            {
                SelectedComboBox1Index = showReservationsPage.SelectedComboBox1Index;
                SelectedComboBox2Index = showReservationsPage.SelectedComboBox2Index;
            }
            else if (page is Guest1ForumViewModel guest1ForumViewModel) // 3. MVVM
            {
                SelectedComboBox1Index = guest1ForumViewModel.SelectedComboBox1Index;
                SelectedComboBox2Index = guest1ForumViewModel.SelectedComboBox2Index;
            }
        }

    }
}
