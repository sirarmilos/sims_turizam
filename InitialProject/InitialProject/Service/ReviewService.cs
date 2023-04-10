using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        public ReviewService(string owner)
        {
            Owner = owner;
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
