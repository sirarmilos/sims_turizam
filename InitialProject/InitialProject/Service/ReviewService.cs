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

        private  ReservationService reservationService;

        private readonly RateGuestsService rateGuestsService;

        private readonly ReviewRepository reviewRepository;

        private ReservationRepository reservationRepository;

        private readonly RateGuestRepository rateGuestRepository;

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
            reservationService = new ReservationService();
            rateGuestsService = new RateGuestsService(Owner);

            reviewRepository = new ReviewRepository();
            reservationRepository = new ReservationRepository();
            rateGuestRepository = new RateGuestRepository();
        }

        public List<CreateReviewDTO> FindAllReviewsForRate()
        {
            List<Reservation> reservations = FindGuest1Reservations();

            List<Review> reviews = FindGuest1Reviews();

            return FindCreateReviewDTOs(reservations, reviews);
        }

        public List<Reservation> FindGuest1Reservations()
        {
            return reservationRepository.FindGuest1Reservations(Guest1);
        }

        public List<Review> FindGuest1Reviews()
        {
            return reviewRepository.FindGuest1Reviews(Guest1);
        }

        public List<CreateReviewDTO> FindCreateReviewDTOs(List<Reservation> guest1reservations, List<Review> guest1Reviews)
        {
            List<CreateReviewDTO> createReviewDTOs = new List<CreateReviewDTO>();

            foreach (Reservation temporaryReservation in guest1reservations.ToList())
            {
                Review temporaryReview = guest1Reviews.Find(x => x.Reservation.ReservationId == temporaryReservation.ReservationId);

                if(temporaryReview == null)
                {
                    CreateReviewDTO createReviewDTO = IsValidToAdd(temporaryReservation);
                    if (createReviewDTO != null)
                    {
                        createReviewDTOs.Add(createReviewDTO);
                    }
                }
            }

            return createReviewDTOs;
        }

        public CreateReviewDTO IsValidToAdd(Reservation temporaryReservation)
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
            Review review = new Review(FindReservationById(saveNewCreateReviewDTO.ReservationId), saveNewCreateReviewDTO);

            reviewRepository.Save(review); 
        }

        private Reservation FindReservationById(int reservationId)
        {
            return reservationRepository.FindById(reservationId); // U konstruktorima servisa treba da se inicijalizuju polja, dok u konstruktorima repozitorijuma ne treba inicijalizovati polja
        }






        public List<ShowGuestReviewsDTO> FindAllReviews()
        {
            return FindShowGuestReviewsDTOs(rateGuestsService.FindOwnerRateGuests(Owner));
        }

        public List<ShowGuestReviewsDTO> FindShowGuestReviewsDTOs(List<RateGuest> ownerRateGuests)
        {
            List<ShowGuestReviewsDTO> showGuestReviewsDTOs = new List<ShowGuestReviewsDTO>();

            foreach (RateGuest temporaryRateGuest in ownerRateGuests.ToList())
            {
                Review temporaryReview = reviewRepository.FindOwnerReviewByReservationId(Owner, temporaryRateGuest.Reservation.ReservationId);

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
            string ownerUsername = reservationService.FindOwnerByReservationId(reservationId);

            List<Review> ownerReviews = reviewRepository.FindReviewsByOwnerUsername(ownerUsername);

            if(ownerReviews.Count >= 50 && FindAverageGuestReviews(ownerReviews) > new Decimal(4.5))
            {
                userService.UpdateUsers(ownerUsername, "super");
            }
            else if(IsSuperOwner(ownerUsername) == true) // ako je bio super owner, a ne treba vise da bude
            {
                userService.UpdateUsers(ownerUsername, "no_super");
            }
        }

        private Decimal FindAverageGuestReviews(List<Review> ownerReviews)
        {
            return (decimal)ownerReviews.Sum(x => (x.Cleanliness + x.Staff + x.Comfort + x.ValueForMoney) / new Decimal(4.0)) / ownerReviews.Count;
        }

        private bool IsSuperOwner(string ownerUsername)
        {
            return userService.FindSuperTypeByOwnerName(ownerUsername).Equals("super");
        }
    }
}
