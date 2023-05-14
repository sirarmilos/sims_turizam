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

        private SuperGuestService superGuestService;

        private AccommodationService accommodationService;

        private CanceledReservationService canceledReservationService;

        public UserService()
        {
            userRepository = Injector.Injector.CreateInstance<IUserRepository>();
            canceledReservationService = new CanceledReservationService();

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

        public List<CancelledReservationsNotificationDTO> FindUnreadCancelledReservations(string ownerUsername)
        {
            canceledReservationService = new CanceledReservationService();

            List<CancelledReservationsNotificationDTO> unreadCancelledReservations = new List<CancelledReservationsNotificationDTO>();

            unreadCancelledReservations = canceledReservationService.FindOwnerUnreadCancelledReservations(ownerUsername);

            return unreadCancelledReservations;
        }


        public void CheckUsersSuperGuestStatus()
        {

            superGuestService = new SuperGuestService();

            List<SuperGuest> superGuestsWithLatestStatus = superGuestService.FindAllLatestSuperGuests();
            List<User> allGuests1 = userRepository.FindAllGuests1();

            CheckPotentiallyExpiredSuperGuests(superGuestsWithLatestStatus);

            MakeEligibleUsersSuperGuest(allGuests1);

        }

        private void MakeEligibleUsersSuperGuest(List<User> allGuests1)
        {
            foreach (User temporaryUser in allGuests1)
            {
                if (!IsSuperGuest(temporaryUser.Username))
                    superGuestService.CheckIfUserEligibleForSuperGuest(temporaryUser.Username);
            }
        }

        private void CheckPotentiallyExpiredSuperGuests(List<SuperGuest> superGuestsWithLatestStatus)
        {
            foreach (SuperGuest tempoprarySuperGuest in superGuestsWithLatestStatus)
            {
                if (tempoprarySuperGuest.StartDate.AddDays(365) < DateTime.Now)
                {
                    MakeUserRegularUser(tempoprarySuperGuest.Guest1Username);
                }
            }
        }

        public void MakeUserRegularUser(string guest1Username)
        {
            userRepository.Update(guest1Username, "no_super");
        }


        public bool IsSuperGuest(string guest1Username)
        {
            return userRepository.IsSuperGuest(guest1Username);
        }

        public void MakeUserSuperGuest(string guest1Username)
        {
            userRepository.Update(guest1Username, "super");
        }

        public void MarkAsReadNotificationsCancelledReservations(List<CancelledReservationsNotificationDTO> unreadCancelledReservations)
        {
            canceledReservationService.MarkAsReadNotificationsCancelledReservations(unreadCancelledReservations);
        }

        public User FindByUsername(string username)
        {
            return userRepository.FindByUsername(username);
        }


    }
}
