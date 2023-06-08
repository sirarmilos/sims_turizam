using InitialProject.DTO;
using InitialProject.Injector;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Service
{
    public class RenovationService
    {
        private readonly IRenovationRepository renovationRepository;

        private readonly RateGuestsService rateGuestsService;

        private readonly CanceledRenovationService canceledRenovationService;

        private readonly AccommodationService accommodationService;

        private readonly ReservationService reservationService;

        private readonly UserService userService;

        private readonly CanceledReservationService canceledReservationService;

        private readonly ForumNotificationsToOwnerService forumNotificationsToOwnerService;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        public RenovationService(string username)
        {
            Owner = username;

            renovationRepository = Injector.Injector.CreateInstance<IRenovationRepository>();
            //renovationRepository = new RenovationRepository();

            rateGuestsService = new RateGuestsService(Owner);
            canceledRenovationService = new CanceledRenovationService();
            accommodationService = new AccommodationService();
            reservationService = new ReservationService(Owner);
            userService = new UserService();
            canceledReservationService = new CanceledReservationService();
            forumNotificationsToOwnerService = new ForumNotificationsToOwnerService();
        }

        public RenovationService()
        {
            renovationRepository = Injector.Injector.CreateInstance<IRenovationRepository>();
            //renovationRepository = new RenovationRepository();

            // accommodationService = new AccommodationService();
            canceledReservationService = new CanceledReservationService();
        }

        public List<ShowRenovationDTO> FindAllRenovations(string ownerUsername)
        {
            return FindShowRenovationDTOs(renovationRepository.FindByOwnerUsername(ownerUsername));
        }

        public List<ShowRenovationDTO> FindShowRenovationDTOs(List<Renovation> ownerRenovations)
        {
            List<ShowRenovationDTO> showRenovationDTOs = new List<ShowRenovationDTO>();

            foreach(Renovation temporaryRenovation in ownerRenovations.ToList())
            {
                RenovationCancelDeadlineAndStatusDTO renovationCancelDeadlineAndStatusDTO = FindRenovationCancelDeadlineAndStatus(temporaryRenovation.StartDate, temporaryRenovation.EndDate);

                ShowRenovationDTO showRenovationDTO = new ShowRenovationDTO(temporaryRenovation, renovationCancelDeadlineAndStatusDTO);
                showRenovationDTOs.Add(showRenovationDTO);
            }

            return showRenovationDTOs;
        }

        public RenovationCancelDeadlineAndStatusDTO FindRenovationCancelDeadlineAndStatus(DateTime startDate, DateTime endDate)
        {
            int days = Math.Abs(DateTime.Now.Subtract(startDate).Days);

            bool dateCheckStartDate = DateTime.Compare(DateTime.Now, startDate) >= 0;
            bool dateCheckEndDate = DateTime.Compare(DateTime.Now, endDate) <= 0;

            string renovationCancelDeadline = "You cannot cancel this renovation";
            string status = "Near future";

            if (dateCheckStartDate && dateCheckEndDate)
            {
                renovationCancelDeadline = "This renovation process is ongoing";
                status = "In process";
            }
            else if (!dateCheckEndDate)
            {
                renovationCancelDeadline = "This renovation is complete";
                status = "Past";
            }
            else if (!dateCheckStartDate && days > 5)
            {
                renovationCancelDeadline = "You have " + (days - 5) + " more days to cancel renovation request";
                status = "Can be cancelled";
            }

            RenovationCancelDeadlineAndStatusDTO renovationCancelDeadlineAndStatusDTO = new RenovationCancelDeadlineAndStatusDTO(renovationCancelDeadline, status);

            return renovationCancelDeadlineAndStatusDTO;
        }

        public int FindNumberOfUnratedGuests(string ownerUsername)
        {
            return rateGuestsService.FindNumberOfUnratedGuests(ownerUsername);
        }

        public List<ShowRenovationDTO> RemoveRenovation(ShowRenovationDTO showRenovationDTO, string ownerUsername)
        {
            Renovation temporaryRenovation = renovationRepository.FindById(showRenovationDTO.RenovationId);

            renovationRepository.RemoveById(showRenovationDTO.RenovationId);

            canceledRenovationService.AddRenovation(temporaryRenovation);

            return FindAllRenovations(ownerUsername);
        }

        public List<DateSlot> FindAvailableDateSlotsToRenovation(string accommodationName, DateTime startDate, DateTime endDate, int duration)
        {
            List<DateSlot> busyDateSlots = new List<DateSlot>();

            busyDateSlots = FindBusyDateSlots(accommodationName, startDate, endDate);

            List<DateSlot> availableDateSlots = new List<DateSlot>();

            DateTime temporaryStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            DateTime temporaryEndDate = temporaryStartDate.AddDays(duration);

            while (DateTime.Compare(temporaryEndDate, endDate) < 1) // < 1 da bi i poslednji dan usao u opseg
            {
                if (busyDateSlots.Find(x => IsDateBusy(temporaryStartDate, temporaryEndDate, x.StartDate, x.EndDate) == false) == null)
                {
                    DateSlot availableDateSlot = new DateSlot(temporaryStartDate, temporaryEndDate);
                    availableDateSlots.Add(availableDateSlot);
                }

                temporaryStartDate = temporaryStartDate.AddDays(1);
                temporaryEndDate = temporaryEndDate.AddDays(1);
            }

            return availableDateSlots;
        }

        private List<DateSlot> FindBusyDateSlots(string accommodationName, DateTime startDate, DateTime endDate)
        {
            List<DateSlot> busyDateSlots = new List<DateSlot>();

            List<Reservation> accommodationReservations = reservationService.FindReservationsByAccommodationName(accommodationName);

            foreach (Reservation temporaryReservation in accommodationReservations.ToList())
            {
                if (IsDateBusy(startDate, endDate, temporaryReservation.StartDate, temporaryReservation.EndDate) == false)
                {
                    busyDateSlots.Add(new DateSlot(temporaryReservation.StartDate, temporaryReservation.EndDate));
                }
            }

            List<Renovation> accommodationRenovations = renovationRepository.FindByAccommodationName(accommodationName);

            foreach (Renovation temporaryRenovation in accommodationRenovations.ToList())
            {
                if (IsDateBusy(startDate, endDate, temporaryRenovation.StartDate, temporaryRenovation.EndDate) == false)
                {
                    busyDateSlots.Add(new DateSlot(temporaryRenovation.StartDate, temporaryRenovation.EndDate));
                }
            }

            return busyDateSlots;
        }

        private bool IsDateBusy(DateTime slotStartDate, DateTime slotEndDate, DateTime reservationStartDate, DateTime reservationEndDate)
        {
            bool dateCheckSlotStartDate = (DateTime.Compare(slotStartDate, reservationStartDate) < 0 && DateTime.Compare(slotEndDate, reservationStartDate) < 0);
            bool dateCheckSlotEndDate = (DateTime.Compare(slotStartDate, reservationEndDate) > 0 && DateTime.Compare(slotEndDate, reservationEndDate) > 0);

            return dateCheckSlotStartDate || dateCheckSlotEndDate;
        }

        public Renovation CreateRenovationToAdd(string accommodationName, DateTime startDate, DateTime endDate, string description)
        {
            Renovation renovation = new Renovation(FindNextId(), FindAccommodationByAccommodationName(accommodationName), startDate, endDate, description);

            return renovation;
        }

        public int FindNextId()
        {
            return renovationRepository.NextId();
        }

        public Accommodation FindAccommodationByAccommodationName(string accommodationName)
        {
            return accommodationService.FindAccommodationByAccommodationName(accommodationName);
        }

        public void AddRenovation(Renovation renovation)
        {
            renovationRepository.Add(renovation);
        }

        public List<Renovation> FindAllRenovations()
        {
            return renovationRepository.FindAll();
        }

        public List<CancelledReservationsNotificationDTO> FindUnreadCancelledReservations(string ownerUsername)
        {
            return userService.FindUnreadCancelledReservations(ownerUsername);
        }

        public List<string> FindOwnerAccommodations(string ownerUsername)
        {
            return accommodationService.FindOwnerAccommodationNames(ownerUsername);
        }

        public void MarkAsReadNotificationsCancelledReservations(List<CancelledReservationsNotificationDTO> unreadCancelledReservations)
        {
            canceledReservationService.MarkAsReadNotificationsCancelledReservations(unreadCancelledReservations);
        }

        public int FindNumberOfNewForums(string ownerUsername)
        {
            return forumNotificationsToOwnerService.FindNumberOfNewForums(ownerUsername);
        }

        public void MarkAsReadNotificationsForums(string ownerUsername)
        {
            forumNotificationsToOwnerService.MarkAsReadNotificationsForums(ownerUsername);
        }

        public bool CheckFutureRenovations(int locationId, string ownerUsername)
        {
            return renovationRepository.IsFutureRenovationExistByLocationId(locationId, ownerUsername);
        }
    }
}
