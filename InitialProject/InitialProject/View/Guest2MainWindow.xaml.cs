using InitialProject.Dto;
using InitialProject.Service;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InitialProject;
using ControlzEx.Theming;

namespace InitialProject.View
{
    public partial class Guest2MainWindow : Window
    {
        private string Username;

        public static int ThemeType = 0; // Svetla tema 0, tamna 1
         
        public static int LanguageId = 0; // engleski 0, srpski 1

        public static Button currentlySelected = new Button();

        public event EventHandler ButtonClicked;

        public event EventHandler ButtonThemeClicked;

        public event EventHandler ToursButtonClicked;

        private string currentLanguage;

        public string CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                currentLanguage = value;
            }
        }

        public event EventHandler<EventArgs> PageNavigated;

        protected virtual void OnPageNavigated()
        {
            PageNavigated?.Invoke(this, EventArgs.Empty);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            OnPageNavigated();
        }

        private void Frame_PageNavigated(object sender, EventArgs e)
        {
            if(mainFrame.Content is Guest2PageTours)
            {
                PaintButtons();
                ToursButton.Background = Brushes.LightGray;
                ToursButton.Foreground = Brushes.Black;

                currentlySelected = ToursButton;
            }

            if(mainFrame.Content is Guest2NotificationPage)
            {
                PaintButtons();
                NotificationsButton.Background = Brushes.LightGray;
                NotificationsButton.Foreground = Brushes.Black;
            }
        }

        

        private void MainFrameContentRendered(object sender, EventArgs e)
        {
            if (mainFrame.Content is Guest2PageTours)
            {
                PaintButtons();
                ToursButton.Background = Brushes.LightGray;
                ToursButton.Foreground = Brushes.Black;

                currentlySelected = ToursButton;
            }
            else if (mainFrame.Content is Guest2NotificationPage)
            {
                PaintButtons();
                NotificationsButton.Background = Brushes.LightGray;
                NotificationsButton.Foreground = Brushes.Black;
            }

            else if (mainFrame.Content is Guest2ProfilePreview)
            {
                PaintButtons();
                ProfileButton.Background = Brushes.LightGray;
                ProfileButton.Foreground = Brushes.Black;

                currentlySelected = ProfileButton;
            }
            else if (mainFrame.Content is Guest2TourRequestCreation)
            {
                PaintButtons();
                TourRequestCreationButton.Background = Brushes.LightGray;
                TourRequestCreationButton.Foreground = Brushes.Black;

                currentlySelected = TourRequestCreationButton;
            }
            else if (mainFrame.Content is Guest2TourAttendance)
            {
                PaintButtons();
                TourAttendanceButton.Background = Brushes.LightGray;
                TourAttendanceButton.Foreground = Brushes.Black;

                currentlySelected = TourAttendanceButton;
            }
            else if(mainFrame.Content is Guest2ComplexTourDisplay)
            {
                PaintButtons();
                ComplexTourDisplayButton.Background = Brushes.LightGray;    
                ComplexTourDisplayButton.Foreground = Brushes.Black;
                currentlySelected = ComplexTourDisplayButton;
            }

            else if(mainFrame.Content is Guest2TourRequestStatistics)
            {
                PaintButtons();
                TourRequestStatisticsButton.Background = Brushes.LightGray;
                TourRequestStatisticsButton.Foreground = Brushes.Black;
                currentlySelected = TourRequestStatisticsButton;
            }

            else if(mainFrame.Content is Guest2ComplexTourRequestCreation)
            {
                PaintButtons();
                ComplexTourRequestButton.Background = Brushes.LightGray;
                ComplexTourRequestButton.Foreground = Brushes.Black;
                currentlySelected = ComplexTourRequestButton;
            }

            else if(mainFrame.Content is Guest2DisplayRequestedTours)
            {
                PaintButtons();
                TourRequestDisplayButton.Background = Brushes.LightGray;
                TourRequestDisplayButton.Foreground = Brushes.Black;
                currentlySelected = TourRequestDisplayButton;
            }
        }

        public Page Page { get; set; }

        public Guest2MainWindow(string username)
        {
            InitializeComponent();
            Username = username;

            mainFrame.ContentRendered += MainFrameContentRendered;
            CurrentLanguage = "en-US";

            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            mainFrame.Content = guest2PageTours;

            PaintButtons();
            ToursButton.Background = Brushes.LightGray;
            ToursButton.Foreground = Brushes.Black;

            currentlySelected = ToursButton;

            VoucherService voucherService = new VoucherService();
            voucherService.AddNewVoucher(username);

        }


        public void ToursButtonClick(object sender, EventArgs e)
        {
            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            mainFrame.Content = guest2PageTours;

            ToursButton.Background = Brushes.LightGray;


            PaintButtons();
            ToursButton.Background = Brushes.LightGray;
            ToursButton.Foreground = Brushes.Black;

            currentlySelected = ToursButton;

            ToursButtonClicked?.Invoke(this, EventArgs.Empty);
        }


        private void ProfilePage(object sender, RoutedEventArgs e)
        {
            Guest2ProfilePreview profilePreview = new Guest2ProfilePreview(Username);
            mainFrame.Content = profilePreview;


            PaintButtons();
            ProfileButton.Background = Brushes.LightGray;
            ProfileButton.Foreground = Brushes.Black;

            currentlySelected = ProfileButton;

        }

        private void TourRequestCreate(object sender, RoutedEventArgs e)
        {
            
            Guest2TourRequestCreation guest2TourRequestCreation = new Guest2TourRequestCreation();
            mainFrame.Content = guest2TourRequestCreation;


            PaintButtons();
            TourRequestCreationButton.Background = Brushes.LightGray;
            TourRequestCreationButton.Foreground = Brushes.Black;

            currentlySelected = TourRequestCreationButton;
        }

        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
             if (ThemeType == 0)
             {
                 if (LanguageId == 0)
                 {
                     AppTheme.ChangeTheme(new Uri("/Themes/DarkTheme.xaml", UriKind.Relative), new Uri("/Languages/StringEn.xaml", UriKind.Relative));
                     ThemeType = 1;
                 }
                 else
                 {
                     AppTheme.ChangeTheme(new Uri("/Themes/DarkTheme.xaml", UriKind.Relative), new Uri("/Languages/StringSr.xaml", UriKind.Relative));
                     ThemeType = 1;
                 }
             }
             else
             {
                 if (LanguageId == 0)
                 {
                     AppTheme.ChangeTheme(new Uri("/Themes/LightTheme.xaml", UriKind.Relative), new Uri("/Languages/StringEn.xaml", UriKind.Relative));
                     ThemeType = 0;
                 }
                 else
                 {
                     AppTheme.ChangeTheme(new Uri("/Themes/LightTheme.xaml", UriKind.Relative), new Uri("/Languages/StringSr.xaml", UriKind.Relative));
                     ThemeType = 0;
                 }
             }

             DataContext = this;
            PaintButtons();
            currentlySelected.Background = Brushes.LightGray;
            currentlySelected.Foreground = Brushes.Black;

            ButtonClicked?.Invoke(this, EventArgs.Empty);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (LanguageId == 0)
            {
                if (ThemeType == 0)
                {
                    AppTheme.ChangeTheme(new Uri("/Themes/LightTheme.xaml", UriKind.Relative), new Uri("/Languages/StringSr.xaml", UriKind.Relative));
                    LanguageId = 1;
                }
                else
                {
                    AppTheme.ChangeTheme(new Uri("/Themes/DarkTheme.xaml", UriKind.Relative), new Uri("/Languages/StringSr.xaml", UriKind.Relative));
                    LanguageId = 1;
                }
            }
            else
            {
                if (ThemeType == 0)
                {
                    AppTheme.ChangeTheme(new Uri("/Themes/LightTheme.xaml", UriKind.Relative), new Uri("/Languages/StringEn.xaml", UriKind.Relative));
                    LanguageId = 0;
                }
                else
                {
                    AppTheme.ChangeTheme(new Uri("/Themes/DarkTheme.xaml", UriKind.Relative), new Uri("/Languages/StringEn.xaml", UriKind.Relative));
                    LanguageId = 0;
                }
            }


            DataContext = this;
            PaintButtons();
            currentlySelected.Background = Brushes.LightGray;
            currentlySelected.Foreground = Brushes.Black;

            ButtonClicked?.Invoke(this, EventArgs.Empty);

        }

        private void TourAttendance(object sender, RoutedEventArgs e)
        {
            Guest2TourAttendance guest2TourAttendance = new Guest2TourAttendance();
            mainFrame.Content = guest2TourAttendance;


            PaintButtons();
            TourAttendanceButton.Background = Brushes.LightGray;
            TourAttendanceButton.Foreground = Brushes.Black;

            currentlySelected = TourAttendanceButton;
        }


        public void PaintButtons()
        {
            Brush buttonColor;
            Brush textColor;
            if(ThemeType==0)
            {
                buttonColor = (Brush)new BrushConverter().ConvertFrom("#4CAF50");
                textColor = (Brush)new BrushConverter().ConvertFrom("#333333");
            }
            else
            {
                buttonColor = (Brush)new BrushConverter().ConvertFrom("#2196F3");
                textColor = (Brush)new BrushConverter().ConvertFrom("#EFEFEF");
            }

            NotificationsButton.Background = buttonColor;
            NotificationsButton.Foreground = textColor;

            ProfileButton.Background = buttonColor;
            ProfileButton.Foreground = textColor;

            ToursButton.Background = buttonColor;
            ToursButton.Foreground = textColor;

            TourAttendanceButton.Background = buttonColor;
            TourAttendanceButton.Foreground = textColor;

            TourRequestCreationButton.Background = buttonColor;
            TourRequestCreationButton.Foreground = textColor;

            TourRequestDisplayButton.Background = buttonColor;
            TourRequestDisplayButton.Foreground = textColor;

            TourRequestStatisticsButton.Background = buttonColor;
            TourRequestStatisticsButton.Foreground = textColor;

            ComplexTourRequestButton.Background = buttonColor;
            ComplexTourRequestButton.Foreground = textColor;

            ComplexTourDisplayButton.Background = buttonColor;
            ComplexTourDisplayButton.Foreground = textColor;


        }

        private void LogoClick(object sender, RoutedEventArgs e)
        {
            InitializeComponent();

            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            mainFrame.Content = guest2PageTours;

            PaintButtons();
            ToursButton.Background = Brushes.LightGray;
            ToursButton.Foreground = Brushes.Black;

            currentlySelected = ToursButton;
        }

        private void TourRequestDisplayButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2DisplayRequestedTours guest2DisplayRequestedTours = new Guest2DisplayRequestedTours(Username);
            mainFrame.Content = guest2DisplayRequestedTours;

            PaintButtons();
            TourRequestDisplayButton.Background = Brushes.LightGray;
            TourRequestDisplayButton.Foreground = Brushes.Black;

            currentlySelected = TourRequestDisplayButton;
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2NotificationPage guest2NotificationPage = new Guest2NotificationPage(Username);
            mainFrame.Content = guest2NotificationPage; 

            PaintButtons();
            NotificationsButton.Background = Brushes.LightGray;
            NotificationsButton.Foreground = Brushes.Black;

            currentlySelected = NotificationsButton;
        }

        private void TourRequestStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2TourRequestStatistics guest2TourRequestStatistics = new Guest2TourRequestStatistics(Username);
            mainFrame.Content = guest2TourRequestStatistics;

            PaintButtons();
            TourRequestStatisticsButton.Background = Brushes.LightGray;
            TourRequestStatisticsButton.Foreground = Brushes.Black;

            currentlySelected = TourRequestStatisticsButton;
        }

        private void ComplexTourRequestButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2ComplexTourRequestCreation guest2ComplexTourRequestCreation = new Guest2ComplexTourRequestCreation(Username);
            mainFrame.Content= guest2ComplexTourRequestCreation;

            PaintButtons();

            ComplexTourRequestButton.Background = Brushes.LightGray;
            ComplexTourRequestButton.Foreground = Brushes.Black;

            currentlySelected = ComplexTourRequestButton;
        }

        private void ComplexTourDisplayButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2ComplexTourDisplay guest2ComplexTourDisplay = new Guest2ComplexTourDisplay(Username);
            mainFrame.Content = guest2ComplexTourDisplay;

            PaintButtons();

            ComplexTourDisplayButton.Background = Brushes.LightGray;
            ComplexTourDisplayButton.Foreground = Brushes.Black;

            currentlySelected = ComplexTourDisplayButton;
        }
    }
}
