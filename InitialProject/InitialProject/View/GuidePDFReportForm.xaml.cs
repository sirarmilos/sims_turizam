using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Service;
using LiveCharts.Wpf;
using LiveCharts;
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
using System.Reflection.Emit;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Repository;
using System.Diagnostics;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for GuidePDFReportForm.xaml
    /// </summary>
    public partial class GuidePDFReportForm : Window
    {
        //private readonly ReviewService reviewService;

        private UserService userService;

        private TourService tourService;

        private TourGuidenceService tourGuidenceService;

        private VoucherService voucherService;

        private double voucherPercentage;

        private List<string> _xLabels;

        public string GuideUsername
        {
            get;
            set;
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

        private List<string> _xvoucherLabels;

        public List<string> XvoucherLabels
        {
            get { return _xvoucherLabels; }
            set
            {
                if (_xvoucherLabels != value)
                {
                    _xvoucherLabels = value;
                    OnPropertyChanged();
                }

            }
        }


        private Func<double, string> _axisYLabelFormatter;

        public Func<double, string> AxisYLabelFormatter
        {
            get { return _axisYLabelFormatter; }
            set
            {
                _axisYLabelFormatter = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GuidePDFReportForm(string guideUsername)
        {
            InitializeComponent();
            DataContext = this;

            GuideUsername = guideUsername;



            //reviewService = new ReviewService(OwnerUsername);

            SetDefaultValue();
        }

        private void SetDefaultValue()
        {
            //OwnerPDFReportDTOs = new List<OwnerPDFReportDTO>();
            //OwnerPDFReportDTOs = reviewService.FindOwnerPDFReportDTOs(OwnerUsername);
            //lvShowGuestReviews.ItemsSource = OwnerPDFReportDTOs;
            userService = new UserService();
            tourService = new TourService();
            tourGuidenceService = new TourGuidenceService();
            voucherService = new VoucherService();
            //InitializePieChart();
            

            labelGuideUsername.Content = GuideUsername;
            labelGuideType.Content = CheckSuperType();
            labelNumberOfTours.Content = tourService.FindAll().Count;
            labelReportDate.Content = DateTime.Now.ToString("dd/MM/yyyy");
            labelReportTime.Content = DateTime.Now.ToString("HH:mm:ss");

            SeriesCollection seriesCollection = new SeriesCollection();
            List<string> labels = new List<string>();
            labels.Add("Under 18");
            labels.Add("18-50");
            labels.Add("Above 50");
            AxisYLabelFormatter = value => value.ToString("0.0");

            List<int> values = new List<int>();
            values = tourService.FindGuestNumber(1, GuideUsername);


            XLabels = labels;

            ColumnSeries lineSeries = new ColumnSeries
            {

                Values = new ChartValues<int>(values),
                DataLabels = true,
            };


            lineSeries.LabelPoint = point => $"{point.Y}";
            seriesCollection.Add(lineSeries);
            chartAgeCount.Series = seriesCollection;


            //voucher chart

            SeriesCollection seriesCollection2 = new SeriesCollection();
            List<string> labels2 = new List<string>();
            labels2.Add("With Voucher");
            labels2.Add("Without Voucher");
            //AxisYLabelFormatter = value => value.ToString("0.0");

            List<double> values2 = new List<double>();
            values2.Add(voucherService.GetWithVoucherPercentage(1));
            values2.Add(voucherService.GetWithoutVoucherPercentage(1));



            XvoucherLabels = labels2;

            ColumnSeries lineSeries2 = new ColumnSeries
            {

                Values = new ChartValues<double>(values2),
                DataLabels = true,
            };


            lineSeries2.LabelPoint = point => $"{point.Y}";
            seriesCollection2.Add(lineSeries2);
            chartVoucher.Series = seriesCollection2;



            try
            {
                Tour tour = tourGuidenceService.FindMostVisitedAllTime();
                ObservableCollection<Tour> tours = new ObservableCollection<Tour>();
                tours.Add(tour);
                MostPopularTour.ItemsSource = tours;
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occurred while loading: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                
        }

        private string CheckSuperType()
        {
            if (userService.FindSuperTypeByOwnerName(GuideUsername).Equals("super") == true)
            {
                return "super guide";
            }

            return "regular";
        }

        private void GeneratePDF(object sender, RoutedEventArgs e)
        {
            buttonPDF.Visibility = Visibility.Hidden;
            buttonBack.Visibility = Visibility.Hidden;
            Border1.Visibility = Visibility.Hidden;
            Border2.Visibility = Visibility.Hidden;
            this.Height = 900;
            this.Width = 775;

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(this, "pdf");
            }

            buttonPDF.Visibility = Visibility.Visible;
            buttonBack.Visibility = Visibility.Visible;
            this.Height = 900;
            this.Width = 775;

            MessageBox.Show("You have successfully saved the report in PDF format.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            string filePath = @"C:\Users\PC\Documents\GitHub\sims_turizam\InitialProject\InitialProject\Resources\Images";
            try
            {
                // Convert the file path to a URI
                Uri fileUri = new Uri(filePath);

                // Open the URI in the default web browser
                Process.Start(new ProcessStartInfo(fileUri.AbsoluteUri));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while opening the file: {ex.Message}");
            }

            Close();
        }

        private void GoToPreviousPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /*public void InitializePieChart()
        {
            pieChart.Series.Clear();

            pieChart.Series.Add(new PieSeries
            {
                Title = "with Voucher",
                
                Values = new ChartValues<double> { voucherService.GetWithVoucherPercentage(1) },
                DataLabels = true
            });

            pieChart.Series.Add(new PieSeries
            {
                Title = "without voucher",
                Values = new ChartValues<double> { voucherService.GetWithoutVoucherPercentage(1) },
                DataLabels = true
            });

            pieChart.LegendLocation = LegendLocation.Right;
            //pieChart.DefaultLegend.Visibility = Visibility.Visible;
        }*/
    }
}
