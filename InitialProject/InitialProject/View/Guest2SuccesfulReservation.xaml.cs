using InitialProject.Dto;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Path = System.IO.Path;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2SuccesfulReservation.xaml
    /// </summary>
    public partial class Guest2SuccesfulReservation : Page
    {
        private string Username;

        private int NumberOfGuests;

        private TourDisplayDTO tourDisplayDTO;

        private string voucher;
        public Guest2SuccesfulReservation(string username, TourDisplayDTO tourDisplayDTO, int numberOfGuests,string voucher)
        {
            InitializeComponent();
            Username = username;
            DataContext = this;
            this.tourDisplayDTO = tourDisplayDTO;
            NumberOfGuests = numberOfGuests;
            this.voucher = voucher;
        }

        private void HyperLinkHomePage(object sender, RoutedEventArgs e)
        {
            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            NavigationService.Navigate(guest2PageTours);
        }


        private void HyperLinkCreateMoreReservations(object sender, RoutedEventArgs e)
        {
            Guest2TourReservation guest2TourReservation = new Guest2TourReservation(Username,tourDisplayDTO);
            NavigationService.Navigate(guest2TourReservation);
        }

        private void GenerateReport(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            Guest2Report guest2Report = new Guest2Report(tourDisplayDTO, Username, NumberOfGuests,voucher);
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(guest2Report, "Report");
            }
        }
    }
}
