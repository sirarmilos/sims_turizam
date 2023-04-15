using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Interaction logic for GuideStart.xaml
    /// </summary>
    public partial class GuideStart : Window
    {
        public static Tour tour { get; set; }

        public static Tour tourFiltered { get; set; }

        private readonly TourGuidenceRepository tourGuidenceRepository;

        public GuideStart()
        {
            InitializeComponent();
            DataContext = this;
            tourGuidenceRepository = new TourGuidenceRepository();
            tour = tourGuidenceRepository.GetMostVisitedAllTime();
            int year = 2022;
            tourFiltered = tourGuidenceRepository.GetMostVisitedByYear(year);
        }

        private void GoToAddNewTour(object sender, RoutedEventArgs e)
        {
            AddNewTour window = new AddNewTour();
            window.Show();
        }

        private void GoToShowTourGuidences(object sender, RoutedEventArgs e)
        {
            ShowTourGuidences window = new ShowTourGuidences();
            window.Show();
        }

        private void GoToFutureTours(object sender, RoutedEventArgs e)
        {
            FutureTours window = new FutureTours();
            window.Show();
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }
    }
}
