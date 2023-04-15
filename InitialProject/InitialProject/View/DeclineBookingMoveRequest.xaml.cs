using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
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
    /// Interaction logic for DeclineBookingMoveRequest.xaml
    /// </summary>
    public partial class DeclineBookingMoveRequest : Window
    {
        public OwnerManageBookingMoveRequests ownerManageBookingMoveRequestsForm;

        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;

        public string Comment
        {
            get;
            set;
        }

        public DeclineBookingMoveRequest(ReservationReschedulingRequestService service, OwnerManageBookingMoveRequests form)
        {
            InitializeComponent();

            DataContext = this;

            reservationReschedulingRequestService = service;

            ownerManageBookingMoveRequestsForm = form;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            ownerManageBookingMoveRequestsForm.dgBookingMoveRequests.ItemsSource = reservationReschedulingRequestService.FindPendingRequests();

            Close();
        }

        private void ConfirmRejection(object sender, RoutedEventArgs e)
        {
            reservationReschedulingRequestService.SaveRejectedRequest(ownerManageBookingMoveRequestsForm.SelectedBookingMoveRequest, Comment);

            ownerManageBookingMoveRequestsForm.dgBookingMoveRequests.ItemsSource = reservationReschedulingRequestService.FindPendingRequests();

            Close();
        }
    }
}
