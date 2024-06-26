﻿using InitialProject.Model;
using InitialProject.Service;
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
    /// Interaction logic for ShowMostPopularTour.xaml
    /// </summary>
    public partial class ShowMostPopularTour : Window
    {
        private readonly TourGuidenceService tourGuidenceService;

        public List<string> Years
        {
            get;
            set;
        }

        public string SelectedYear
        {
            get;
            set;
        }

        public ShowMostPopularTour()
        {
            InitializeComponent();
            DataContext = this;
            tourGuidenceService = new();
            Years = new List<string>();
            Years.Add("ALL TIME");
            Years.Add("2021");
            Years.Add("2022");
            Years.Add("2023");
            SelectedYear = null;
            MostPopularTour.ItemsSource = null;
            MostPopularTour.Visibility = Visibility.Hidden;
            Label1.Visibility = Visibility.Hidden;

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
            Close();
        }

        private void FindMostPopularTour(object sender, RoutedEventArgs e)
        {
            if (SelectedYear == null)
            {
                MostPopularTour.ItemsSource = null;
                return;
            }
            else
                if (SelectedYear.Equals("ALL TIME"))
            {
                MostPopularTour.Visibility = Visibility.Visible;
                Label1.Visibility = Visibility.Hidden;
                Tour tour = tourGuidenceService.FindMostVisitedAllTime();
                ObservableCollection<Tour> tours = new ObservableCollection<Tour>();
                tours.Add(tour);
                MostPopularTour.ItemsSource = tours;
            }
            else
                if (SelectedYear != null)
            {
                Tour tour = tourGuidenceService.FindMostVisitedByYear(int.Parse(SelectedYear));
                if (tour == null)
                {
                    MostPopularTour.ItemsSource = null;
                    MostPopularTour.Visibility = Visibility.Hidden;
                    Label1.Visibility = Visibility.Visible;
                    return;
                }
                MostPopularTour.Visibility = Visibility.Visible;
                Label1.Visibility = Visibility.Hidden;
                ObservableCollection<Tour> tours = new ObservableCollection<Tour>();
                tours.Add(tour);
                MostPopularTour.ItemsSource = tours;
            }
        }

        private void GoToHomePage(object sender, RoutedEventArgs e)
        {
            TourGuidence tg = tourGuidenceService.CheckIfStartedAndNotFinished();
            if (tg != null)
            {
                GuideStart2 window = new GuideStart2("Guide1", tg);
                window.Show();
                Close();
            }
            else
            {
                GuideStart1 window = new GuideStart1("Guide1");
                window.Show();
                Close();
            }
        }

        private void GoToAllTourOccurences(object sender, RoutedEventArgs e)
        {
            AllTourOccurences window = new AllTourOccurences();
            window.Show();
            Close();
        }
    }
}
