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

namespace InitialProject.View
{
    public partial class CreateReview : Page
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

        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Images
        {
            get;
            set;
        }

        public string SelectedImage
        {
            get;
            set;
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
            set
            {
                cleanliness = value;
            }
        }

        public int Staff
        {
            get { return staff; }
            set
            {
                staff = value;
            }
        }

        public int Comfort
        {
            get { return comfort; }
            set
            {
                comfort = value;
            }
        }

        public int ValueForMoney
        {
            get { return valueForMoney; }
            set
            {
                valueForMoney = value;
            }
        }

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
                NotificationMenuItem.Visibility = Visibility.Visible;
            }
            else
            {
                NotificationMenuItem.Visibility = Visibility.Collapsed;
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

            if (CreateReviewDTOs.Count == 0)
            {
                MessageBox.Show("All accommodations are reviewed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
            else
            {
                MessageBox.Show("There are " + CreateReviewDTOs.Count + " acommodations left for you to rate.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            SetDefaultValue();
        }

        private void SetUsernameHeader()
        {
            Notification = reviewService.Guest1HasNotification();
            CheckNotification();
            usernameAndSuperGuest.Header = Guest1 + CheckSuperType();
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (reviewService.IsSuperGuest(Guest1))
            {
                superType = " (Super guest)";
            }

            return superType;
        }

        private void SaveReview(object sender, RoutedEventArgs e)
        {
            if (!IsValidationPassed())
            {
                MessageBox.Show("You haven't input a recommendation comment.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

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

            if (renovationCheckBox.IsChecked.GetValueOrDefault())
                reviewService.SaveNewReviewWithRenovation(saveNewCreateReviewDTO);
            else
                reviewService.SaveNewReview(saveNewCreateReviewDTO);


            reviewService.CheckSuperOwner(saveNewCreateReviewDTO.ReservationId);

            CreateReviewDTOs.Remove(SelectedAccommodation);

            dgCreateReview.Items.Refresh();

            SetDefaultValue();

            MessageBox.Show("You have successfully rated an accommodation", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            if (CreateReviewDTOs.Count == 0)
            {
                MessageBox.Show("All accommodations are rated.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                GoToSearchAndShowAccommodations(sender, e);
            }
        }

        public bool IsValidationPassed()
        {
            if (renovationCheckBox.IsChecked.GetValueOrDefault() && string.IsNullOrWhiteSpace(RecommendationComment)) return false;

            return true;
        }

        private void CancelReview(object sender, RoutedEventArgs e)
        {
            SetDefaultValue();
            dgCreateReview.SelectedItem = null; 
        }

        private void SetDefaultValue()
        {
            sliderCleanliness.Value = 3;
            sliderStaff.Value = 3;
            sliderComfort.Value = 3;
            sliderValueForMoney.Value = 3;
            Comment = "";
            tbComment.Text = "";
            Images = new ObservableCollection<string>();
            dgImages.ItemsSource = Images;
            buttonRate.IsEnabled = false;
        }

        private void RateButtonEnable(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedAccommodation == null)
            {
                buttonRate.IsEnabled = false;
            }
            else
            {
                buttonRate.IsEnabled = true;
            }
        }

        private void AddImageToList(object sender, RoutedEventArgs e)
        {
            if (CheckErrorUrlExists() == false)
            {
                MessageBox.Show("The image with the specified url does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (CheckErrorImageAlreadyExists() == false)
            {
                Images.Add(Image.ToString());
            }
            else
            {
                MessageBox.Show("You have already added this image.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckErrorUrlExists() 
        {
            string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".jfif" };
            Uri uriResult;
            if (Uri.TryCreate(Image.ToString(), UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                string extension = Path.GetExtension(uriResult.LocalPath).ToLower();
                if (string.IsNullOrEmpty(extension))
                {
                    try
                    {
                        using (var client = new WebClient())
                        {
                            var data = client.DownloadData(uriResult);
                            var contentType = client.ResponseHeaders["Content-Type"];
                            if (!string.IsNullOrEmpty(contentType) && contentType.StartsWith("image/"))
                            {
                                extension = "." + contentType.Substring("image/".Length);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                return allowedExtensions.Contains(extension);
            }
            return false;
        }

        private bool CheckErrorImageAlreadyExists()
        {
            return Images.Any(x => x.Equals(Image, StringComparison.OrdinalIgnoreCase));
        }

        void LoadingRowForDgCreateReview(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        void LoadingRowForDgImages(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void RemoveImageFromList(object sender, RoutedEventArgs e)
        {
            if (SelectedImage == null)
            {
                MessageBox.Show("Select the image you want to remove.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                dgImages.Items.Refresh();
                Images.Remove(SelectedImage);
            }
        }

        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var label = (Label)sender;
                Keyboard.Focus(label.Target);
            }
        }

        private void SliderCleanlinessValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Cleanliness = Convert.ToInt32(sliderCleanliness.Value);
        }

        private void SliderStaffValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Staff = Convert.ToInt32(sliderStaff.Value);
        }

        private void SliderComfortValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Comfort = Convert.ToInt32(sliderComfort.Value);
        }
        private void SliderValueForMoneyValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ValueForMoney = Convert.ToInt32(sliderValueForMoney.Value);
        }

        private void SliderRenovationRecommendationLevelValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RecommendationLevel = "Level " + Convert.ToString(sliderRenovationRecommendationLevel.Value);
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
            NavigationService?.Navigate(new Guest1Forum(Guest1, this));
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

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);

            LoginForm window = new LoginForm();
            window.Show();
            currentWindow.Close();
        }


    }
}
