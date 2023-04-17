﻿using InitialProject.Model;
using InitialProject.Repository;
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

        private readonly TourReservationRepository tourReservationRepository;

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
            tourReservationRepository = new TourReservationRepository();
            tourReservations = new List<Dto.ReservationDisplayDto> (tourReservationRepository.GetAllForOneTourGuidence(guidenceId));
            GuidenceId = guidenceId;
        }

        private void Save(object sender, RoutedEventArgs e)
        {

            if(tourReservationRepository.UpdateKeyPointArrivals(GuidenceId, Guest, Convert.ToInt32(KeyPointId)) == 1)
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
