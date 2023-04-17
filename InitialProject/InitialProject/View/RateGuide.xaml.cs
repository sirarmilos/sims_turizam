using InitialProject.Repository;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RateGuide.xaml
    /// </summary>
    public partial class RateGuide : Window
    {
        private List<string> pictures = new List<string>();

        private readonly string username;
        private readonly string guideUsername;
        private readonly int tourGuidenceId;

        private readonly Guest2Repository guest2Repository = new Guest2Repository();
        public RateGuide(string username,int tourGuidenceId,string guideUsername)
        {
            InitializeComponent();
            this.username = username;
            this.guideUsername = guideUsername;
            this.tourGuidenceId = tourGuidenceId;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void RateGuidee(object sender, RoutedEventArgs e)
        {
     
            int guideGeneralKnowledgeValue = 0;
            foreach (RadioButton radioButton in ((StackPanel)GuideKnowledge.Content).Children)
            {
                if (radioButton.IsChecked == true)
                {
                    guideGeneralKnowledgeValue = Convert.ToInt32(radioButton.Content.ToString());
                    break;
                }
            }

            // Get the selected value of the "Guide language knowledge" radio button group
            int guideLanguageKnowledgeValue = 0;
            foreach (RadioButton radioButton in ((StackPanel)GuideLanguage.Content).Children)
            {
                if (radioButton.IsChecked == true)
                {
                    guideLanguageKnowledgeValue = Convert.ToInt32(radioButton.Content.ToString());
                    break;
                }
            }

            // Get the selected value of the "General tour experience" radio button group
            int generalTourExperienceValue = 0;
            foreach (RadioButton radioButton in ((StackPanel)TourExperience.Content).Children)
            {
                if (radioButton.IsChecked == true)
                {
                    generalTourExperienceValue = Convert.ToInt32(radioButton.Content.ToString());
                    break;
                }
            }


            try 
            {
                guest2Repository.GuideRating(username,guideUsername,tourGuidenceId,guideGeneralKnowledgeValue,guideGeneralKnowledgeValue,generalTourExperienceValue,tourComment.Text,pictures);
                MessageBox.Show("Success!");
            }
            catch
            {
                MessageBox.Show("Error!");
            }

        }

        private void AddPictureToList(object sender, RoutedEventArgs e)
        {
            pictures.Add(pictureURL.Text);
            pictureURL.Text= string.Empty;
        }
    }
}
