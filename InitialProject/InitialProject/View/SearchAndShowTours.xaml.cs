﻿using InitialProject.Model;
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
    /// Interaction logic for SearchAndShowTours.xaml
    /// </summary>
    public partial class SearchAndShowTours : Window
    {
        public static ObservableCollection<Tour> tours { get; set; }

        private readonly TourRepository repository;
        public SearchAndShowTours()
        {
            InitializeComponent();
            Initializecblang();
            DataContext = this;
            repository = new TourRepository();
            tours = new ObservableCollection<Tour>(repository.Load());
        }

        private void Initializecblang()
        {
            foreach(string lang in (Enum.GetNames(typeof(Languages))))
            {
                cblang.Items.Add(lang);
            }
            cblang.SelectedIndex = 0;
        }

        private void GetToursByParameters(object sender, RoutedEventArgs e)
        {
            tours = new ObservableCollection<Tour>(repository.SearchAndShow(tb1.Text,tb2.Text,0,0,12));
        }
    }
}
