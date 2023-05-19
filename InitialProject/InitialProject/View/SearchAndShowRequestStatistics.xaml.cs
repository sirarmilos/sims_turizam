using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for SearchAndShowRequestStatistics.xaml
    /// </summary>
    public partial class SearchAndShowRequestStatistics : Window
    {
        private readonly TourRequestService tourRequestService;
        public string GuideUsername
        {
            get;
            set;
        }

        private string country;
        private string city;
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

        public string Jezik
        {
            get { return jezik; }
            set
            {
                jezik = value;
                OnPropertyChanged(nameof(Jezik));
            }
        }

        /*private List<int> godine;
        public List<int> Godine
        {
            get { return godine; }
            set
            {
                godine = value;
            }
        }

        private List<int> brstanje;
        public List<int> Brstanje
        {
            get { return brstanje; }
            set
            {
                brstanje = value;
            }
        }*/
        
        public SearchAndShowRequestStatistics(string username)
        {
            InitializeComponent();
            DataContext = this;
            GuideUsername = username;
            tourRequestService = new TourRequestService();
            //Godine = new List<int>();
            //Brstanje = new List<int>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Results(object sender, RoutedEventArgs e)
        {
            List<TourRequest> tourRequests = tourRequestService.FindAll(Country, City, Jezik);

            List<(int Year, int Count)> groupedData = new List<(int Year, int Count)>();

            if (tourRequests == null)
            {
                tbResult.Text = "0";
                dgYears.ItemsSource = null;
                dgCount.ItemsSource = null;
            }
            else
            {
                tbResult.Text = tourRequests.Count.ToString();
                groupedData = tourRequestService.FindCountTourRequestsForYears(tourRequests).ToList();
                dgYears.ItemsSource = groupedData.Select(pair => pair.Year).ToList();
                dgCount.ItemsSource = groupedData.Select(pair => pair.Count).ToList();
            }

           // List<(int Year, int Count)> groupedData = tourRequestService.FindCountTourRequestsForYears(tourRequests).ToList();
            //dgYears.ItemsSource = groupedData;
            /*List<int> years = groupedData.Select(pair => pair.Year).ToList();
            List<int> count = groupedData.Select(pair => pair.Count).ToList();
            Years = years;
            Count = count;*/
            //dgYears.ItemsSource = groupedData.Select(pair => pair.Year).ToList();
            /*Godine = godine;
            dgYears.ItemsSource = Godine;*/
            //dgCount.ItemsSource = groupedData.Select(pair => pair.Count).ToList();



        }


        private void dgYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<TourRequest> tourRequests = tourRequestService.FindAll(Country, City, Jezik);

            //List<(int Month, int Count)> groupedData = tourRequestService.FindCountTourRequestForMonthsForSpecificYear(tourRequests, (int)dgYears.SelectedItem).ToList();
            List<(int Month, int Count)> groupedData = new List<(int Month, int Count)>();
            List<string> months = new List<string>();

            if(tourRequests == null)
            {
                dgMonths.ItemsSource = null;
                dgMonthsCount.ItemsSource = null;
            }
            else {

                groupedData = tourRequestService.FindCountTourRequestForMonthsForSpecificYear(tourRequests, (int)dgYears.SelectedItem).ToList();

                foreach (var a in groupedData.Select(pair => pair.Month).ToList())
                {
                    months.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(a));
                }

                dgMonths.ItemsSource = /*groupedData.Select(pair => pair.Month).ToList();*/ months;
                dgMonthsCount.ItemsSource = groupedData.Select(pair => pair.Count).ToList();
            }
            
            /*foreach(var a in groupedData.Select(pair => pair.Month).ToList())
            {
                months.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(a));
            }

            dgMonths.ItemsSource = groupedData.Select(pair => pair.Month).ToList(); months;
            dgMonthsCount.ItemsSource = groupedData.Select(pair => pair.Count).ToList();*/

        }


    }
}
