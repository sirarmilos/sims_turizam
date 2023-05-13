using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for ShowKeyPoints.xaml
    /// </summary>
    public partial class ShowKeyPoints : Window
    {
        public static ObservableCollection<TourKeyPoint> tourKeyPoints { get; set; }

        public ShowKeyPoints(TourGuidence tourGuidence)
        {
            InitializeComponent();
            DataContext = this;
            TourKeyPointService tourKeyPointService = new TourKeyPointService();
            tourKeyPoints = new ObservableCollection<TourKeyPoint>(tourKeyPointService.FindByTourGuidance(tourGuidence.Id));
        }
    }
}
