using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ChooseDateForTourRequest.xaml
    /// </summary>
    public partial class ChooseDateForTourRequest : Window
    {
        private readonly TourRequestService tourRequestService;

        private readonly TourGuidenceService tourGuidenceService;

        public string GuideUsername
        {
            get;
            set;
        }

        public TourRequest TourRequest { get; set; }

        private DateTime selectedStartDate;
        public DateTime SelectedStartDate
        {
            get { return selectedStartDate; }
            set
            {
                selectedStartDate = value;
            }
        }

        private string selectedTime;

        public string SelectedTime
        {
            get { return selectedTime;  }
            set
            {
                selectedTime = value;
            }
        }


        public ChooseDateForTourRequest(string username, TourRequest tourRequest)
        {
            InitializeComponent();
            DataContext = this;
            tourRequestService = new TourRequestService();
            tourGuidenceService = new TourGuidenceService();
            TourRequest = tourRequest;
            GuideUsername = username;
            SelectedStartDate = DateTime.Today;

        }

        private void AcceptTourRequest(object sender, RoutedEventArgs e)
        {
            if (tourRequestService.CheckIfIsInDateRange(TourRequest, SelectedStartDate))
            {
                if(tourGuidenceService.CheckIfGuideIsFreeInPeriod(GuideUsername, SelectedStartDate))
                {
                    tourRequestService.UpdateStatusToAccepted(TourRequest);
                    AddNewTour window = new AddNewTour(GuideUsername, TourRequest, SelectedStartDate);
                    window.Show();
                    Close();

                }
                else
                {
                    MessageBox.Show("You are not available in that period. Choose another date");
                }
                
            }
            else
            {
                MessageBox.Show("Selected date is not from date range in request!");
            }
            
        }
    }
}
