using InitialProject.Model;
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
    /// Interaction logic for SearchAndShowTours.xaml
    /// </summary>
    public partial class SearchAndShowTours : Window
    {
        public static ObservableCollection<Tour> tours { get; set; }

        private readonly TourRepository tourRepository;


        public SearchAndShowTours()
        {
            InitializeComponent();
            Initializecblang();
            DataContext = this;
            tourRepository = new TourRepository();
            tours = new ObservableCollection<Tour>(tourRepository.Load());
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            tours = new ObservableCollection<Tour>(tourRepository.SearchAndShow(tb1.Text,tb2.Text,0,0,12));
        }
    }
}
