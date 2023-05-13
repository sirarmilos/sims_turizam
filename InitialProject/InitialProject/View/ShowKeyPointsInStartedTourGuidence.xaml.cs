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

        //private readonly TourGuidenceService tourGuidenceService;

        //private readonly TourKeyPointService tourKeyPointService;

        //private readonly TourKeyPointRepository tourKeyPointRepository;
        public static ObservableCollection<TourKeyPoint> tourKeyPoints { get; set; }

        public TourGuidence TourGuidence { get; set; }

        public List<TourGuidence> TourGuidences;

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }



        public ShowKeyPointsInStartedTourGuidence(TourGuidence guidence, List<TourGuidence> guidences)
        {
            InitializeComponent();
            DataContext = this;
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            TourKeyPointService tourKeyPointService = new TourKeyPointService();
            TourGuidenceService tourGuidenceService = new TourGuidenceService();
            tourKeyPoints = new ObservableCollection<TourKeyPoint>(tourKeyPointService.FindByTourGuidance(guidence.Id));
            tourKeyPoints[0].Passed = true;
            TourGuidence = guidence;
            this.TourGuidences = guidences;
            Guide = "Guide1";
        }

        private void SaveCheckedKeyPoints(object sender, RoutedEventArgs e)
        {
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            TourGuidenceService tourGuidenceService = new TourGuidenceService();
            if (TourGuidence.Started == true)
            {
                tourKeyPointRepository.UpdateCheckedKeyPoints(tourKeyPoints.ToList());
                if (tourKeyPoints[tourKeyPoints.Count - 1].Passed == true)
                {
                    tourGuidenceService.UpdateFinishedField(TourGuidence.Id);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("First start your tour!!!");
            }

        }

        private void MarkPresentGuests(object sender, RoutedEventArgs e)
        {
            if (TourGuidence.Started == true)
            {
                ShowGuestOnTourGuidence window = new ShowGuestOnTourGuidence(TourGuidence.Id);
                window.Show();
            }
            else
            {
                MessageBox.Show("First start your tour!!!");
            }

        }

        private void StartTourGuidence(object sender, RoutedEventArgs e)
        {
            TourGuidenceService tourGuidenceService = new TourGuidenceService();
            if (tourGuidenceService.CheckGuidencesForStart(TourGuidences))
            {
                MessageBox.Show("You already started another tour!");
                this.Close();
                ShowTourGuidences window = new ShowTourGuidences(Guide);
                window.Show();
            }
            else
            {
                tourGuidenceService.UpdateStartedField(TourGuidence.Id);
                MessageBox.Show("Tour successfully started");
                //ShowKeyPointsInStartedTourGuidence window = new(TourGuidence, TourGuidences);
                this.Close();
                //window.Show();
                ShowTourGuidences window = new ShowTourGuidences(Guide);
                window.Show();
            }

        }

        private void FinishTourGuidence(object sender, RoutedEventArgs e)
        {
            TourGuidenceService tourGuidenceService = new TourGuidenceService();
            if (TourGuidence.Started == false)
            {
                MessageBox.Show("Start your tour first!!");
            }
            else
            {
                tourGuidenceService.UpdateFinishedField(TourGuidence.Id);
                MessageBox.Show("Tour successfully finished");
                this.Close();
                ShowTourGuidences window = new ShowTourGuidences(Guide);
                window.Show();
            }

            
        }

    }
}
