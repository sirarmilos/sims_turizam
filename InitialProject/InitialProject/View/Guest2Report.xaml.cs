using InitialProject.Dto;
using Org.BouncyCastle.Asn1.Mozilla;
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
    /// Interaction logic for Guest2Report.xaml
    /// </summary>
    public partial class Guest2Report : Page
    {
        public string Username { get; set; }
        public string CreationDate { get; set; }
        public string TourName { get; set; }

        public string TourDate { get; set; }

        public string TourCountry { get; set; }
        public string TourCity { get; set; }
        public string TourGuests { get; set; }
        public string TourDuration { get; set; }
        public string TourDescription { get; set; }

        public string TourVoucher { get; set; }

        public Guest2Report(TourDisplayDTO tourDisplayDTO,string username,int guests,string voucher)
        {
            InitializeComponent();
            DataContext = this;

            Username = username;
            CreationDate = DateTime.Now.ToString();
            TourName = tourDisplayDTO.TourName;
            TourDate = tourDisplayDTO.TourDate.ToString();
            TourCountry = tourDisplayDTO.Location.Country;
            TourCity = tourDisplayDTO.Location.City;
            TourGuests = guests.ToString();
            TourDuration = tourDisplayDTO.Duration.ToString();
            TourDescription = tourDisplayDTO.Description;

            if(string.IsNullOrEmpty(voucher))
            {
                TourVoucher = "None";
            }
            else
            {
                TourVoucher = voucher;
            }
        }
    }
}
