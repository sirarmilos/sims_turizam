using GalaSoft.MvvmLight.Command;
using InitialProject.DTO;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Prism.Commands;


namespace InitialProject.View
{

    public partial class Guest1ForumViewModel : Page, INotifyPropertyChanged//, IObserver
    {
        private readonly ForumService forumService;

        public List<ShowGuest1ForumsDTO> ShowGuest1ForumsDTOs
        {
            get;
            set;
        }

        public ShowGuest1ForumsDTO ShowGuest1ForumsDTO
        {
            get;
            set;
        }

        public NavigationService NavService { get; set; }

        private string guest1;
        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
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

        //public UpcommingToursVIewModel(int id, NavigationService navService)
        //{
        //    ActiveGuide = new UserService().GetById(id);
        //    TourOccurrenceService = new TourOccurrenceService();
        //    TourOccurrenceService.Subscribe(this);
        //    TourOccurrences = new ObservableCollection<TourOccurrence>(TourOccurrenceService.GetUpcomingToursForGuide(ActiveGuide.Id));
        //    NavService = navService;
        //    CancelCommand = new ButtonCommandNoParameter(CancelTour);
        //    CreateCommand = new ButtonCommandNoParameter(CreateNewTour);
        //    UndoCancelCommand = new ButtonCommandNoParameter(UndoCancelTour);
        //    CanceledTour = -1;
        //}

        Page ViewPage { get; set; } 

        public Guest1ForumViewModel(string username, Guest1ForumView guest1ForumView, Page page, NavigationService navService) 
        {
            Guest1 = username;

            forumService = new ForumService(Guest1);

            ShowGuest1ForumsDTOs = forumService.FindGuest1Forums();

            ReadMoreCommand = new RelayCommand<ShowGuest1ForumsDTO>(ReadMore);

            SetUsernameHeader();

            SetComboBoxes(page);

            NavService = navService;

            ViewPage = guest1ForumView;

            ComboBoxCommand = new DelegateCommand<object>(ComboBoxAction);
            ComboBoxSGCommand = new DelegateCommand<object>(ComboBoxSuperGuestAction);

            GoToGuest1CreateForumCommand = new RelayCommand(GoToGuest1CreateForum);
            GoToShowReservationsCommand = new RelayCommand(GoToShowReservations);
            GoToAnywhereAnytimeCommand = new RelayCommand(GoToAnywhereAnytime);
            GoToSearchAndShowAccommodationsCommand = new RelayCommand(GoToSearchAndShowAccommodations);
            GoToForumCommand = new RelayCommand(GoToForum);
    }


        public ICommand ReadMoreCommand
        {
            get;
            set;
        }

        private void ReadMore(ShowGuest1ForumsDTO showGuest1ForumsDTO)
        {
            ShowGuest1ForumsDTO = showGuest1ForumsDTO;

            GoToForumPreview(null, null);
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
            Notification = forumService.Guest1HasNotification();
            CheckNotification();
            UsernameAndSuperGuestText = Guest1;
            SuperGuestText = CheckSuperType();
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (forumService.IsSuperGuest(Guest1))
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
                    // Logika za otvaranje Create Review prozora
                    GoToCreateReview(null, null);

                }
                else if (parameter.Equals("Reviews"))
                {
                    // Logika za otvaranje Owner Reviews prozora
                    GoToShowOwnerReviews(null, null);

                }
                else if (parameter.Equals("Requests"))
                {
                    // Logika za otvaranje Guest1 Requests prozora
                    GoToGuest1Requests(null, null);

                }
            }
        }

        private void ComboBoxSuperGuestAction(object parameter)
        {
            if (parameter != null)
            {
                if (parameter.Equals("Super-guest"))
                {
                    // Logika za otvaranje Owner Reviews prozora
                    GoToShowSuperGuest(null, null);

                }
                else if (parameter.Equals("Logout"))
                {
                    // Logika za otvaranje Guest1 Requests prozora
                    GoToLogout(null, null);

                }
            }
        }


        // OLD 
        //private bool comboBoxClicked = false;
        //private bool itemClicked = false;

        //private void CBPreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    comboBoxClicked = true;
        //}

        //private void CBCreateReviewDropDownClosed(object sender, EventArgs e)
        //{
        //    if (comboBoxClicked && itemClicked)
        //    {
        //        ComboBox comboBox = (ComboBox)sender;
        //        ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

        //        if (selectedItem.Content.ToString() == "Create review")
        //        {
        //            GoToCreateReview(sender, null);
        //        }
        //        else if (selectedItem.Content.ToString() == "Reviews")
        //        {
        //            GoToShowOwnerReviews(sender, null);
        //        }
        //        else if (selectedItem.Content.ToString() == "Requests")
        //        {
        //            GoToGuest1Requests(sender, null);
        //        }
        //    }

        //    comboBoxClicked = false;
        //    itemClicked = false;
        //}

        //private void CBItemPreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    itemClicked = true;
        //}

        //private void CBSuperGuestDropDownClosed(object sender, EventArgs e)
        //{
        //    if (comboBoxClicked && itemClicked)
        //    {
        //        ComboBox comboBox = (ComboBox)sender;
        //        ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

        //        if (selectedItem.Content.ToString() == "Super-guest")
        //        {
        //            GoToSearchAndShowAccommodations(sender, null);
        //        }
        //        else if (selectedItem.Content.ToString() == "Logout")
        //        {
        //            GoToLogout(sender, null);
        //        }
        //    }

        //    comboBoxClicked = false;
        //    itemClicked = false;
        //}

        // ------------------------------

        public ICommand GoToShowOwnerReviewsCommand { get; set; }
        private void GoToShowOwnerReviews(object sender, RoutedEventArgs e)
        {
            NavService?.Navigate(new ShowOwnerReviews(Guest1, this));
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


        public ICommand GoToGuest1CreateForumCommand { get; set;  }

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
            //{
            //    var comboBox = searchAndShowPage.SelectedComboBox1Index;
            //    if (comboBox != null)
            //    {
            //        SelectedComboBox1Index = comboBox;
            //    }

            //    comboBox = searchAndShowPage.SelectedComboBox2Index;
            //    if (comboBox != null)
            //    {
            //        SelectedComboBox2Index = comboBox;
            //    }
            //}
            
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
            //else if (page is AccommodationReservation accommodationReservationPage)
            //{
            //    //var comboBox = accommodationReservationPage.CBCreateReview;
            //    //if (comboBox != null)
            //    //{
            //    //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    //}

            //    //comboBox = accommodationReservationPage.CBSuperGuest;
            //    //if (comboBox != null)
            //    //{
            //    //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    //}
            //}
            //else if (page is CreateReservationReschedulingRequest createReschedulingRequestPage)
            //{
            //    var comboBox = createReschedulingRequestPage.CBCreateReview;
            //    if (comboBox != null)
            //    {
            //        CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    }

            //    comboBox = createReschedulingRequestPage.CBSuperGuest;
            //    if (comboBox != null)
            //    {
            //        CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    }
            //}
            //else if (page is CreateReview createReviewPage)
            //{
            //    //var comboBox = createReviewPage.CBCreateReview;
            //    //if (comboBox != null)
            //    //{
            //    //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    //}

            //    //comboBox = createReviewPage.CBSuperGuest;
            //    //if (comboBox != null)
            //    //{
            //    //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    //}
            //}
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
            //else if (page is ShowOwnerReviews showOwnerReviewsPage)
            //{
            //    //var comboBox = showOwnerReviewsPage.CBCreateReview;
            //    //if (comboBox != null)
            //    //{
            //    //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    //}

            //    //comboBox = showOwnerReviewsPage.CBSuperGuest;
            //    //if (comboBox != null)
            //    //{
            //    //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    //}
            //}
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

    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
