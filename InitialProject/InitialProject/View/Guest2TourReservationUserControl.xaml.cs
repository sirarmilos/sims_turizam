using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2TourReservationUserControl.xaml
    /// </summary>
    public partial class Guest2TourReservationUserControl : UserControl
    {
        private readonly Guest2Service guest2Service = new Guest2Service();
        private string Username { get; set; }


        public string TourName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string Duration { get; set; }
        public string Date { get; set; }
        public string KeyPoints { get; set; }

        public Guest2TourReservationUserControl()
        {
            InitializeComponent();
            InitializeComboBoxVouchers();
        }

        public Guest2TourReservationUserControl(string username, TourDisplayDTO tourDisplayDTO)
        {
            InitializeComponent();
            Username = username;

            InitializeComboBoxVouchers();

            TourName = tourDisplayDTO.TourName;
            tourName.Content = TourName;

            Country = tourDisplayDTO.Location.Country;
            country.Content = Country;

            City = tourDisplayDTO.Location.City;
            city.Content = City;

            Description = tourDisplayDTO.Description;
            description.Content = Description;


            Language = tourDisplayDTO.Language.ToString();
            language.Content = Language;

            Duration = tourDisplayDTO.Duration.ToString();
            duration.Content = Duration;

            Date = tourDisplayDTO.TourDate.ToString().Split(" ")[0];
            date.Content = Date;

            KeyPoints = tourDisplayDTO.TourKeyPoints[0].KeyPointName.ToString();
            keyPoints.Content = KeyPoints;

            BitmapImage loadedImage = new BitmapImage(new Uri(tourDisplayDTO.Images[0]));
            image.Source = loadedImage;
        }


        public void InitializeComboBoxVouchers()
        {
            List<Voucher> vouchers = guest2Service.GetGuestsVouchers(Username);

            foreach (Voucher voucher in vouchers)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = voucher.voucherType.ToString();
                item.Tag = voucher.Id.ToString();

                ComboBoxVouchers.Items.Add(item);
            }

            ComboBoxVouchers.Items.Insert(0, new ComboBoxItem { Content = "Select a voucher...", Tag = "0" });
            ComboBoxVouchers.SelectedIndex = 0;
        }

        private void CancelReservation(object sender, RoutedEventArgs e)
        {
            Guest2PageTours guest2PageTours = new Guest2PageTours();
            page.Content = guest2PageTours;
        }
    }
}
