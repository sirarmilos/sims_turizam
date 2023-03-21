using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    /// 

    public partial class TourReservation : Window
    {
        public static ObservableCollection<Tour> Tours { get; set; }

        private readonly TourRepository tourRepository = new TourRepository();

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
        public TourReservation(string tourId)
        {
            InitializeComponent();
            TourId = tourId;
            Tours  = new ObservableCollection<Tour>(tourRepository.GetByName(TourId));
            listTours.ItemsSource = Tours;
        }

        private void CreateReservation(object sender, RoutedEventArgs e)
        {
            numberOfGuests = Convert.ToInt32(guestNumber.Text);

            if (listTours.SelectedItems.Count != 1)
            {
                MessageBox.Show("Morate da odaberete turu koju zelite da rezervisete!");
            }
            else
            {
                Tour tour = new Tour();
                tour = (Tour)listTours.SelectedItems[0];

                if (numberOfGuests <= tour.FreeSlots)
                {
                    tourRepository.CreateReservation("korisnik1", tour, numberOfGuests);
                    MessageBox.Show("Uspesna rezervacija ture!");
                }
                else if(tour.FreeSlots > 0)
                {
                    MessageBox.Show("Za datu turu nema dovoljno mesta za rezervaciju. Preostalo je " + tour.FreeSlots.ToString() + "mesta.");
                }
                else
                {
                    MessageBox.Show("Za izabranu turu nema mesta za dati broj osoba. Predlozene ture:");
                    listTours.ItemsSource = tourRepository.SearchAndShow(tour.Location.City,tour.Location.Country,0,Model.Language.ALL,numberOfGuests);
                }
            }
        }
    }
}
