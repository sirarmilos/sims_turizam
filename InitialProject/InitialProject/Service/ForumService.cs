using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class ForumService
    {
        private readonly IForumRepository forumRepository;

        private readonly RateGuestsService rateGuestsService;

        private readonly UserService userService;

        private readonly CanceledReservationService canceledReservationService;

        public string Owner
        {
            get;
            set;
        }

        public ForumService(string username)
        {
            Owner = username;

            forumRepository = Injector.Injector.CreateInstance<IForumRepository>();

            rateGuestsService = new RateGuestsService(Owner);
            userService = new UserService();
            canceledReservationService = new CanceledReservationService();
        }

        public int FindNumberOfUnratedGuests(string ownerUsername)
        {
            return rateGuestsService.FindNumberOfUnratedGuests(ownerUsername);
        }

        public List<CancelledReservationsNotificationDTO> FindUnreadCancelledReservations(string ownerUsername)
        {
            return userService.FindUnreadCancelledReservations(ownerUsername);
        }

        public void MarkAsReadNotificationsCancelledReservations(List<CancelledReservationsNotificationDTO> unreadCancelledReservations)
        {
            canceledReservationService.MarkAsReadNotificationsCancelledReservations(unreadCancelledReservations);
        }

        public List<ShowOwnerForumsDTO> FindForums()
        {
            List<Forum> forums = forumRepository.FindAll();
            List<ShowOwnerForumsDTO> showOwnerForumsDTOs = new List<ShowOwnerForumsDTO>();

            foreach(Forum forum in forums.ToList())
            {
                ShowOwnerForumsDTO showOwnerForumsDTO = new ShowOwnerForumsDTO(forum);
                showOwnerForumsDTOs.Add(showOwnerForumsDTO);
            }

            return showOwnerForumsDTOs;
        }
    }
}
