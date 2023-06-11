using InitialProject.Model;
using InitialProject.Service;
using Microsoft.Win32;
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
    /// Interaction logic for Guest2RateTourAndGuide.xaml
    /// </summary>
    public partial class Guest2RateTourAndGuide : Page
    {
        private Guest2Service guest2Service;

        private readonly string username;
        private readonly string guideUsername;
        private readonly int tourGuidenceId;

        private List<string> pictures;

        private string generalKnowledgeVal;
        private string languageKnowledgeVal;
        private string tourExperienceVal;

        public string TourName { get; set; }
        public string GuideName { get; set; }

        public string GeneralKnowledgeVal
        {
            get { return generalKnowledgeVal; }
            set
            {
                if (generalKnowledgeVal != value)
                {
                    generalKnowledgeVal = value;
                    rateButton.IsEnabled = IsButtonActive();
                    OnPropertyChanged(nameof(GeneralKnowledgeVal));
                }
            }
        }

        public string LanguageKnowledgeVal
        {
            get { return languageKnowledgeVal; }
            set
            {
                if (languageKnowledgeVal != value)
                {
                    languageKnowledgeVal = value;
                    rateButton.IsEnabled = IsButtonActive();
                    OnPropertyChanged(nameof(LanguageKnowledgeVal));
                }
            }
        }

        public string TourExperienceVal
        {
            get { return tourExperienceVal; }
            set
            {
                if (tourExperienceVal != value)
                {
                    tourExperienceVal = value;
                    rateButton.IsEnabled = IsButtonActive();
                    OnPropertyChanged(nameof(TourExperienceVal));
                }
            }
        }

        public bool IsButtonActive()
        {
            if(string.IsNullOrEmpty(TourExperienceVal) || string.IsNullOrEmpty(LanguageKnowledgeVal) || string.IsNullOrEmpty(GeneralKnowledgeVal))
            {
                return false;
            }
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Guest2RateTourAndGuide(string username, TourGuidence tourGuidence)
        {
            InitializeComponent();
            DataContext = this;

            this.username = username;
            guideUsername = tourGuidence.Tour.GuideUsername;
            tourGuidenceId = tourGuidence.Id;

            pictures = new List<string>();
            guest2Service = new Guest2Service();

            GeneralKnowledgeVal = string.Empty;
            LanguageKnowledgeVal = string.Empty;
            TourExperienceVal = string.Empty;

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {

                string selectedFilePath = openFileDialog.FileName;
                pictures.Add(selectedFilePath);

            }
        }

        private void RateGuide(object sender, RoutedEventArgs e)
        {
            guest2Service.GuideRating(username, "Guide1", tourGuidenceId, Convert.ToInt32(GeneralKnowledgeVal), Convert.ToInt32(LanguageKnowledgeVal), Convert.ToInt32(TourExperienceVal), "", pictures);
            Guest2SuccesRating guest2SuccesRating = new Guest2SuccesRating(username);
            NavigationService.Navigate(guest2SuccesRating);

        }
    }
}