using InitialProject.Model;
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
    /// Interaction logic for AllTourOccurences.xaml
    /// </summary>
    public partial class AllTourOccurences : Window
    {
        private readonly TourGuidenceService tourGuidenceService;

        private List<TourGuidence> TourGuidences
        {
            get;
            set;
        }

        private DateTime selectedFromDate;
        public DateTime SelectedFromDate
        {
            get { return selectedFromDate; }
            set
            {
                selectedFromDate = value;
            }
        }

        private DateTime selectedToDate;
        public DateTime SelectedToDate
        {
            get { return selectedToDate; }
            set
            {
                selectedToDate = value;
            }
        }


        public AllTourOccurences()
        {
            InitializeComponent();
            DataContext = this;
            tourGuidenceService = new TourGuidenceService();
            TourGuidences = new List<TourGuidence>();
            TourGuidences = tourGuidenceService.FindAll();
            dataGrid.ItemsSource = TourGuidences;
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

        private void ApplyDateFilters(object sender, RoutedEventArgs e)
        {
            if(SelectedFromDate == null || SelectedToDate == null)
            {
                return;
            }
            TourGuidences =  tourGuidenceService.FindAllInsideDateRange(SelectedFromDate, SelectedToDate);
            dataGrid.ItemsSource = TourGuidences;
        }

        private void OptionButton(object sender, RoutedEventArgs e)
        {
            TourGuidence selectedItem = (TourGuidence)dataGrid.SelectedItem;
            if(selectedItem != null)
            {
                TourGuidenceInformation window = new TourGuidenceInformation(selectedItem);
                window.Show();
                Close();
            }
            
            
        }
    }
}
