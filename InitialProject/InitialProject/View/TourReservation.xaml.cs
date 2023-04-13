using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Repository;
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

        public TourReservation()
        {
            InitializeComponent();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
         
        public TourReservation(TourDisplayDTO tour)
        {
            InitializeComponent();
            Tours.Add(tour);
            listTours.ItemsSource = Tours;
        }


        private void CreateReservation(object sender, RoutedEventArgs e)
        {
            numberOfGuests = Convert.ToInt32(guestNumber.Text);

            if (listTours.SelectedItems.Count != 1)
            {
                MessageBox.Show("Morate da odaberete turu i datum koje zelite da rezervisete!");
            }
            else
            {
                TourDisplayDTO tourDisplayDTO = (TourDisplayDTO)listTours.SelectedItems[0];
                DateTime dateTime = tourDisplayDTO.TourDate;
                Tour tour = tourRepository.GetByName(tourDisplayDTO.TourName);
                TourGuidence tourGuidence = tourGuidenceRepository.GetByTourAndDate(tour,dateTime);
                List<Boolean> arrivals = tourReservationRepository.SetArrivalsToFalse(tourGuidence.Id);

                if(numberOfGuests<=tourGuidence.FreeSlots)
                {
                    if (tourGuidenceRepository.CreateReservation("korisnik1", tourGuidence, arrivals, numberOfGuests))
                        MessageBox.Show("Uspesna rezervacija ture!");
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
                    listTours.ItemsSource = tourRepository.SearchAndShow(tourGuidence.Tour.Location.City, tourGuidence.Tour.Location.Country, 0, Model.Language.ALL, 0);
                }

            }
        }

        private void listTours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
