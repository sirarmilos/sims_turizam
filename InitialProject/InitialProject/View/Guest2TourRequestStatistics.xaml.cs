using InitialProject.Model;
using InitialProject.Service;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2TourRequestStatistics.xaml
    /// </summary>
    public partial class Guest2TourRequestStatistics : Page
    {

        TourRequestService tourRequestService;

        private string Username;

        private string year;

        private string year1;

        private List<string> _xLabels;


        public string Year
        {
            get { return year; }
            set
            {
                year = value;

                OnPropertyChanged(nameof(Year));
                InitializePieChart(Year);
            }
        }

        public string Year1
        {
            get { return year1; }
            set
            {
                year1 = value;
                OnPropertyChanged(nameof(Year1));

                if(!string.IsNullOrEmpty(Username))
                {
                    UpdateTextBox(Username, Year1);
                }

            }
        }

        public List<string> XLabels
        {
            get { return _xLabels; }
            set
            {
                if (_xLabels != value)
                {
                    _xLabels = value;
                    OnPropertyChanged();
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guest2TourRequestStatistics(string username)
        {
            InitializeComponent();
            DataContext = this;

            tourRequestService = new TourRequestService();
            Username = username;

            InitializeComboBoxYears();
            InitializeComboBoxYears1();

            InitializePieChart("");

            SeriesCollection seriesCollection = new SeriesCollection();
            List<string> labels = new List<string>();
            List<int> values = new List<int>();

            foreach (Language language in Enum.GetValues(typeof(Language)))
            {
                int val = tourRequestService.GetCountOfLanguage(username,language);
                if(val>0)
                {
                    labels.Add(language.ToString());
                    values.Add(val);
                };
            }

            LineSeries lineSeries = new LineSeries
            {
                Values = new ChartValues<int>(values),
                DataLabels = true,
            };


            lineSeries.LabelPoint = point => $"{point.Y}";

            seriesCollection.Add(lineSeries);

            chartLanguague.Series = seriesCollection;
            chartLanguague.AxisX.Add(new Axis { Labels = labels });

            UpdateTextBox(Username,"");

        }


        public void InitializeComboBoxYears()
        {
            comboBoxYears.Items.Add("");
            foreach(string year in tourRequestService.GetYears(Username))
            {
                comboBoxYears.Items.Add(year);
            }

            comboBoxYears.SelectedIndex = 0;

        }

        public void InitializeComboBoxYears1()
        {
            comboBoxYears1.Items.Add("");
            foreach (string year in tourRequestService.GetYears(Username))
            {
                comboBoxYears1.Items.Add(year);
            }

            comboBoxYears1.SelectedIndex = 0;

        }

        public void InitializePieChart(string year)
        {
            pieChart.Series.Clear();

            pieChart.Series.Add(new PieSeries
            {
                Title = "Accepted tours",
                Values = new ChartValues<int> { tourRequestService.CountAcceptedForUser(Username,Year) },
                DataLabels = true
            });

            pieChart.Series.Add(new PieSeries
            {
                Title = "Invalid tours",
                Values = new ChartValues<int> { tourRequestService.CountInvalidForUser(Username, Year) },
                DataLabels = true
            });
        }

        public void UpdateTextBox(string username, string year)
        {
            tb1.Text = tourRequestService.GetAverageGuestsByYear(Username,year).ToString();
        }


    }
}
