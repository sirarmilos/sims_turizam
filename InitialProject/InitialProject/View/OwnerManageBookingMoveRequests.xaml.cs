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

        private readonly AccommodationRepository accommodationRepository;

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
            accommodationRepository = new AccommodationRepository();

            AllReservationReschedulingRequests = new List<ReservationReschedulingRequest>();
            OwnerBookingMoveRequestsDTOs = new List<OwnerBookingMoveRequestsDTO>();
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

            Reservations = new List<Reservation>();

            foreach (Reservation temporaryReservation in AllReservations.ToList())
            {
                if (temporaryReservation.Accommodation.OwnerUsername.Equals(Owner) == true)
                {
                    Reservations.Add(temporaryReservation);
                }
            }
        }

        private void FindAllOwnerPendingRequests()
        {
            AllReservationReschedulingRequests = reservationReschedulingRequestRepository.FindAllReservationReschedulingRequests();

            ReservationReschedulingRequests = new List<ReservationReschedulingRequest>();

            FindReservationsForReservationReschedulingRequests();
        }

        private void FindReservationsForReservationReschedulingRequests()
        {
            List<Accommodation> accommodations = new List<Accommodation>();

            accommodations = accommodationRepository.FindAllAccommodations();

            foreach (ReservationReschedulingRequest temporaryReservationReschedulingRequest in AllReservationReschedulingRequests.ToList())
            {
                foreach (Reservation temporaryReservation in Reservations.ToList())
                {
                    if (temporaryReservationReschedulingRequest.Reservation.ReservationId == temporaryReservation.ReservationId)
                    {
                        temporaryReservationReschedulingRequest.Reservation = temporaryReservation;
                        foreach (Accommodation temporaryAccommodation in accommodations.ToList())
                        {
                            if (temporaryReservationReschedulingRequest.Reservation.Accommodation.Id == temporaryAccommodation.Id && temporaryReservationReschedulingRequest.Reservation.Accommodation.OwnerUsername.Equals(Owner) == true && temporaryReservationReschedulingRequest.Status.Equals("pending") == true)
                            {
                                ReservationReschedulingRequests.Add(temporaryReservationReschedulingRequest);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void ShowAllOwnerPendingRequests()
        {
            foreach(ReservationReschedulingRequest temporaryReservationReschedulingRequest in ReservationReschedulingRequests.ToList())
            {
                foreach(Reservation temporaryReservation in Reservations.ToList())
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
            List<Reservation> accommodationReservations = FindAllReservationsByAccommodationId(accommodationId);

            List<Reservation> reservationsToBusyReservations = new List<Reservation>();

            foreach (Reservation temporaryReservation in accommodationReservations.ToList())
            {
                if(temporaryReservation.ReservationId == reservationId)
                {
                    reservationsToBusyReservations.Add(temporaryReservation);
                }

                bool firstExpression = (DateTime.Compare(temporaryReservation.StartDate, newStartDate) < 0 && DateTime.Compare(temporaryReservation.EndDate, newStartDate) < 0);
                bool secondExpression = (DateTime.Compare(temporaryReservation.StartDate, newEndDate) > 0 && DateTime.Compare(temporaryReservation.EndDate, newEndDate) > 0);

                if ( (firstExpression || secondExpression) == false)
                {
                    if (temporaryReservation.ReservationId != reservationId)
                    {
                        reservationsToBusyReservations.Add(temporaryReservation);
                    }
                }
            }

            BusyReservations.Add(new BusyReservation(reservationId, reservationsToBusyReservations));

            if(reservationsToBusyReservations.Count > 1) // 1 jer se ubacuje taj bas koji treba da se odobri ili ne
            {
                return false;
            }

            return true;
        }

        private List<Reservation> FindAllReservationsByAccommodationId(int accommodationId)
        {
            List<Reservation> accommodationReservations = new List<Reservation>();

            foreach (Reservation reservation in Reservations.ToList())
            {
                if (reservation.Accommodation.Id == accommodationId)
                {
                    accommodationReservations.Add(reservation);
                }
            }

            return accommodationReservations;
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
            foreach (Reservation temporaryReservation in AllReservations.ToList<Reservation>())
            {
                if (temporaryReservation.ReservationId == SelectedBookingMoveRequest.ReservationId)
                {
                    if (SelectedBookingMoveRequest.NewDateAvailable.Equals("Available") == true)
                    {
                        temporaryReservation.StartDate = SelectedBookingMoveRequest.NewStartDate;
                        temporaryReservation.EndDate = SelectedBookingMoveRequest.NewEndDate;

                        foreach (ReservationReschedulingRequest temporaryReservationReschedulingRequest in AllReservationReschedulingRequests.ToList())
                        {
                            if (temporaryReservationReschedulingRequest.Reservation.ReservationId == SelectedBookingMoveRequest.ReservationId)
                            {
                                temporaryReservationReschedulingRequest.Status = "accepted";
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
                                        AllReservations.Add(reservation);
                                    }
                                    
                                    AllReservations.Remove(temporaryReservationToDelete);

                                    foreach (ReservationReschedulingRequest temporaryReservationReschedulingRequest in AllReservationReschedulingRequests.ToList())
                                    {
                                        if (temporaryReservationReschedulingRequest.Reservation.ReservationId == SelectedBookingMoveRequest.ReservationId)
                                        {
                                            temporaryReservationReschedulingRequest.Status = "accepted";
                                            OwnerBookingMoveRequestsDTOs.Remove(SelectedBookingMoveRequest);
                                        }
                                        else if(temporaryReservationReschedulingRequest.Reservation.ReservationId == temporaryReservationToDelete.ReservationId)
                                        {
                                            AllReservationReschedulingRequests.Remove(temporaryReservationReschedulingRequest);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            reservationRepository.UpdateReservations(AllReservations);
            reservationReschedulingRequestRepository.UpdateReservationReschedulingRequest(AllReservationReschedulingRequests);

            AllReservationReschedulingRequests = new List<ReservationReschedulingRequest>();
            OwnerBookingMoveRequestsDTOs = new List<OwnerBookingMoveRequestsDTO>();
            AllReservations = new List<Reservation>();
            BusyReservations = new List<BusyReservation>();

            FindAllOwnerReservations();
            FindAllOwnerPendingRequests();
            ShowAllOwnerPendingRequests();

            dgBookingMoveRequests.ItemsSource = OwnerBookingMoveRequestsDTOs;
        }

        private void DeclineRequest(object sender, RoutedEventArgs e)
        {
            DeclineBookingMoveRequest window = new DeclineBookingMoveRequest(OwnerBookingMoveRequestsDTOs, dgBookingMoveRequests, Owner, SelectedBookingMoveRequest, AllReservationReschedulingRequests);

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
