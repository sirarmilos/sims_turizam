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
    /// Interaction logic for ShowReviewsGuest2.xaml
    /// </summary>
    public partial class ShowReviewsGuest2 : Window
    {
        public static ObservableCollection<Dto.RateGuideDisplayDto> guideRates { get; set; }

        private readonly RateGuideRepository rateGuideRepository;

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }

        public ShowReviewsGuest2(string guide)
        {
            InitializeComponent();
            DataContext = this;
            Guide = guide;
            rateGuideRepository = new RateGuideRepository();
            guideRates = new ObservableCollection<Dto.RateGuideDisplayDto>(rateGuideRepository.FindForDisplay(Guide));
        }

        private void DeleteRateLogical(object sender, RoutedEventArgs e)
        {

            Dto.RateGuideDisplayDto selectedRate = (Dto.RateGuideDisplayDto)dataGrid.SelectedItem;
            if(selectedRate != null)
            {
                rateGuideRepository.UpdateIsDeleted(selectedRate.UserId, selectedRate.tourGuidenceId);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select one review!");
            }
        }
            
    }
}
