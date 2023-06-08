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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for GuideMarkGuests.xaml
    /// </summary>
    public partial class GuideMarkGuests : Window
    {
        public TourKeyPoint TourKeyPoint { get; set; }

        public TourGuidence TourGuidence { get; set; }

        public static List<Dto.ReservationDisplayDto> tourReservations { get; set; }

        public List<User> users { get; set; }



        public string WelcomeText
        {
            get; set;
        }

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }

        public GuideMarkGuests(TourKeyPoint tkp, TourGuidence tg, string guide)
        {
            InitializeComponent();
            DataContext = this;
            TourKeyPoint = tkp;
            TourGuidence = tg;
            Guide = guide;
            WelcomeText = "Mark New Guests - " + TourKeyPoint.KeyPointName.ToString();
            TourReservationService tourReservationService = new TourReservationService();
            tourReservations = new List<Dto.ReservationDisplayDto>(tourReservationService.GetAllForOneTourGuidence(TourGuidence.Id));
            UserService userService = new UserService();
            users = new List<User>();
            users.Add(userService.FindByUsername("Guest21"));
            //addedGuest.ItemsSource = users;

        }

        private void GoToBack(object sender, RoutedEventArgs e)
        {
            GuideMarkKeyPoints window = new GuideMarkKeyPoints(TourGuidence, Guide);
            this.Close();
            window.Show();
        }

        private void GoToAddGuest(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            Dto.ReservationDisplayDto selectedItem = (Dto.ReservationDisplayDto)allGuests.SelectedItem;
            users.Add(userService.FindByUsername(selectedItem.userId));
            addedGuest.ItemsSource = users;
        }
    }
}
