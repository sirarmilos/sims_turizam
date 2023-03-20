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
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    /// 

    public partial class TourReservation : Window
    {
        public static ObservableCollection<Tour> Tours { get; set; }

        private readonly TourRepository tourRepository = new TourRepository();

        private string TourId { get; set; }

        public TourReservation()
        {
            InitializeComponent();
        }

        public TourReservation(string tourId)
        {
            InitializeComponent();
            TourId = tourId;
            Tours  = new ObservableCollection<Tour>(tourRepository.GetById(TourId));
            listTours.ItemsSource = Tours;
        }
    }
}
