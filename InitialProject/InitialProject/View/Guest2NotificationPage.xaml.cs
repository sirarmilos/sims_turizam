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
    /// Interaction logic for Guest2NotificationPage.xaml
    /// </summary>
    public partial class Guest2NotificationPage : Page
    {
        private string Username;

        private TourNotificationsService tourNotificationsService;

        public Guest2NotificationPage(string username)
        {
            InitializeComponent();
            Username = username;

            tourNotificationsService = new TourNotificationsService();

            tourNotificationsService.Update();

            dataGrid.ItemsSource = tourNotificationsService.NotifyOfNewTour(Username);
        }
    }
}
