using InitialProject.Dto;
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

namespace InitialProject.View
{

    public partial class Guest2MainWindow : Window
    {
        private string Username;

        public static int ThemeType = 0; // Svetla tema 0, tamna 1
         
        public static int LanguageId = 0; // engleski 0, srpski 1

        public static Button currentlySelected = new Button();

        public Page Page { get; set; }

        public Guest2MainWindow(string username)
        {
            InitializeComponent();
            Username = username;

        

            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            mainFrame.Content = guest2PageTours;

            PaintButtons();
            ToursButton.Background = Brushes.LightGray;
            ToursButton.Foreground = Brushes.Black;

            currentlySelected = ToursButton;

        }


        private void ToursButtonClick(object sender, RoutedEventArgs e)
        {
            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            mainFrame.Content = guest2PageTours;
            ToursButton.Background = Brushes.LightGray;


            PaintButtons();
            ToursButton.Background = Brushes.LightGray;
            ToursButton.Foreground = Brushes.Black;

            currentlySelected = ToursButton;
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
            
            Guest2TourRequestCreation guest2TourRequestCreation = new Guest2TourRequestCreation(Username);
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


            object currentPage = mainFrame.Content;

            // Check if the page is of a specific type
            if (currentPage is Guest2PageTours)
            {
                // The displayed page is of type YourPageType
                Guest2PageTours pagee = (Guest2PageTours)currentPage;
                // Perform actions specific to YourPageType
                mainFrame.Content = pagee;
            }

        }

        private void TourAttendance(object sender, RoutedEventArgs e)
        {
            Guest2TourAttendance guest2TourAttendance = new Guest2TourAttendance(Username);
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
    }
}
