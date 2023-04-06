using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
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
        public DeclineBookingMoveRequest declineBookingMoveRequest { get; set; }

        private readonly ReservationReschedulingRequestRepository reservationReschedulingRequestRepository;

        private ReservationReschedulingRequest reservationReschedulingRequestToReject;

        private List<ReservationReschedulingRequest> reservationReschedulingRequests;

        private readonly OwnerManageBookingMoveRequests OwnerManageBookingMoveRequest;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        private string comment;

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
            }
        }

        public ReservationReschedulingRequest ReservationReschedulingRequestToReject
        {
            get;
            set;
        }

        public List<ReservationReschedulingRequest> ReservationReschedulingRequests
        {
            get;
            set;
        }

        public DeclineBookingMoveRequest(OwnerManageBookingMoveRequests ownerManageBookingMoveRequest ,string owner, ReservationReschedulingRequest reservationReschedulingRequestToReject, List<ReservationReschedulingRequest> allReservationReschedulingRequests)
        {
            InitializeComponent();
            Owner = owner;
            DataContext = this;
            reservationReschedulingRequestRepository = new ReservationReschedulingRequestRepository();
            ReservationReschedulingRequestToReject = reservationReschedulingRequestToReject;
            ReservationReschedulingRequests = allReservationReschedulingRequests;
            OwnerManageBookingMoveRequest = ownerManageBookingMoveRequest;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ConfirmRejection(object sender, RoutedEventArgs e)
        {
            ReservationReschedulingRequestToReject.Status = "rejected";
            ReservationReschedulingRequestToReject.Comment = Comment;

            reservationReschedulingRequestRepository.UpdateReservationReschedulingRequest(ReservationReschedulingRequests);

            OwnerManageBookingMoveRequest.OwnerBookingMoveRequestsDTOs.Remove(OwnerManageBookingMoveRequest.SelectedBookingMoveRequest);

            OwnerManageBookingMoveRequest.dgBookingMoveRequests.Items.Refresh();

            Close();
        }
    }
}
