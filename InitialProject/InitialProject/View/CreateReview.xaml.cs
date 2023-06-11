using InitialProject.DTO;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Net.Mime;
using System.Net;
using System.Reflection;
using System.Windows.Navigation;
using InitialProject.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Windows.Data;

namespace InitialProject.View
{
    public partial class CreateReview : Page, INotifyPropertyChanged 
    {
        private readonly ReviewService reviewService;
        private string guest1;
        private int cleanliness;
        private int staff;
        private int comfort;
        private int valueForMoney;
        private string comment;
        private string image;
        private string recommendationLevel;
        private string recommendationComment;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public List<CreateReviewDTO> CreateReviewDTOs
        {
            get;
            set;
        }

        public CreateReviewDTO SelectedAccommodation 
        {
            get;
            set;
        }


        public int Cleanliness
        {
            get { return cleanliness; }
            set { cleanliness = value; OnPropertyChanged(nameof(Cleanliness)); }
        }

        public List<int> CleanlinessValues { get; set; } = new List<int> { 1, 2, 3, 4, 5 };


        public int Staff
        {
            get { return staff; }
            set { staff = value; OnPropertyChanged(nameof(Staff)); }
        }

        public List<int> StaffValues { get; set; } = new List<int> { 1, 2, 3, 4, 5 };

        public int Comfort
        {
            get { return comfort; }
            set { comfort = value; OnPropertyChanged(nameof(Comfort)); }
        }

        public List<int> ComfortValues { get; set; } = new List<int> { 1, 2, 3, 4, 5 };

        public int ValueForMoney
        {
            get { return valueForMoney; }
            set { valueForMoney = value; OnPropertyChanged(nameof(ValueForMoney)); }
        }

        public List<int> ValueForMoneyValues { get; set; } = new List<int> { 1, 2, 3, 4, 5 };


        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
            }
        }

        public string RecommendationLevel
        {
            get { return recommendationLevel; }
            set
            {
                recommendationLevel = value;
            }
        }


        public string RecommendationComment
        {
            get { return recommendationComment; }
            set
            {
                recommendationComment = value;
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

        public CreateReview(string guest1, Page page)
        {
            InitializeComponent();
            DataContext = this;
            Guest1 = guest1;
            reviewService = new ReviewService(Guest1);

            SetUsernameHeader();

            CreateReviewDTOs = new List<CreateReviewDTO>();

            CreateReviewDTOs = reviewService.FindAllReviewsToRate();

            ReviewButtonCommand = new RelayCommand<CreateReviewDTO>(OpenReview);

            SetImagePreviewer();

            SetComboBoxes(page);

            SetReviewComboBoxes();
        }

        private void SetReviewComboBoxes()
        {
            Cleanliness = 3;
            Staff = 3;
            Comfort = 3;
            ValueForMoney = 3;
        }


        // start window
        public ICommand ReviewButtonCommand { get; set; }
        private void OpenReview(CreateReviewDTO createReviewDTO)
        {
            SelectedAccommodation = createReviewDTO;

            CreateReviewWindow.Visibility = Visibility.Collapsed;
            RateAccommodationsTitle.Visibility = Visibility.Collapsed;

            FillTheReviewTitle.Visibility = Visibility.Visible;
            FillReview_Window1.Visibility = Visibility.Visible;
        }

        // 1st window
        private void GoForwardToAddImage_Window2(object sender, RoutedEventArgs e)
        {
            FillReview_Window1.Visibility = Visibility.Collapsed;

            AddImage_Window2.Visibility = Visibility.Visible;
        }

        // 2nd window
        private void GoForwardToFillReview_Window3(object sender, RoutedEventArgs e)
        {
                AddImage_Window2.Visibility = Visibility.Collapsed;

                FillReview_Window3.Visibility = Visibility.Visible;
        }

        private void GoBackToFillReview_Window1(object sender, RoutedEventArgs e)
        {
             AddImage_Window2.Visibility = Visibility.Collapsed;

             FillReview_Window1.Visibility = Visibility.Visible;
        }

        // 3rd window
        private void GoForwardToFillReview_Window4(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Comment))
            {
                CommentErrorMessage.Text = "You haven't input a comment.";
                CommentErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                CommentErrorMessage.Visibility = Visibility.Collapsed;
                FillReview_Window3.Visibility = Visibility.Collapsed;

                FillReview_Window4.Visibility = Visibility.Visible;
            }
        }

        private void GoBackToFillReview_Window2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Comment))
            {
                CommentErrorMessage.Text = "You haven't input a comment.";
                CommentErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                CommentErrorMessage.Visibility = Visibility.Collapsed;
                FillReview_Window3.Visibility = Visibility.Collapsed;

                AddImage_Window2.Visibility = Visibility.Visible;
            }
        }

        // 4th window
        private void GoBackToFillReview_Window3(object sender, RoutedEventArgs e)
        {
            FillReview_Window4.Visibility = Visibility.Collapsed;

            FillReview_Window3.Visibility = Visibility.Visible;
        }

        // redirection depends on whether the radio button is selected
        private void GoForwardToFillReview_Window56(object sender, RoutedEventArgs e)
        {
            if (IsAnyRadioButtonSelected())
            {
                FillReview_Window4.Visibility = Visibility.Collapsed;
                FillReview_Window5.Visibility = Visibility.Visible;
            }
            else
            {
                FillReview_Window4.Visibility = Visibility.Collapsed;
                FillReview_Window6.Visibility = Visibility.Visible;
            }
        }

        private bool IsAnyRadioButtonSelected()
        {
            if (RecommendationLevel == null) 
                return false;
            return true;
        }


        // 5th window
        private void GoBackToFillReview_Window4(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RecommendationComment))
            {
                RecommendationCommentErrorMessage.Text = "You haven't input a comment.";
                RecommendationCommentErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                RecommendationCommentErrorMessage.Visibility = Visibility.Collapsed;
                FillReview_Window5.Visibility = Visibility.Collapsed;

                FillReview_Window4.Visibility = Visibility.Visible;
            }
        }

        private void GoForwardToFillReview_Window7(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RecommendationComment))
            {
                RecommendationCommentErrorMessage.Text = "You haven't input a comment.";
                RecommendationCommentErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                RecommendationCommentErrorMessage.Visibility = Visibility.Collapsed;
                FillReview_Window5.Visibility = Visibility.Collapsed;

                FillReview_Window7.Visibility = Visibility.Visible;

                SaveReview();
            }
        }

        // 6th window
        private void GoBackToFillReview_Window4From6(object sender, RoutedEventArgs e)
        {
            FillReview_Window6.Visibility = Visibility.Collapsed;

            FillReview_Window4.Visibility = Visibility.Visible;
        }

        private void GoForwardToFillReview_Window7From6(object sender, RoutedEventArgs e)
        {
            FillReview_Window6.Visibility = Visibility.Collapsed;

            FillReview_Window7.Visibility = Visibility.Visible;

            SaveReview();
        }


        private void SetUsernameHeader()
        {
            Notification = reviewService.Guest1HasNotification();
            CheckNotification();
            usernameAndSuperGuest.Text = $"{Guest1}";
            superGuest.Text = $"{CheckSuperType()}";
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (reviewService.IsSuperGuest(Guest1))
            {
                superType = "(Super guest)";
            }

            return superType;
        }

        private void SaveReview()
        {

            SaveNewCreateReviewDTO saveNewCreateReviewDTO =
                new SaveNewCreateReviewDTO(
                    SelectedAccommodation.ReservationId,
                    Cleanliness,
                    Staff,
                    Comfort,
                    ValueForMoney,
                    Comment,
                    Images,
                    RecommendationLevel,
                    RecommendationComment);

            if (IsAnyRadioButtonSelected()) // it means that renovation is filled out
                reviewService.SaveNewReviewWithRenovation(saveNewCreateReviewDTO);
            else
                reviewService.SaveNewReview(saveNewCreateReviewDTO);


            reviewService.CheckSuperOwner(saveNewCreateReviewDTO.ReservationId);

        }


        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                Image = imagePath;

                string fileExtension = System.IO.Path.GetExtension(imagePath);

                if (IsImageFile(fileExtension))
                {
                    if (CheckErrorImageAlreadyExists() == false)
                    {
                        CurrentImage = imagePath;
                        SetImagePreviewer();
                    }
                    else
                    {
                        ImageSliderErrorMessage.Text = "You have already added this image.";
                        ImageSliderErrorMessage.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    ImageSliderErrorMessage.Text = "The selected file is not an image.";
                    ImageSliderErrorMessage.Visibility = Visibility.Visible;
                }
            }
        }

        private bool IsImageFile(string fileExtension)
        {
            string[] imageExtensions = { ".png", ".jpeg", ".jpg" };
            return imageExtensions.Contains(fileExtension.ToLower());
        }


        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _images;
        private int _currentIndex;
        private string _currentImage;

        public ObservableCollection<string> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged(nameof(Images));
                UpdateCurrentImage();
            }
        }

        public string CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        public ICommand PreviousImageCommand { get; private set; }
        public ICommand NextImageCommand { get; private set; }

        private void SetImagePreviewer()
        {
            if (Images == null)
            {
                ImageSlider.Visibility = Visibility.Collapsed;
                Images = new ObservableCollection<string>();
            }
            else
            { 
                Images.Add(Image);
                ImageSlider.Visibility = Visibility.Visible;
                ImageSliderErrorMessage.Visibility = Visibility.Collapsed;
            }

            _currentIndex = 0;
            PreviousImageCommand = new RelayCommand(PreviousImage);
            NextImageCommand = new RelayCommand(NextImage);
        }

        private void PreviousImage()
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = Images.Count - 1;
            }
            UpdateCurrentImage();
        }

        private void NextImage()
        {
            _currentIndex++;
            if (_currentIndex >= Images.Count)
            {
                _currentIndex = 0;
            }
            UpdateCurrentImage();
        }

        private void UpdateCurrentImage()
        {
            if (Images != null && Images.Count > 0)
            {
                CurrentImage = Images[_currentIndex];
            }
            else
            {
                CurrentImage = null;
            }
            OnPropertyChanged(nameof(CurrentImage));
        }

        private bool CheckErrorImageAlreadyExists()
        {
            return Images.Any(x => string.Equals(x, Image, StringComparison.OrdinalIgnoreCase));
        }


        private bool comboBoxClicked = false;
        private bool itemClicked = false;

        private void CBPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            comboBoxClicked = true;
        }


        public string SelectedCreateReviewCBItem { get; set; } 
        private void CBCreateReviewDropDownClosed(object sender, EventArgs e)
        {
            if (comboBoxClicked && itemClicked)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

                if (selectedItem.Content.ToString() == "Create review")
                {
                    SelectedCreateReviewCBItem = selectedItem.Content.ToString();
                    GoToCreateReview(sender, null);
                }
                else if (selectedItem.Content.ToString() == "Reviews")
                {
                    SelectedCreateReviewCBItem = selectedItem.Content.ToString();

                    //NavigationService?.Navigate(new Guest1GenerateReport(Guest1, this));
                    GoToShowOwnerReviews(sender, null);

                }
                else if (selectedItem.Content.ToString() == "Requests")
                {
                    SelectedCreateReviewCBItem = selectedItem.Content.ToString();
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
                    GoToShowSuperGuest(sender, null);
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
        private void GoToForum(object sender, RoutedEventArgs e)
        {
            //NavigationService?.Navigate(new Guest1Forum(Guest1, this));
        }


        private void GoToShowSuperGuest(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowSuperGuest(Guest1, this));
        }

        private void GoToAnywhereAnytime(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1AnywhereAnytime(Guest1, this));
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

    public class RadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == parameter?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return parameter?.ToString();
            }
            return Binding.DoNothing;
        }
    }
}
