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

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }

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

        private DateTime filterStartDate;
        public DateTime FilterStartDate
        {
            get { return filterStartDate; }
            set
            {
                filterStartDate = value;
            }
        }

        private DateTime filterEndDate;
        public DateTime FilterEndDate
        {
            get { return filterEndDate; }
            set
            {
                filterEndDate = value;
            }
        }

        private DateTime selectedStartDate;
        public DateTime SelectedStartDate
        {
            get { return selectedStartDate; }
            set
            {
                selectedStartDate = value;
            }
        }

        public TourRequest TourRequest { get; set; }

        public SearchAndShowTourRequests(string guideUsername)
        {
            InitializeComponent();
            DataContext = this;
            Guide = guideUsername;
            tourRequestService = new TourRequestService();
            //Jezik = null;
            FilterStartDate = DateTime.Today;
            FilterEndDate = DateTime.Today;
            label1.Visibility = Visibility.Hidden;
            button1.Visibility = Visibility.Hidden;
            dpStartDate.Visibility = Visibility.Hidden;
            SelectedStartDate = DateTime.Today;
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
                new SearchAndShowTourRequestDTO(Country, City, GuestNumber, Jezik, FilterStartDate, FilterEndDate) ;
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

        private void ProceedTourRequest(object sender, RoutedEventArgs e)
        {
            /*TourRequest selectedItem = (TourRequest)ListTourRequests.SelectedItem;
            tourRequestService.UpdateStatusToAccepted(selectedItem);
            MessageBox.Show("Successfully accepted!");*/
           
            TourRequest selectedItem = (TourRequest)ListTourRequests.SelectedItem;
            if(selectedItem != null)
            {
                label1.Visibility = Visibility.Visible;
                button1.Visibility = Visibility.Visible;
                dpStartDate.Visibility = Visibility.Visible;
                TourRequest = selectedItem;
                //ConfirmTourRequest(selectedItem);
                /*ChooseDateForTourRequest window = new ChooseDateForTourRequest("Guide1", selectedItem);
                window.ShowDialog();*/
            }
            else
            {
                MessageBox.Show("Select one row from data grid!");
            }
                
       
            
               
            
            //ListTourRequests.Items.Refresh();


        }

        private void ConfirmTourRequest(object sender, RoutedEventArgs e)
        {
            if (tourRequestService.AcceptTourRequest(Guide, TourRequest, SelectedStartDate))
            {
                MessageBox.Show("Succesfull");
                tourRequestService.UpdateStatusToAccepted(TourRequest);
                GuideCreateNewTour window = new GuideCreateNewTour(Guide, TourRequest, SelectedStartDate);
                window.Show();
            }
            else
            {
                MessageBox.Show("Error");
            }
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

        private void GoToAllTourOccurences(object sender, RoutedEventArgs e)
        {
            AllTourOccurences window = new AllTourOccurences();
            window.Show();
            Close();
        }

        private void GoToHomePage(object sender, RoutedEventArgs e)
        {
            TourGuidenceService tourGuidenceService = new TourGuidenceService();

            TourGuidence tg = tourGuidenceService.CheckIfStartedAndNotFinished();
            if (tg != null)
            {
                GuideStart2 window = new GuideStart2(Guide, tg);
                window.Show();
                Close();
            }
            else
            {
                GuideStart1 window = new GuideStart1(Guide);
                window.Show();
                Close();
            }
        }

        private void GoToAddNewTour(object sender, RoutedEventArgs e)
        {
            GuideCreateNewTour window = new GuideCreateNewTour(Guide);
            window.Show();
            Close();
        }

        private void OfferTour(object sender, RoutedEventArgs e)
        {
            CreateTourForRequestStatistics window = new CreateTourForRequestStatistics(Guide);
            window.ShowDialog();
        }
    }
}
