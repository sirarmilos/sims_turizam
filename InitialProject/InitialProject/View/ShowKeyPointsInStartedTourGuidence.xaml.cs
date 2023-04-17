using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
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
        private readonly TourKeyPointService tourKeyPointService = new TourKeyPointService();

        private readonly TourKeyPointRepository tourKeyPointRepository;

        private readonly TourGuidenceRepository tourGuidenceRepository;

        private readonly TourGuidenceService tourGuidenceService;
        public static ObservableCollection<TourKeyPoint> tourKeyPoints { get; set; }

        public TourGuidence TourGuidence { get; set; }

        public List<TourGuidence> TourGuidences;

        

        public ShowKeyPointsInStartedTourGuidence(TourGuidence guidence, List<TourGuidence> guidences)
        {
            InitializeComponent();
            DataContext = this;
            tourKeyPointRepository = new TourKeyPointRepository();
            tourGuidenceRepository = new TourGuidenceRepository();
            tourGuidenceService = new TourGuidenceService();
            tourKeyPoints = new ObservableCollection<TourKeyPoint>(tourKeyPointService.GetByTourGuidance(guidence.Id));
            tourKeyPoints[0].Passed = true;
            TourGuidence = guidence;
            this.TourGuidences = guidences;
        }

        private void SaveCheckedKeyPoints(object sender, RoutedEventArgs e)
        {
            tourKeyPointRepository.UpdateCheckedKeyPoints(tourKeyPoints.ToList());
            if (tourKeyPoints[tourKeyPoints.Count - 1].Passed == true)
            {
                tourGuidenceService.UpdateFinishedField(TourGuidence.Id);
            }
            this.Close();
        }

        private void MarkPresentGuests(object sender, RoutedEventArgs e)
        {

            ShowGuestOnTourGuidence window = new ShowGuestOnTourGuidence(TourGuidence.Id);
            window.Show();

        }

        private void StartTourGuidence(object sender, RoutedEventArgs e)
        {
            if (tourGuidenceService.CheckGuidencesForStart(TourGuidences))
            {
                MessageBox.Show("You already started another tour!");
                this.Close();
                ShowTourGuidences window = new();
                window.Show();
            }
            else
            {
                tourGuidenceService.UpdateStartedField(TourGuidence.Id);
                MessageBox.Show("Tour successfully started");
                //ShowKeyPointsInStartedTourGuidence window = new(TourGuidence, TourGuidences);
                this.Close();
                //window.Show();
                ShowTourGuidences window = new();
                window.Show();
            }

        }

        private void FinishTourGuidence(object sender, RoutedEventArgs e)
        {
            if(TourGuidence.Started == false)
            {
                MessageBox.Show("Start your tour first!!");
            }
            else
            {
                tourGuidenceService.UpdateFinishedField(TourGuidence.Id);
                MessageBox.Show("Tour successfully finished");
                this.Close();
                ShowTourGuidences window = new();
                window.Show();
            }

            
        }

    }
}
