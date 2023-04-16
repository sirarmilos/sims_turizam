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

        private readonly Guest2Repository guest2Repository = new Guest2Repository();
        public RateGuide(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void RateGuidee(object sender, RoutedEventArgs e)
        {
     
            string guideGeneralKnowledgeValue = null;
            foreach (RadioButton radioButton in ((StackPanel)GuideKnowledge.Content).Children)
            {
                if (radioButton.IsChecked == true)
                {
                    guideGeneralKnowledgeValue = radioButton.Content.ToString();
                    break;
                }
            }

            // Get the selected value of the "Guide language knowledge" radio button group
            string guideLanguageKnowledgeValue = null;
            foreach (RadioButton radioButton in ((StackPanel)GuideLanguage.Content).Children)
            {
                if (radioButton.IsChecked == true)
                {
                    guideLanguageKnowledgeValue = radioButton.Content.ToString();
                    break;
                }
            }

            // Get the selected value of the "General tour experience" radio button group
            string generalTourExperienceValue = null;
            foreach (RadioButton radioButton in ((StackPanel)TourExperience.Content).Children)
            {
                if (radioButton.IsChecked == true)
                {
                    generalTourExperienceValue = radioButton.Content.ToString();
                    break;
                }
            }


            try 
            {
                guest2Repository.GuideRating(username,"Guide1",Convert.ToInt32(guideGeneralKnowledgeValue),Convert.ToInt32(guideLanguageKnowledgeValue),Convert.ToInt32(generalTourExperienceValue),pictures);
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
