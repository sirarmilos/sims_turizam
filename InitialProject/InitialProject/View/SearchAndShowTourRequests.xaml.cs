using InitialProject.Dto;
using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
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
    /// Interaction logic for SearchAndShowTourRequests.xaml
    /// </summary>
    public partial class SearchAndShowTourRequests : Window
    {
        private TourRequestService tourRequestService;
        private string country;
        private string city;
        private int? guestNumber;
        private string jezik;

        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged(nameof(Country));

            }
        }

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public int? GuestNumber
        {
            get { return guestNumber; }
            set
            {
                guestNumber = value;
                OnPropertyChanged();
            }
        }

        public string Jezik
        {
            get { return jezik; }
            set
            {
                jezik = value;
                OnPropertyChanged(nameof(Jezik));
            }
        }

        public SearchAndShowTourRequests()
        {
            InitializeComponent();
            DataContext = this;
            tourRequestService = new TourRequestService();
            //Jezik = null;
        }


        private void Search(object sender, RoutedEventArgs e)
        {
            if ((GuestNumber != null) && (GuestNumber == 0))
            {
                MessageBox.Show("You can't use zero as number of guests.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                ListTourRequests.ItemsSource = null;
                return;
            }
            SearchAndShowTourRequestDTO searchShowTourRequestDTO =
                new SearchAndShowTourRequestDTO(Country, City, GuestNumber, Jezik);
            List<TourRequest> tourRequests = tourRequestService.FindAll(searchShowTourRequestDTO);
            if(tourRequests == null)
            {
                ListTourRequests.ItemsSource = null;
            }
            else
            {
                ListTourRequests.ItemsSource = tourRequests.FindAll(x => x.Status.Equals("pending"));
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AllowOnlyDigits(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void AllowOnlyCharacters(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, 0) && !char.IsWhiteSpace(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void AcceptTourRequest(object sender, RoutedEventArgs e)
        {
            TourRequest selectedItem = (TourRequest)ListTourRequests.SelectedItem;
            tourRequestService.UpdateStatusToAccepted(selectedItem);
            MessageBox.Show("Successfully accepted!");
        }
    }
}
