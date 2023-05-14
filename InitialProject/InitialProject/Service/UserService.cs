using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Service
{
    public class UserService
    {
        private readonly IUserRepository userRepository;

        private RateGuestsService rateGuestsService;

        private AccommodationService accommodationService;

        private CanceledReservationService canceledReservationService;

        public UserService()
        {
            userRepository = Injector.Injector.CreateInstance<IUserRepository>();
            //userRepository = new UserRepository();  

             //rateGuestsService = new RateGuestsService(Owner);
        }

        public void Update(string owner, string superType)
        {
            userRepository.Update(owner, superType);
        }

        public string FindSuperTypeByOwnerName(string ownerName)
        {
            return userRepository.FindSuperTypeByOwnerName(ownerName);
        }

        public bool IsUsernameExist(string username)
        {
            return userRepository.IsUserExist(username);
        }

        public bool IsPasswordCorrect(string username, string password)
        {
            return userRepository.IsPasswordCorrect(username, password);
        }

        public string FindTypeByUsername(string username)
        {
            return userRepository.FindTypeByUsername(username);
        }

        public int FindNumberOfUnratedGuests(string ownerUsername)
        {
            rateGuestsService = new RateGuestsService(ownerUsername);
            return rateGuestsService.FindNumberOfUnratedGuests(ownerUsername);
        }

        public void CheckRecentlyRenovatedAccommodation()
        {
            accommodationService = new AccommodationService();

            accommodationService.CheckRecentlyRenovated();
        }

        public List<string> FindUnreadCancelledReservations(string ownerUsername)
        {
            canceledReservationService = new CanceledReservationService();

            List<CancelledReservationsNotificationDTO> unreadCancelledReservations = new List<CancelledReservationsNotificationDTO>();

            unreadCancelledReservations = canceledReservationService.FindOwnerUnreadCancelledReservations(ownerUsername);

            List<string> unreadCancelledReservationsString = new List<string>();

            foreach(CancelledReservationsNotificationDTO temporaryCancelledReservationsNotificationDTO in unreadCancelledReservations.ToList())
            {
                unreadCancelledReservationsString.Add(temporaryCancelledReservationsNotificationDTO.AccommodationName + ": " + temporaryCancelledReservationsNotificationDTO.ReservationStartDate.ToShortDateString() + " - " + temporaryCancelledReservationsNotificationDTO.ReservationEndDate.ToShortDateString());
            }

            if(unreadCancelledReservationsString.Count == 0)
            {
                unreadCancelledReservationsString.Add("There are currently no new booking cancellations.");
            }

            return unreadCancelledReservationsString;
        }

        public void SaveViewedCancelledReservation(CancelledReservationsNotificationDTO cancelledReservationsNotificationDTO)
        {
            canceledReservationService = new CanceledReservationService();

            canceledReservationService.SaveViewed(cancelledReservationsNotificationDTO);
        }

        public User FindByUsername(string username)
        {
            return userRepository.FindByUsername(username);
        }


    }
}
