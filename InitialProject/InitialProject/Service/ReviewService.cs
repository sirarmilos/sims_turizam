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
using InitialProject.IRepository;
using InitialProject.Serializer;

namespace InitialProject.Service
{
    public class ReviewService
    {
        private readonly UserService userService;

        private ReservationService reservationService;

        private readonly RateGuestsService rateGuestsService;

        private readonly RenovationRecommendationService renovationRecommendationService;

        private readonly IReviewRepository reviewRepository;

        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;

        private readonly AccommodationService accommodationService;

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
            reservationReschedulingRequestService = new ReservationReschedulingRequestService();
            renovationRecommendationService = new RenovationRecommendationService();
            accommodationService = new AccommodationService();


            reviewRepository = Injector.Injector.CreateInstance<IReviewRepository>();
            //reviewRepository = new ReviewRepository();
        }

        public List<CreateReviewDTO> FindAllReviewsToRate()
        {
            List<Reservation> reservations = reservationService.FindGuest1Reservations(Guest1);

            return FindCreateReviewDTOs(reservations);
        }

        public List<CreateReviewDTO> FindCreateReviewDTOs(List<Reservation> guest1reservations)
        {
            List<CreateReviewDTO> createReviewDTOs = new List<CreateReviewDTO>();

            foreach (Reservation temporaryReservation in guest1reservations.ToList())
            {
                Review temporaryReview = reviewRepository.FindGuest1ReviewByReservationId(Guest1, temporaryReservation.ReservationId);

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
            Review review = new Review(reservationService.FindById(saveNewCreateReviewDTO.ReservationId), saveNewCreateReviewDTO);

            RenovationRecommendation renovationRecommendation = 
                new RenovationRecommendation(reservationService.FindById(saveNewCreateReviewDTO.ReservationId), saveNewCreateReviewDTO);

            renovationRecommendationService.SaveRenovationRecommendation(renovationRecommendation);
            reviewRepository.Add(review); // mozda Save
        }

        public bool Guest1HasNotification()
        {
            return reservationReschedulingRequestService.Guest1HasNotification(Guest1);
        }


        public List<ShowOwnerReviewsDTO> FindAllOwnerReviews()
        {
            return FindShowOwnerReviewsDTOs(reviewRepository.FindReviewsByGuest1Username(Guest1));
        }

        public List<ShowOwnerReviewsDTO> FindShowOwnerReviewsDTOs(List<Review> guest1Reviews)
        {
            List<ShowOwnerReviewsDTO> showOwnerReviewsDTOs = new List<ShowOwnerReviewsDTO>();

            foreach (Review temporaryReview in guest1Reviews.ToList())
            {
                RateGuest rateGuest = rateGuestsService.FindRateGuestByReservation(temporaryReview.Reservation.ReservationId);

                if (rateGuest != null)
                {
                    ShowOwnerReviewsDTO showOwnerReviewsDTO = new ShowOwnerReviewsDTO(rateGuest);
                    showOwnerReviewsDTOs.Add(showOwnerReviewsDTO);
                }
            }

            return showOwnerReviewsDTOs;
        }

        public bool IsSuperGuest(string guest1Username)
        {
            return userService.IsSuperGuest(guest1Username);
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

            List<Review> ownerReviews = reviewRepository.FindByOwnerUsername(ownerUsername);

            if(ownerReviews.Count >= 50 && FindAverageGuestReviews(ownerReviews) > new Decimal(4.5))
            {
                userService.Update(ownerUsername, "super");
            }
            else if(IsSuperOwner(ownerUsername) == true) // ako je bio super owner, a ne treba vise da bude
            {
                userService.Update(ownerUsername, "no_super");
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

        public int FindNumberOfUnratedGuests(string ownerUsername)
        {
            return rateGuestsService.FindNumberOfUnratedGuests(ownerUsername);
        }

        public void SaveViewedCancelledReservation(CancelledReservationsNotificationDTO cancelledReservationsNotificationDTO)
        {
            userService.SaveViewedCancelledReservation(cancelledReservationsNotificationDTO);
        }

        public List<string> FindUnreadCancelledReservations(string ownerUsername)
        {
            return userService.FindUnreadCancelledReservations(ownerUsername);
        }

        public List<OwnerPDFReportDTO> FindOwnerPDFReportDTOs(string ownerUsername)
        {
            List<OwnerPDFReportDTO> averageList = new List<OwnerPDFReportDTO>();

            List<Accommodation> ownerAccommodations = accommodationService.FindOwnerAccommodationsToPDFReport(ownerUsername);

            foreach (Accommodation temporaryAccommodation in ownerAccommodations.ToList())
            {
                averageList.Add(new OwnerPDFReportDTO(temporaryAccommodation));
            }

            List<ShowGuestReviewsDTO> ownerReviews = FindAllReviews(); // zbog onog uslova sa SIMS-a

            foreach(ShowGuestReviewsDTO temporaryShowGuestReviewsDTO in ownerReviews.ToList())
            {
                averageList.Where(x => x.AccommodationName.Equals(temporaryShowGuestReviewsDTO.AccommodationName) == true).SetValue(x => x.AverageCleanliness += temporaryShowGuestReviewsDTO.Cleanliness);
                averageList.Where(x => x.AccommodationName.Equals(temporaryShowGuestReviewsDTO.AccommodationName) == true).SetValue(x => x.AverageStaff += temporaryShowGuestReviewsDTO.Staff);
                averageList.Where(x => x.AccommodationName.Equals(temporaryShowGuestReviewsDTO.AccommodationName) == true).SetValue(x => x.AverageComfort += temporaryShowGuestReviewsDTO.Comfort);
                averageList.Where(x => x.AccommodationName.Equals(temporaryShowGuestReviewsDTO.AccommodationName) == true).SetValue(x => x.AverageValueForMoney += temporaryShowGuestReviewsDTO.ValueForMoney);
                averageList.Where(x => x.AccommodationName.Equals(temporaryShowGuestReviewsDTO.AccommodationName) == true).SetValue(x => x.NumberOfReviews += 1);
            }

            foreach(OwnerPDFReportDTO temporaryOwnerPDFReportDTO in averageList.ToList())
            {
                if(temporaryOwnerPDFReportDTO.NumberOfReviews > 0)
                {
                    temporaryOwnerPDFReportDTO.AverageAll = Math.Round((temporaryOwnerPDFReportDTO.AverageCleanliness + temporaryOwnerPDFReportDTO.AverageStaff + temporaryOwnerPDFReportDTO.AverageComfort + temporaryOwnerPDFReportDTO.AverageValueForMoney) / new Decimal(4.0) / temporaryOwnerPDFReportDTO.NumberOfReviews, 2);
                    temporaryOwnerPDFReportDTO.AverageCleanliness = Math.Round(temporaryOwnerPDFReportDTO.AverageCleanliness / temporaryOwnerPDFReportDTO.NumberOfReviews, 2);
                    temporaryOwnerPDFReportDTO.AverageStaff = Math.Round(temporaryOwnerPDFReportDTO.AverageStaff / temporaryOwnerPDFReportDTO.NumberOfReviews, 2);
                    temporaryOwnerPDFReportDTO.AverageComfort = Math.Round(temporaryOwnerPDFReportDTO.AverageComfort / temporaryOwnerPDFReportDTO.NumberOfReviews, 2);
                    temporaryOwnerPDFReportDTO.AverageValueForMoney = Math.Round(temporaryOwnerPDFReportDTO.AverageValueForMoney / temporaryOwnerPDFReportDTO.NumberOfReviews, 2);
                }
            }

            return averageList;
        }

        /* refaktorisan kod, proveri da li je dobro
        
        public List<OwnerPDFReportDTO> FindOwnerPDFReportDTOs(string ownerUsername)
        {
            List<Accommodation> ownerAccommodations = accommodationService.FindOwnerAccommodationsToPDFReport(ownerUsername);

            List<ShowGuestReviewsDTO> ownerReviews = FindAllReviews(); // zbog onog uslova sa SIMS-a

            return CalculateAverageList(SumWithOwnerReviews(ownerReviews, InitializationWithOwnerAccommodations(ownerAccommodations)));
        }

        public List<OwnerPDFReportDTO> InitializationWithOwnerAccommodations(List<Accommodation> ownerAccommodations)
        {
            List<OwnerPDFReportDTO> averageList = new List<OwnerPDFReportDTO>();

            foreach (Accommodation temporaryAccommodation in ownerAccommodations.ToList())
            {
                averageList.Add(new OwnerPDFReportDTO(temporaryAccommodation));
            }

            return averageList;
        }

        public List<OwnerPDFReportDTO> SumWithOwnerReviews(List<ShowGuestReviewsDTO> ownerReviews, List<OwnerPDFReportDTO> averageList)
        {
            foreach (ShowGuestReviewsDTO temporaryShowGuestReviewsDTO in ownerReviews.ToList())
            {
                averageList.Where(x => x.AccommodationName.Equals(temporaryShowGuestReviewsDTO.AccommodationName) == true).SetValue(x => x.AverageCleanliness += temporaryShowGuestReviewsDTO.Cleanliness);
                averageList.Where(x => x.AccommodationName.Equals(temporaryShowGuestReviewsDTO.AccommodationName) == true).SetValue(x => x.AverageStaff += temporaryShowGuestReviewsDTO.Staff);
                averageList.Where(x => x.AccommodationName.Equals(temporaryShowGuestReviewsDTO.AccommodationName) == true).SetValue(x => x.AverageComfort += temporaryShowGuestReviewsDTO.Comfort);
                averageList.Where(x => x.AccommodationName.Equals(temporaryShowGuestReviewsDTO.AccommodationName) == true).SetValue(x => x.AverageValueForMoney += temporaryShowGuestReviewsDTO.ValueForMoney);
                averageList.Where(x => x.AccommodationName.Equals(temporaryShowGuestReviewsDTO.AccommodationName) == true).SetValue(x => x.NumberOfReviews += 1);
            }

            return averageList;
        }

        public List<OwnerPDFReportDTO> CalculateAverageList(List<OwnerPDFReportDTO> averageList)
        {
            foreach (OwnerPDFReportDTO temporaryOwnerPDFReportDTO in averageList.ToList())
            {
                if (temporaryOwnerPDFReportDTO.NumberOfReviews > 0)
                {
                    temporaryOwnerPDFReportDTO.AverageAll = Math.Round((temporaryOwnerPDFReportDTO.AverageCleanliness + temporaryOwnerPDFReportDTO.AverageStaff + temporaryOwnerPDFReportDTO.AverageComfort + temporaryOwnerPDFReportDTO.AverageValueForMoney) / new Decimal(4.0) / temporaryOwnerPDFReportDTO.NumberOfReviews, 2);
                    temporaryOwnerPDFReportDTO.AverageCleanliness = Math.Round(temporaryOwnerPDFReportDTO.AverageCleanliness / temporaryOwnerPDFReportDTO.NumberOfReviews, 2);
                    temporaryOwnerPDFReportDTO.AverageStaff = Math.Round(temporaryOwnerPDFReportDTO.AverageStaff / temporaryOwnerPDFReportDTO.NumberOfReviews, 2);
                    temporaryOwnerPDFReportDTO.AverageComfort = Math.Round(temporaryOwnerPDFReportDTO.AverageComfort / temporaryOwnerPDFReportDTO.NumberOfReviews, 2);
                    temporaryOwnerPDFReportDTO.AverageValueForMoney = Math.Round(temporaryOwnerPDFReportDTO.AverageValueForMoney / temporaryOwnerPDFReportDTO.NumberOfReviews, 2);
                }
            }

            return averageList;
        }

        */

        public string FindSuperTypeByOwnerName(string username)
        {
            return userService.FindSuperTypeByOwnerName(username);
        }
    }
}