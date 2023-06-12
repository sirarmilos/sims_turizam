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
    /// Interaction logic for Guest2DisplayRequestedTours.xaml
    /// </summary>
    public partial class Guest2DisplayRequestedTours : Page
    {
        TourRequestService tourRequestService = new TourRequestService();
        public List<TourRequest> listRequestedTours { get ; set; }

        private string username;

        public Guest2DisplayRequestedTours(string username)
        {
            InitializeComponent();
            listRequestedTours = new List<TourRequest>();
            listTourRequests.ItemsSource = tourRequestService.GetByUser(username);
            this.username = username;
        }

    }
}
