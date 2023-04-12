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
    /// Interaction logic for ShowTourGuidences.xaml
    /// </summary>
    public partial class ShowTourGuidences : Window
    {
        public static ObservableCollection<TourGuidence> tourGuidences { get; set; }

        private readonly TourGuidenceRepository tourGuidenceRepository;



        public ShowTourGuidences()
        {
            InitializeComponent();
            DataContext = this;
            tourGuidenceRepository = new TourGuidenceRepository();
            tourGuidences = new ObservableCollection<TourGuidence>(tourGuidenceRepository.GetAllForToday());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DetailsTourGuidence(object sender, RoutedEventArgs e)
        {
            TourGuidence selectedGuidence = (TourGuidence)dataGrid.SelectedItem;

            ShowKeyPointsInStartedTourGuidence window = new ShowKeyPointsInStartedTourGuidence(selectedGuidence, tourGuidences.ToList());
            window.Show();
            this.Close();
        }
    }
}
