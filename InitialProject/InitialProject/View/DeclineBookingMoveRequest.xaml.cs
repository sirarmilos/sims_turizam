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
        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        private OwnerBookingMoveRequestsDTO selectedBookingMoveRequest;
        private string comment;

        public OwnerBookingMoveRequestsDTO SelectedBookingMoveRequest
        {
            get { return selectedBookingMoveRequest; }
            set
            {
                selectedBookingMoveRequest = value;
            }
        }

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
            }
        }

        public DataGrid DgBookingMoveRequests
        {
            get;
            set;
        }

        public DeclineBookingMoveRequest(ReservationReschedulingRequestService service, string owner, OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, DataGrid dgBookingMoveRequests)
        {
            InitializeComponent();

            Owner = owner;

            DataContext = this;

            reservationReschedulingRequestService = new ReservationReschedulingRequestService(service, Owner);

            DgBookingMoveRequests = dgBookingMoveRequests;
            SelectedBookingMoveRequest = selectedBookingMoveRequest;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ConfirmRejection(object sender, RoutedEventArgs e)
        {
            reservationReschedulingRequestService.SaveRejectedRequest(selectedBookingMoveRequest, comment);
            DgBookingMoveRequests.Items.Refresh();
            Close();
        }
    }
}
