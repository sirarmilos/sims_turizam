using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Guest2TourReservation.xaml
    /// </summary>
    public partial class Guest2TourReservation : Page
    {
        private readonly Guest2Service guest2Service = new Guest2Service();
        
        private readonly TourService tourService = new TourService();

        private readonly TourGuidenceService tourGuidenceService = new TourGuidenceService();
        
        private readonly TourReservationService tourReservationService = new TourReservationService();  

        private string Username { get; set; }


        public string TourName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string Duration { get; set; }
        public string Date { get; set; }
        public string KeyPoints { get; set; }

        private string maxGuests;

        public string MaxGuests
        {
            get { return maxGuests; }
            set
            {
                if (int.TryParse(value.ToString(), out int result) || string.IsNullOrEmpty(value))
                {
                    if (int.Parse(value) < 0)
                    {
                        maxGuests = value;
                        guestsNumWarning.Visibility = Visibility.Visible;
                        createReservationButton.IsEnabled = false;
                    }
                    else
                    {
                        if (tourDisplayDTO.FreeSlots < int.Parse(value))
                        {
                            maxGuests = value;
                            guestsNumWarning.Visibility = Visibility.Hidden;
                            createReservationButton.IsEnabled = false;
                            numOfSlotsLabel.Content = tourDisplayDTO.FreeSlots.ToString();

                            warning1label.Visibility = Visibility.Visible;
                            warning2label.Visibility = Visibility.Visible;
                            numOfSlotsLabel.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            maxGuests = value;
                            guestsNumWarning.Visibility = Visibility.Hidden;
                            createReservationButton.IsEnabled = true;

                            warning1label.Visibility = Visibility.Hidden;
                            warning2label.Visibility = Visibility.Hidden;
                            numOfSlotsLabel.Visibility = Visibility.Hidden;
                        }
                    }
                }
                else
                {
                    maxGuests = value;
                    guestsNumWarning.Visibility = Visibility.Visible;
                    createReservationButton.IsEnabled = false;
                }

                OnPropertyChanged(nameof(MaxGuests));
            }
        }

        public TourDisplayDTO tourDisplayDTO { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Guest2TourReservation()
        {
            InitializeComponent();
            InitializeComboBoxVouchers();
            DataContext = this;
        }

        public Guest2TourReservation(string username,TourDisplayDTO tourDisplayDTO)
        {
            InitializeComponent();
            Username = username;

            InitializeComboBoxVouchers();

            this.tourDisplayDTO = tourDisplayDTO;

            DataContext = this;

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
            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            //Window parent = Window.GetWindow(this);
            NavigationService.Navigate(guest2PageTours);
            //page.Content = guest2PageTours;

        }

        private void createReservationButton_Click(object sender, RoutedEventArgs e)
        {
            int numberOfGuests = int.Parse(MaxGuests);

            ComboBoxItem selectedItem = ComboBoxVouchers.SelectedItem as ComboBoxItem;

            int voucherId = 0;

            if (selectedItem != null)
            {
                string selectedValue = selectedItem.Tag as string;

                if (int.TryParse(selectedValue, out voucherId))
                {
                }
                else
                {
                    voucherId = 0;
                }
            }

            Tour tour = tourService.FindByName(tourDisplayDTO);
            TourGuidence tourGuidence = tourGuidenceService.FindByTourAndDate(tour,tourDisplayDTO.TourDate);

            if(numberOfGuests<=tourGuidence.FreeSlots)
            {
                if(tourReservationService.CreateReservation(Username,tourGuidence,numberOfGuests,voucherId,tourReservationService.NextId()))
                {
                    guest2Service.UpdateVoucherUsedStatus(voucherId);
                    Guest2RateTourAndGuide guest2RateTourAndGuide = new Guest2RateTourAndGuide();
                    NavigationService.Navigate(guest2RateTourAndGuide);
                }
            }

        }
    }
}
