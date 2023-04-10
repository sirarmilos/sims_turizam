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
        }

        public List<CreateReviewDTO> FindAllReviewsForRate()
        {
            List<Reservation> allReservations = reservationRepository.FindAllReservations();

            List<Reservation> guest1Reservations = FindGuest1Reservations(allReservations);

            List<Review> allReviews = reviewRepository.FindAll(); 

            allReviews = FindReservationsForCreateReview(allReviews, allReservations);

            List<Review> guest1Reviews = FindGuest1Reviews(allReviews);

            return FindCreateReviewDTOs(guest1Reservations, guest1Reviews);
        }

        public List<Reservation> FindGuest1Reservations(List<Reservation> allReservations)
        {
            return allReservations.ToList().FindAll(x => x.GuestUsername.Equals(Guest1) == true);
        }

        public List<Review> FindGuest1Reviews(List<Review> allReviews)
        {
            return allReviews.ToList().FindAll(x => x.Reservation.GuestUsername.Equals(Guest1) == true);
        }

        public List<Review> FindReservationsForCreateReview(List<Review> allReviews, List<Reservation> allReservations)
        {
            foreach (Review temporaryReview in allReviews.ToList())
            {
                temporaryReview.Reservation = allReservations.ToList().Find(x => x.ReservationId == temporaryReview.Reservation.ReservationId);
            }

            return allReviews;
        }

        public List<CreateReviewDTO> FindCreateReviewDTOs(List<Reservation> guest1Reservations, List<Review> guest1Reviews)
        {
            List<CreateReviewDTO> createReviewDTOs = new List<CreateReviewDTO>();

            foreach (Reservation temporaryReservation in guest1Reservations.ToList())
            {
                Review temporaryReview = guest1Reviews.Find(x => x.Reservation.ReservationId == temporaryReservation.ReservationId);

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
    }
}
