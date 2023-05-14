using InitialProject.Model;
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
    /// <summary>
    /// Interaction logic for TourGuidenceInformation.xaml
    /// </summary>
    public partial class TourGuidenceInformation : Window
    {
        public TourGuidence TourGuidence { get; set; }

        public string TitleText
        {
            get;
            set;
        }

        public string LatAndLong
        {
            get;
            set;
        }

        public string LoremIpsum
        {
            get;
            set;
        }

        public TourGuidenceInformation(TourGuidence tourGuidence)
        {
            InitializeComponent();
            DataContext = this;
            TourGuidence = tourGuidence;
            TitleText = "Information About " + TourGuidence.Tour.TourName;
            LatAndLong = TourGuidence.Tour.Location.Latitude + " and " + TourGuidence.Tour.Location.Longitude;
            LoremIpsum = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. " +
                "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an" +
                " unknown printer took a galley of type and scrambled it to make a type specimen book. " +
                "It has survived not only five centuries, but also the leap into electronic typesetting, " +
                "remaining essentially unchanged. It was popularised in the 1960s with the release of" +
                " Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing" +
                " software like Aldus PageMaker including versions of Lorem Ipsum.";

            CheckButtonAvailable();


        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

        private void GoToMostPopularTour(object sender, RoutedEventArgs e)
        {
            ShowMostPopularTour window = new ShowMostPopularTour();
            window.Show();
        }

        private void CheckButtonAvailable()
        {
            if(TourGuidence.Started == true)
            {
                CancelButton.IsEnabled = false;
            }

            if(TourGuidence.Finished == false)
            {
                StatsButton.IsEnabled = false;
            }
        }

        private void HyperlinkKeyPoints(object sender, RoutedEventArgs e)
        {
            ShowKeyPoints window = new ShowKeyPoints(TourGuidence);
            window.Show();
        }

        private void HyperlinkImages(object sender, RoutedEventArgs e)
        {
            TourImages window = new TourImages();
            window.Show();
        }
    }
}
