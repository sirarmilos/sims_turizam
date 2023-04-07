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

        private List<ReservationReschedulingRequest> allReservationReschedulingRequests;

        private List<OwnerBookingMoveRequestsDTO> ownerBookingMoveRequestsDTOs;

        private DataGrid dgBookingMoveRequests;

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

        private OwnerBookingMoveRequestsDTO SelectedBookingMoveRequest
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

        public List<ReservationReschedulingRequest> AllReservationReschedulingRequests
        {
            get;
            set;
        }

        public List<OwnerBookingMoveRequestsDTO> OwnerBookingMoveRequestsDTOs
        {
            get;
            set;
        }

        public DataGrid DgBookingMoveRequests
        {
            get;
            set;
        }

        public DeclineBookingMoveRequest(List<OwnerBookingMoveRequestsDTO> ownerBookingMoveRequestsDTOs, DataGrid dgBookingMoveRequests, string owner, OwnerBookingMoveRequestsDTO selectedBookingMoveRequest, List<ReservationReschedulingRequest> allReservationReschedulingRequests)
        {
            InitializeComponent();
            Owner = owner;
            DataContext = this;

            reservationReschedulingRequestRepository = new ReservationReschedulingRequestRepository();

            OwnerBookingMoveRequestsDTOs = ownerBookingMoveRequestsDTOs;
            DgBookingMoveRequests = dgBookingMoveRequests;
            SelectedBookingMoveRequest = selectedBookingMoveRequest;
            AllReservationReschedulingRequests = allReservationReschedulingRequests;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ConfirmRejection(object sender, RoutedEventArgs e)
        {
            FindAndUpdateRescheduledReservation();

            OwnerBookingMoveRequestsDTOs.Remove(SelectedBookingMoveRequest);
            DgBookingMoveRequests.Items.Refresh();

            Close();
        }

        private void FindAndUpdateRescheduledReservation()
        {
            foreach (ReservationReschedulingRequest temporaryReservationReschedulingRequest in AllReservationReschedulingRequests)
            {
                if (temporaryReservationReschedulingRequest.Reservation.ReservationId == SelectedBookingMoveRequest.ReservationId)
                {
                    temporaryReservationReschedulingRequest.Status = "rejected";
                    temporaryReservationReschedulingRequest.Comment = Comment;
                    break;
                }
            }

            reservationReschedulingRequestRepository.UpdateReservationReschedulingRequest(AllReservationReschedulingRequests);
        }
    }
}
