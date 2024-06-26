﻿using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ShowGuestOnTourGuidence.xaml
    /// </summary>
    public partial class ShowGuestOnTourGuidence : Window
    {
        public static List<Dto.ReservationDisplayDto> tourReservations { get; set; }

        private readonly TourReservationService tourReservationService;

        private readonly TourGuidenceService tourGuidenceService;

        private string guest;

        public string Guest
        {
            get { return guest; }
            set
            {
                guest = value;
            }
        }

        private string keyPointId;

        public string KeyPointId
        {
            get { return keyPointId; }
            set
            {
                keyPointId = value;
            }
        }

        public int GuidenceId { get; set; }

        public ShowGuestOnTourGuidence(int guidenceId)
        {
            InitializeComponent();
            DataContext = this;
            tourReservationService = new TourReservationService();
            tourGuidenceService = new TourGuidenceService();
            tourReservations = new List<Dto.ReservationDisplayDto> (tourReservationService.GetAllForOneTourGuidence(guidenceId));
            GuidenceId = guidenceId;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (!tourGuidenceService.CheckIfTourGuidenceReachedTourKeyPoint(GuidenceId, Convert.ToInt32(KeyPointId)))
            {
                MessageBox.Show("Tour Guidence has not reached that key point or that key point does not exist!");
                return;
            }
            int br = tourReservationService.UpdateKeyPointArrivals(GuidenceId, Guest, Convert.ToInt32(KeyPointId));
            if (br == 1)
            {
                MessageBox.Show("Successfully marked!");
            }
            else
            {
                MessageBox.Show("Error");
            }
        }


    }
}
