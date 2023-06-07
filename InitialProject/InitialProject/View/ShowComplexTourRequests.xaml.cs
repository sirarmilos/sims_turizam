using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Service;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for ShowComplexTourRequests.xaml
    /// </summary>
    public partial class ShowComplexTourRequests : Window
    {
        private TourRequestService tourRequestService;

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }

        public ShowComplexTourRequests(string guideUsername)
        {
            InitializeComponent();
            DataContext = this;
            Guide = guideUsername;
            tourRequestService = new TourRequestService();
            complexGrid.ItemsSource = tourRequestService.FindAllComplexRequestPending().Where(x => x.ComplexTourId != 0);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TourRequest selectedItem = (TourRequest)complexGrid.SelectedItem;

            TourGuidenceService tourGuidenceService = new TourGuidenceService();
            List<DateTime> dates = tourGuidenceService.RecommendDateForComplexTour(Guide, selectedItem.StartDate, selectedItem.EndDate);

            suggestedDates.ItemsSource = dates;
        }

   
    }
}
