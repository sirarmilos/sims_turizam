using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for OwnerManageBookingMoveRequests.xaml
    /// </summary>
    public partial class OwnerManageBookingMoveRequests : Window
    {
        public OwnerManageBookingMoveRequests ownerManageBookingMoveRequests { get; set; }

        private readonly ReservationReschedulingRequestRepository reservationReschedulingRequestRepository;

        private readonly ReservationRepository reservationRepository;

        private List<ReservationReschedulingRequest> reservationReschedulingRequests;

        private List<ReservationReschedulingRequest> allReservationReschedulingRequests;

        private List<OwnerBookingMoveRequestsDTO> ownerBookingMoveRequestsDTOs;

        private List<Reservation> reservations;

        private List<Reservation> allReservations;

        private List<BusyReservation> busyReservations;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        private string guestUsername;
        private string accommodationName;
        private DateTime oldStartDate;
        private DateTime oldEndDate;
        private DateTime newStartDate;
        private DateTime newEndDate;
        private string newDateAvailable;

        private OwnerBookingMoveRequestsDTO selectedBookingMoveRequest;

        public string GuestUsername
        {
            get { return guestUsername; }
            set
            {
                guestUsername = value;
            }
        }

        public string AccommodationName
        {
            get { return accommodationName; }
            set
            {
                accommodationName = value;
            }
        }

        public DateTime OldStartDate
        {
            get { return oldStartDate; }
            set
            {
                oldStartDate = value;
            }
        }

        public DateTime OldEndtDate
        {
            get { return oldEndDate; }
            set
            {
                oldEndDate = value;
            }
        }

        public DateTime NewStartDate
        {
            get { return newStartDate; }
            set
            {
                newStartDate = value;
            }
        }

        public DateTime NewEndDate
        {
            get { return newEndDate; }
            set
            {
                newEndDate = value;
            }
        }

        public string NewDateAvailable
        {
            get { return newDateAvailable; }
            set
            {
                newDateAvailable = value;
            }
        }

        public OwnerBookingMoveRequestsDTO SelectedBookingMoveRequest
        {
            get;
            set;
        }

        public List<ReservationReschedulingRequest> ReservationReschedulingRequests
        {
            get;
            set;
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

        public List<Reservation> Reservations
        {
            get;
            set;
        }

        public List<Reservation> AllReservations
        {
            get;
            set;
        }

        public List<BusyReservation> BusyReservations
        {
            get;
            set;
        }

        public OwnerManageBookingMoveRequests(string owner)
        {
            InitializeComponent();
            Owner = owner;
            DataContext = this;
            reservationReschedulingRequestRepository = new ReservationReschedulingRequestRepository();
            reservationRepository = new ReservationRepository();
            ReservationReschedulingRequests = new List<ReservationReschedulingRequest>();
            AllReservationReschedulingRequests = new List<ReservationReschedulingRequest>();
            OwnerBookingMoveRequestsDTOs = new List<OwnerBookingMoveRequestsDTO>();
            Reservations = new List<Reservation>();
            AllReservations = new List<Reservation>();
            BusyReservations = new List<BusyReservation>();
            FindAllOwnerReservations();
            FindAllOwnerPendingRequests();
            ShowAllOwnerPendingRequests();
            dgBookingMoveRequests.ItemsSource = OwnerBookingMoveRequestsDTOs;
            buttonAcceptRequest.IsEnabled = false;
            buttonDeclineRequest.IsEnabled = false;
            SelectedBookingMoveRequest = null;
        }

        private void FindAllOwnerReservations()
        {
            AllReservations = reservationRepository.FindAllReservations();

            foreach (Reservation reservation1 in AllReservations)
            {
                Trace.WriteLine(reservation1.ReservationId);
            }

            Trace.WriteLine("");

            Reservations = new List<Reservation>(AllReservations); // reservationRepository.FindAllReservations(); // jako cudno zasto ovo kopira i gleda kao jedan objekat i menja ih zajedno, ovaj kod sto je komentarisan?

            foreach(Reservation temporaryReservation in Reservations.ToList())
            {
                if(temporaryReservation.Accommodation.OwnerUsername.Equals(Owner) == false)
                {
                    Reservations.Remove(temporaryReservation);
                }
            }

            foreach (Reservation reservation1 in AllReservations)
            {
                Trace.WriteLine(reservation1.ReservationId);
            }

            Trace.WriteLine("");
        }

        private void FindAllOwnerPendingRequests()
        {
            AllReservationReschedulingRequests = reservationReschedulingRequestRepository.FindAllReservationReschedulingRequests();

            ReservationReschedulingRequests = new List<ReservationReschedulingRequest>(AllReservationReschedulingRequests); // reservationReschedulingRequestRepository.FindAllReservationReschedulingRequests(); isto kao i iznad

            ReservationReschedulingRequests = reservationRepository.FindReservationsForReservationReschedulingRequests(ReservationReschedulingRequests, Reservations);

            foreach(ReservationReschedulingRequest temporaryReservationReschedulingRequest in ReservationReschedulingRequests.ToList())
            {
                if(temporaryReservationReschedulingRequest.Reservation.Accommodation.OwnerUsername.Equals(Owner) == false || temporaryReservationReschedulingRequest.Status.Equals("pending") == false)
                {
                    ReservationReschedulingRequests.Remove(temporaryReservationReschedulingRequest);
                }
            }
        }

        private void ShowAllOwnerPendingRequests()
        {
            foreach(ReservationReschedulingRequest temporaryReservationReschedulingRequest in ReservationReschedulingRequests)
            {
                foreach(Reservation temporaryReservation in Reservations)
                {
                    if(temporaryReservationReschedulingRequest.Reservation.ReservationId == temporaryReservation.ReservationId)
                    {
                        string newDateAvailable = "Available";
                        if (IsNewDateAvailable(temporaryReservationReschedulingRequest.Reservation.ReservationId, temporaryReservation.Accommodation.Id, temporaryReservationReschedulingRequest.NewStartDate, temporaryReservationReschedulingRequest.NewEndDate) == false)
                        {
                            newDateAvailable = "Busy";
                        }
                        OwnerBookingMoveRequestsDTO temporaryOwnerBookingMoveRequestsDTO = new OwnerBookingMoveRequestsDTO(temporaryReservation.ReservationId, temporaryReservation.GuestUsername, temporaryReservation.Accommodation.AccommodationName, temporaryReservation.StartDate, temporaryReservation.EndDate, temporaryReservationReschedulingRequest.NewStartDate, temporaryReservationReschedulingRequest.NewEndDate, newDateAvailable);
                        OwnerBookingMoveRequestsDTOs.Add(temporaryOwnerBookingMoveRequestsDTO);
                    }
                }
            }
        }

        private bool IsNewDateAvailable(int reservationId, int accommodationId, DateTime newStartDate, DateTime newEndDate)
        {
            List<Reservation> accommodationReservations = reservationRepository.FindAllByAccommodation(accommodationId);

            List<Reservation> reservationsToBusyReservations = new List<Reservation>();

            foreach (Reservation temporaryReservation in accommodationReservations)
            {
                if(temporaryReservation.ReservationId == reservationId)
                {
                    reservationsToBusyReservations.Add(temporaryReservation);
                }
                bool firstExpression = (DateTime.Compare(temporaryReservation.StartDate, newStartDate) < 0 && DateTime.Compare(temporaryReservation.EndDate, newStartDate) < 0);
                bool secondExpression = (DateTime.Compare(temporaryReservation.StartDate, newEndDate) > 0 && DateTime.Compare(temporaryReservation.EndDate, newEndDate) > 0);
                if ( (firstExpression || secondExpression) == true)
                {

                }
                else
                {
                    if (temporaryReservation.ReservationId != reservationId)
                    {
                        reservationsToBusyReservations.Add(temporaryReservation);
                    }
                }
            }

            BusyReservations.Add(new BusyReservation(reservationId, reservationsToBusyReservations));

            if(reservationsToBusyReservations.Count != 1) // 1 jer se ubacuje taj bas koji treba da se odobri ili ne
            {
                return false;
            }

            return true;
        }

        private void GoToAddNewAccommodation(object sender, RoutedEventArgs e)
        {
            AddNewAccommodation window = new AddNewAccommodation(Owner);
            window.Show();
        }

        private void GoToRateGuests(object sender, RoutedEventArgs e)
        {
            RateGuests window = new RateGuests(Owner);
            if (window.dgRateGuests.Items.Count > 0)
            {
                window.Show();
            }
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

        private void GoToShowGuestReviews(object sender, RoutedEventArgs e)
        {
            ShowGuestReviews window = new ShowGuestReviews(Owner);
            window.Show();
            Close();
        }

        private void GoToShowOwnerManageBookingMoveRequests(object sender, RoutedEventArgs e)
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(Owner);
            window.Show();
            Close();
        }

        void LoadingRowForDgBookingMoveRequests(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void AcceptRequest(object sender, RoutedEventArgs e)
        {
            foreach(Reservation reservation1 in AllReservations)
            {
                Trace.WriteLine(reservation1.ReservationId);
            }

            foreach (Reservation temporaryReservation in AllReservations.ToList<Reservation>())
            {
                if (temporaryReservation.ReservationId == SelectedBookingMoveRequest.ReservationId)
                {
                    if (SelectedBookingMoveRequest.NewDateAvailable.Equals("Available") == true)
                    {
                        Reservation reservation = new Reservation(temporaryReservation);
                        reservation.StartDate = SelectedBookingMoveRequest.NewStartDate;
                        reservation.EndDate = SelectedBookingMoveRequest.NewEndDate;
                        AllReservations.Remove(temporaryReservation);
                        AllReservations.Add(reservation);

                        foreach (ReservationReschedulingRequest temporaryReservationReschedulingRequest in AllReservationReschedulingRequests.ToList())
                        {
                            if (temporaryReservationReschedulingRequest.Reservation.ReservationId == SelectedBookingMoveRequest.ReservationId)
                            {
                                ReservationReschedulingRequest reservationReschedulingRequest = new ReservationReschedulingRequest(temporaryReservationReschedulingRequest);
                                reservationReschedulingRequest.Status = "accepted";
                                AllReservationReschedulingRequests.Remove(temporaryReservationReschedulingRequest);
                                AllReservationReschedulingRequests.Add(reservationReschedulingRequest);
                                OwnerBookingMoveRequestsDTOs.Remove(SelectedBookingMoveRequest);
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach(BusyReservation temporaryBusyReservation in BusyReservations.ToList())
                        {
                            if(temporaryBusyReservation.ReservationId == SelectedBookingMoveRequest.ReservationId)
                            {
                                foreach(Reservation temporaryReservationToDelete in temporaryBusyReservation.ReservationsToDelete.ToList())
                                {
                                    if (temporaryReservationToDelete.ReservationId == SelectedBookingMoveRequest.ReservationId)
                                    {
                                        Reservation reservation = new Reservation(temporaryReservationToDelete);
                                        reservation.StartDate = SelectedBookingMoveRequest.NewStartDate;
                                        reservation.EndDate = SelectedBookingMoveRequest.NewEndDate;
                                        AllReservations.Remove(temporaryReservationToDelete);
                                        AllReservations.Add(reservation);
                                    }
                                    else
                                    {
                                        AllReservations.Remove(temporaryReservationToDelete);
                                    }
                                }
                            }
                        }

                        foreach (ReservationReschedulingRequest temporaryReservationReschedulingRequest in AllReservationReschedulingRequests.ToList())
                        {
                            if (temporaryReservationReschedulingRequest.Reservation.ReservationId == SelectedBookingMoveRequest.ReservationId)
                            {
                                ReservationReschedulingRequest reservationReschedulingRequest = new ReservationReschedulingRequest(temporaryReservationReschedulingRequest);
                                reservationReschedulingRequest.Status = "accepted";
                                AllReservationReschedulingRequests.Remove(temporaryReservationReschedulingRequest);
                                AllReservationReschedulingRequests.Add(reservationReschedulingRequest);
                                OwnerBookingMoveRequestsDTOs.Remove(SelectedBookingMoveRequest);
                                break;
                            }
                        }
                    }
                }
            }

            dgBookingMoveRequests.Items.Refresh();
            reservationRepository.UpdateReservations(AllReservations);
            reservationReschedulingRequestRepository.UpdateReservationReschedulingRequest(AllReservationReschedulingRequests);
        }

        private void DeclineRequest(object sender, RoutedEventArgs e)
        {
            ReservationReschedulingRequest reservationReschedulingRequestToReject = new ReservationReschedulingRequest();

            foreach(ReservationReschedulingRequest temporaryReservationReschedulingRequest in ReservationReschedulingRequests)
            {
                if(temporaryReservationReschedulingRequest.Reservation.ReservationId == SelectedBookingMoveRequest.ReservationId)
                {
                    reservationReschedulingRequestToReject = temporaryReservationReschedulingRequest;
                    break;
                }
            }

            DeclineBookingMoveRequest window = new DeclineBookingMoveRequest(this, Owner, reservationReschedulingRequestToReject, reservationReschedulingRequestRepository.FindAllReservationReschedulingRequests());

            window.Show();
        }

        private void AcceptRequestButtonEnable(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedBookingMoveRequest == null)
            {
                buttonAcceptRequest.IsEnabled = false;
                buttonDeclineRequest.IsEnabled = false;
            }
            else
            {
                buttonAcceptRequest.IsEnabled = true;
                buttonDeclineRequest.IsEnabled = true;
            }
        }
    }
}
