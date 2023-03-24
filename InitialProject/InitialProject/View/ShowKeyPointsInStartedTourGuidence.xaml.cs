using InitialProject.Model;
using InitialProject.Repository;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for ShowKeyPointsInStartedTourGuidence.xaml
    /// </summary>
    public partial class ShowKeyPointsInStartedTourGuidence : Window
    {
        private readonly TourKeyPointRepository tourKeyPointRepository;

        private readonly TourGuidenceRepository tourGuidenceRepository;
        public static ObservableCollection<TourKeyPoint> tourKeyPoints { get; set; }

        public ShowKeyPointsInStartedTourGuidence(int tourGuidenceId)
        {
            InitializeComponent();
            DataContext = this;
            tourKeyPointRepository = new TourKeyPointRepository();
            tourGuidenceRepository = new TourGuidenceRepository();
            tourKeyPoints = new ObservableCollection<TourKeyPoint>(tourKeyPointRepository.Load(tourGuidenceId));
            tourKeyPoints[0].Passed = true;
        }

        private void SaveCheckedKeyPoints(object sender, RoutedEventArgs e)
        {
            tourKeyPointRepository.UpdateCheckedKeyPoints();
            if (tourKeyPoints[tourKeyPoints.Count - 1].Passed == true)
            {
                tourGuidenceRepository.UpdateFinishedField(tourKeyPoints[0].TourGuidence.Id);
            }
            this.Close();
        }
    }
}
