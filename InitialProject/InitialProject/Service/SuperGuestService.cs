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
using InitialProject.Injector;

namespace InitialProject.Service
{
    public class SuperGuestService
    {
        private readonly ISuperGuestRepository superGuestRepository;

        private ReservationService reservationService;

        private UserService userService;

        private string guest1;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public SuperGuestService()
        {
            superGuestRepository = Injector.Injector.CreateInstance<ISuperGuestRepository>();
        }

        public SuperGuestService(string guest1)
        {
            Guest1 = guest1;
            superGuestRepository = Injector.Injector.CreateInstance<ISuperGuestRepository>();

        }


        public void DecreaseOneBonusPoint()
        {
            SuperGuest superGuest = superGuestRepository.FindLatestByGuest1(Guest1);

            if (superGuest.NumberOfBonusPoints != 0)
            {
                superGuest.NumberOfBonusPoints -= 1;

                superGuestRepository.Update(superGuest);
            }
        }

        public SuperGuest FindSuperGuestByGuest1Username()
        {
            return superGuestRepository.FindLatestByGuest1(Guest1);
        }

        public List<SuperGuest> FindAllLatestSuperGuests()
        {
            return superGuestRepository.FindAllLatestSuperGuests();
        }

        public void MakeUserSuperGuest(string guest1Username)
        {
            SuperGuest superGuest = new SuperGuest(guest1Username, DateTime.Now, 5);

            superGuestRepository.Add(superGuest);
        }

        public bool MakeEligibleUserSuperGuest(string guest1Username) 
        {
            userService = new UserService();
            reservationService = new ReservationService(guest1Username);

            if (reservationService.FindNumberOfReservationsInPastYear() >= 10)
            {
                MakeUserSuperGuest(guest1Username);

                userService.MakeUserSuperGuest(guest1Username);

                return true;
            }

            return false;
        }
    }
}
