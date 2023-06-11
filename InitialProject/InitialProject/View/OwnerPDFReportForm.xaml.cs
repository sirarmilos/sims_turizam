using InitialProject.DTO;
using InitialProject.Service;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for OwnerPDFReportForm.xaml
    /// </summary>
    public partial class OwnerPDFReportForm : Window
    {
        private readonly ReviewService reviewService;

        public string OwnerUsername
        {
            get;
            set;
        }

        public List<OwnerPDFReportDTO> OwnerPDFReportDTOs
        {
            get;
            set;
        }

        public OwnerPDFReportForm(string ownerUsername)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            reviewService = new ReviewService(OwnerUsername);

            SetDefaultValue();
        }

        private void SetDefaultValue()
        {
            OwnerPDFReportDTOs = new List<OwnerPDFReportDTO>();
            OwnerPDFReportDTOs = reviewService.FindOwnerPDFReportDTOs(OwnerUsername);
            lvShowGuestReviews.ItemsSource = OwnerPDFReportDTOs;

            labelOwnerUsername.Content = OwnerUsername;
            labelOwnerType.Content = CheckSuperType();
            labelNumberOfAccommodations.Content = OwnerPDFReportDTOs.Count.ToString();
            labelReportDate.Content = DateTime.Now.ToString("dd/MM/yyyy");
            labelReportTime.Content = DateTime.Now.ToString("HH:mm:ss");
        }

        private string CheckSuperType()
        {
            if (reviewService.FindSuperTypeByOwnerName(OwnerUsername).Equals("super") == true)
            {
                return "super owner";
            }
            
            return "regular";
        }

        private void PDF_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PDF_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            buttonPDF.Visibility = Visibility.Hidden;
            buttonBack.Visibility = Visibility.Hidden;
            this.Height = 639;
            this.Width = 1400;

            Keyboard.ClearFocus();

            lvShowGuestReviews.SelectedItem = null;

            PrintDialog printDialog = new PrintDialog();
            if(printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(this, "pdf");

                MessageBox.Show("You have successfully saved the report in PDF format.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            else
            {
                buttonPDF.Visibility = Visibility.Visible;
                buttonBack.Visibility = Visibility.Visible;
                this.Height = 740;
                this.Width = 1416;
            }
        }

        private void BackFromPDFReport_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void BackFromPDFReport_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
