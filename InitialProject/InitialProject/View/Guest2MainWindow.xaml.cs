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

        public Page Page { get; set; }

        public Guest2MainWindow(string username)
        {
            InitializeComponent();
            Username = username;

            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            mainFrame.Content = guest2PageTours;

        }


        private void ToursButtonClick(object sender, RoutedEventArgs e)
        {
            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            mainFrame.Content = guest2PageTours;
        }

        private void ProfilePage(object sender, RoutedEventArgs e)
        {
            Guest2ProfilePreview profilePreview = new Guest2ProfilePreview(Username);
            mainFrame.Content = profilePreview;

        }

        private void TourRequestCreate(object sender, RoutedEventArgs e)
        {
            
            Guest2TourRequestCreation guest2TourRequestCreation = new Guest2TourRequestCreation(Username);
            mainFrame.Content = guest2TourRequestCreation;
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

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Page currentPage = mainFrame.Content as Page;
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
            mainFrame.Content = currentPage;

        }

        private void TourAttendance(object sender, RoutedEventArgs e)
        {
            Guest2TourAttendance guest2TourAttendance = new Guest2TourAttendance(Username);
            mainFrame.Content = guest2TourAttendance;



        }
    }
}
