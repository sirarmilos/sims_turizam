using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Service
{
    public class ReviewService
    {
        private readonly UserService userService;

        private readonly ReviewRepository reviewRepository;

        private readonly ReservationRepository reservationRepository;

        private readonly RateGuestRepository rateGuestRepository;

        public List<Review> AllReviews
        {
            get;
            set;
        }

        public List<Reservation> AllReservations
        {
            get;
            set;
        }

        public List<Review> OwnerReviews
        {
            get;
            set;
        }

        public List<RateGuest> AllRateGuests
        {
            get;
            set;
        }

        public List<RateGuest> OwnerRateGuests
        {
            get;
            set;
        }

        public List<Reservation> Guest1Reservations
        {
            get;
            set;
        }

        public List<Review> Guest1Reviews
        {
            get;
            set;
        }

        private string owner;
        private string guest1;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public ReviewService(string username)
        {
            Owner = username;
            Guest1 = username;

            userService = new UserService();

            reviewRepository = new ReviewRepository();
            reservationRepository = new ReservationRepository();
            rateGuestRepository = new RateGuestRepository();

            ListInitialization();
        }

        private void ListInitialization()
        {
            AllReviews = new List<Review>();
            AllReservations = new List<Reservation>();
            OwnerReviews = new List<Review>();
            AllRateGuests = new List<RateGuest>();
            OwnerRateGuests = new List<RateGuest>();
            Guest1Reservations = new List<Reservation>();
            Guest1Reviews = new List<Review>();
        }

        public List<CreateReviewDTO> FindAllReviewsForRate()
        {
            AllReservations = reservationRepository.FindAllReservations();

            FindGuest1Reservations();

            AllReviews = reviewRepository.FindAll();

            FindReservationsForCreateReview();

            FindGuest1Reviews();

            return FindCreateReviewDTOs();
        }

        public void FindGuest1Reservations()
        {
            Guest1Reservations = AllReservations.ToList().FindAll(x => x.GuestUsername.Equals(Guest1) == true);
        }

        public void FindGuest1Reviews()
        {
            Guest1Reviews = AllReviews.ToList().FindAll(x => x.Reservation.GuestUsername.Equals(Guest1) == true);
        }

        public void FindReservationsForCreateReview()
        {
            foreach (Review temporaryReview in AllReviews.ToList())
            {
                temporaryReview.Reservation = AllReservations.ToList().Find(x => x.ReservationId == temporaryReview.Reservation.ReservationId);
            }
        }

        public List<CreateReviewDTO> FindCreateReviewDTOs()
        {
            List<CreateReviewDTO> createReviewDTOs = new List<CreateReviewDTO>();

            foreach (Reservation temporaryReservation in Guest1Reservations.ToList())
            {
                Review temporaryReview = Guest1Reviews.Find(x => x.Reservation.ReservationId == temporaryReservation.ReservationId);

                if(temporaryReview == null)
                {
                    CreateReviewDTO createReviewDTO = IsValidToAdd(temporaryReservation, temporaryReview);
                    if (createReviewDTO != null)
                    {
                        createReviewDTOs.Add(createReviewDTO);
                    }
                }
            }

            return createReviewDTOs;
        }

        public CreateReviewDTO IsValidToAdd(Reservation temporaryReservation, Review temporaryReview)
        {
            int days = DateTime.Now.Subtract(temporaryReservation.EndDate).Days;

            if (days <= 5 && days >= 0)
            {
                string deadline = "This is the last day to rate an accommodation.";

                if (5 - days > 0)
                {
                    deadline = "You have " + (5 - days) + " more days to rate the accommodation.";
                }

                return new CreateReviewDTO(temporaryReservation.ReservationId, temporaryReservation.Accommodation.AccommodationName, deadline);
            }

            return null;
        }

        public void SaveNewReview(SaveNewCreateReviewDTO saveNewCreateReviewDTO)
        {
            Review review = new Review(FindReservationByReservationId(saveNewCreateReviewDTO.ReservationId), saveNewCreateReviewDTO);

            List<Review> allReviews = reviewRepository.FindAll(); 

            allReviews.Add(review);

            reviewRepository.Save(allReviews); // todo: razmisliti da li je bolje cela ova logika da bude u repozitorijumu, a ne u servisu
        }

        private Reservation FindReservationByReservationId(int reservationId)
        {
            List<Reservation> allReservations = reservationRepository.FindAllReservations();
            return allReservations.Find(x => x.ReservationId == reservationId);
        }

        public List<ShowGuestReviewsDTO> FindAllReviews()
        {
            AllReviews = reviewRepository.FindAllReviews();

            AllReservations = reservationRepository.FindAllReservations();

            FindReservationsForReviews();

            FindOwnerReviews();

            AllRateGuests = rateGuestRepository.FindAllRateGuests();

            FindReservationsForRateGuestsReview();

            FindOwnerRateGuests();

            return FindShowGuestReviewsDTOs();
        }

        public void FindReservationsForReviews()
        {
            foreach (Review temporaryReview in AllReviews.ToList())
            {
                temporaryReview.Reservation = AllReservations.ToList().Find(x => x.ReservationId == temporaryReview.Reservation.ReservationId);
            }
        }

        public void FindOwnerReviews()
        {
            OwnerReviews = AllReviews.ToList().FindAll(x => x.Reservation.Accommodation.OwnerUsername.Equals(Owner) == true);
        }

        public void FindReservationsForRateGuestsReview()
        {
            foreach (RateGuest temporaryRateGuest in AllRateGuests.ToList())
            {
                temporaryRateGuest.Reservation = AllReservations.ToList().Find(x => x.ReservationId == temporaryRateGuest.Reservation.ReservationId);
            }
        }

        public void FindOwnerRateGuests()
        {
            OwnerRateGuests = AllRateGuests.ToList().FindAll(x => x.Reservation.Accommodation.OwnerUsername.Equals(Owner) == true);
        }

        public List<ShowGuestReviewsDTO> FindShowGuestReviewsDTOs()
        {
            List<ShowGuestReviewsDTO> showGuestReviewsDTOs = new List<ShowGuestReviewsDTO>();

            foreach (RateGuest temporaryRateGuest in OwnerRateGuests.ToList())
            {
                Review temporaryReview = OwnerReviews.Find(x => x.Reservation.ReservationId == temporaryRateGuest.Reservation.ReservationId);

                if(temporaryReview != null)
                {
                    ShowGuestReviewsDTO showGuestReviewsDTO = new ShowGuestReviewsDTO(temporaryReview);
                    showGuestReviewsDTOs.Add(showGuestReviewsDTO);
                }
            }

            return showGuestReviewsDTOs;
        }

        public void CheckSuperOwner(int reservationId)
        {
            AllReviews = reviewRepository.FindAllReviews();

            AllReservations = reservationRepository.FindAllReservations();

            FindReservationsForReviews();

            FindOwnerUsernameByReservationId(reservationId);

            FindOwnerReviews();

            if(OwnerReviews.Count >= 50 && FindAverageGuestReviews() > new Decimal(4.5))
            {
                userService.UpdateUsers(Owner, "super");
            }
            else if(IsSuperOwner(reservationId) == true) // ako je bio super owner, a ne treba vise da bude
            {
                userService.UpdateUsers(Owner, "no_super");
            }
        }

        public void FindOwnerUsernameByReservationId(int reservationId)
        {
            Owner = AllReservations.Find(x => x.ReservationId == reservationId).Accommodation.OwnerUsername;
        }

        private Decimal FindAverageGuestReviews()
        {
            return (decimal)OwnerReviews.Sum(x => (x.Cleanliness + x.Staff + x.Comfort + x.ValueForMoney) / new Decimal(4.0)) / OwnerReviews.Count;
        }

        private bool IsSuperOwner(int reservationId)
        {
            return userService.FindSuperTypeByOwnerName(Owner).Equals("super");
        }
    }
}
