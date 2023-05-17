﻿using InitialProject.Model;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for GuideStart1.xaml
    /// </summary>
    public partial class GuideStart1 : Window
    {
        private readonly TourGuidenceService tourGuidenceService;

        public TourGuidence tourGuidence { get; private set; }

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }

        public string WelcomeText
        {
            get; set;
        }

        public GuideStart1(string username)
        {
            InitializeComponent();
            DataContext = this;
            tourGuidenceService = new TourGuidenceService();
            Guide = username;
            WelcomeText = "WELCOME, " + Guide;
            dgStart1.ItemsSource = tourGuidenceService.FindAllForToday(Guide);
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

        private void StartTourGuidence(object sender, RoutedEventArgs e)
        {
            TourGuidenceService tourGuidenceService = new TourGuidenceService();
            /*if (tourGuidenceService.CheckGuidencesForStart(TourGuidences))
            {
                MessageBox.Show("You already started another tour!");
                this.Close();
                ShowTourGuidences window = new ShowTourGuidences(Guide);
                window.Show();
            }
            else*/
            {
                TourGuidence selectedItem = (TourGuidence)dgStart1.SelectedItem;
                tourGuidenceService.UpdateStartedField(selectedItem.Id);
                MessageBox.Show("Tour successfully started");
                //ShowKeyPointsInStartedTourGuidence window = new(TourGuidence, TourGuidences);
                GuideStart2 window = new GuideStart2(Guide, selectedItem);
                window.Show();
                this.Close();
                //window.Show();
                
            }

        }
    }
}