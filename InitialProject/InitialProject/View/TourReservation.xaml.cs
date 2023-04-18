using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    /// 

    public partial class TourReservation : Window
    {
        public List<TourDisplayDTO> Tours = new List<TourDisplayDTO>();

        private readonly TourRepository tourRepository = new TourRepository();

        private readonly TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();

        private readonly TourReservationRepository tourReservationRepository = new TourReservationRepository();

        private readonly Guest2Repository guest2Repository = new Guest2Repository();

        private TourReservationService tourReservationService = new TourReservationService();

        private TourService tourService = new TourService();

        private readonly string username;

        private string TourId { get; set; }

        private int numberOfGuests;

        public int NumberOfGuests
        {
            get { return numberOfGuests; }
            set
            {
                numberOfGuests = value;
                OnPropertyChanged(nameof(NumberOfGuests));
            }
        }

        public TourReservation(string username)
        {
            InitializeComponent();
            InitializeComboBoxVouchers();
            this.username = username;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
         
        public TourReservation(TourDisplayDTO tour,string username)
        {
            InitializeComponent();
            Tours.Add(tour);
            listTours.ItemsSource = Tours;
            this.username = username;
            InitializeComboBoxVouchers();
        }

        public void InitializeComboBoxVouchers()
        {
            List<Voucher> vouchers = guest2Repository.GetGuestsVouchers(username);  

            foreach (Voucher voucher in vouchers)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = voucher.voucherType.ToString() + "   " + voucher.expirationDate.ToString();
                item.Tag = voucher.Id.ToString();

                ComboBoxVouchers.Items.Add(item);
            }

            ComboBoxVouchers.Items.Insert(0, new ComboBoxItem { Content = "Select a voucher...", Tag = "0" });
            ComboBoxVouchers.SelectedIndex = 0;
        }


        private void CreateReservation(object sender, RoutedEventArgs e)
        {
            numberOfGuests = Convert.ToInt32(guestNumber.Text);

            ComboBoxItem selectedItem = ComboBoxVouchers.SelectedItem as ComboBoxItem;

            int voucherId=0;

            if (selectedItem!=null)
            {
                string selectedValue = selectedItem.Tag as string;

                if(int.TryParse(selectedValue,out voucherId))
                {
                }
                else
                {
                    voucherId = 0;
                }
            }

            if (listTours.SelectedItems.Count != 1)
            {
                MessageBox.Show("Morate da odaberete turu i datum koje zelite da rezervisete!");
            }
            else
            {
                TourDisplayDTO tourDisplayDTO = (TourDisplayDTO)listTours.SelectedItems[0];
                DateTime dateTime = tourDisplayDTO.TourDate;
                Tour tour = tourRepository.FindByName(tourDisplayDTO.TourName);
                TourGuidence tourGuidence = tourGuidenceRepository.FindByTourAndDate(tour,dateTime);

                if(numberOfGuests<=tourGuidence.FreeSlots)
                {
                    if (tourReservationService.CreateReservation(username, tourGuidence, numberOfGuests, voucherId, tourReservationRepository.NextId()))
                    {
                        guest2Repository.UpdateVoucherUsedStatus(voucherId);
                        MessageBox.Show("Uspesna rezervacija ture!");
                    }
                    else
                        MessageBox.Show("Greska prilikom kreiranja rezervacije!");
                }

                else if (tourGuidence.FreeSlots > 0)
                {
                    MessageBox.Show("Za datu turu nema dovoljno mesta za rezervaciju. Preostalo je " + tourGuidence.FreeSlots.ToString() + " mesta.");
                }
                else if(tourGuidence.FreeSlots == 0)
                {
                    MessageBox.Show("Za datu turu nema vise mesta. Predlozene ture u istom gradu:");
                    listTours.ItemsSource = tourService.SearchAndShow(tourGuidence.Tour.Location.City, tourGuidence.Tour.Location.Country, 0, Model.Language.ALL, 0);
                }

            }

        }

        private void listTours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
