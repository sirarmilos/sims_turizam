using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Service
{
    public class ForumService
    {
        private readonly IForumRepository forumRepository;

        private readonly RateGuestsService rateGuestsService;

        private readonly UserService userService;

        private readonly CanceledReservationService canceledReservationService;

        private readonly CommentService commentService;

        private readonly Guest1ReportService guest1ReportService;
        
        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;

        private readonly ForumNotificationsToOwnerService forumNotificationsToOwnerService;

        public string Owner
        {
            get;
            set;
        }

        public string Guest1
        {
            get;
            set;
        }

        public ForumService(string username)
        {
            Owner = username;
            Guest1 = username;

            forumRepository = Injector.Injector.CreateInstance<IForumRepository>();

            rateGuestsService = new RateGuestsService(Owner);
            userService = new UserService();
            canceledReservationService = new CanceledReservationService();
            commentService = new CommentService();
            guest1ReportService = new Guest1ReportService();
            reservationReschedulingRequestService = new ReservationReschedulingRequestService(Guest1);
            forumNotificationsToOwnerService = new ForumNotificationsToOwnerService();
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

        public List<ShowOwnerForumCommentsDTO> FindComments(int forumId)
        {
            List<ShowOwnerForumCommentsDTO> showOwnerForumCommentsDTOs = new List<ShowOwnerForumCommentsDTO>();

            List<Comment> allComments = commentService.FindComments(forumId);

            foreach(Comment temporaryComment in allComments.ToList())
            {
                showOwnerForumCommentsDTOs.Add(new ShowOwnerForumCommentsDTO(temporaryComment));
            }

            showOwnerForumCommentsDTOs = showOwnerForumCommentsDTOs.OrderBy(x => x.CommentId).ToList();

            return showOwnerForumCommentsDTOs;
        }

        public void CreateForum(CreateForumDTO createForumDTO)
        {
            createForumDTO = ParseCreateForumDTOInput(createForumDTO);

            Forum forum = new Forum(
                forumRepository.NextId(),
                createForumDTO.Guest1Username, 
                new ForumLocationDTO {City = createForumDTO.City, Country = createForumDTO.Country}, 
                createForumDTO.Question, 
                false, 
                false);

            forumRepository.Add(forum);

            forumNotificationsToOwnerService.CreateOwnersNotification(forum);
        }

        private CreateForumDTO ParseCreateForumDTOInput(CreateForumDTO input)
        {
            input.City = input.City.Trim();
            input.Country = input.Country.Trim();

            input.City = char.ToUpper(input.City[0]) + input.City.Substring(1);
            input.Country = char.ToUpper(input.Country[0]) + input.Country.Substring(1);

            return input;
        }

        public bool Guest1HasNotification()
        {
            return reservationReschedulingRequestService.Guest1HasNotification(Guest1);
        }

        public bool IsSuperGuest(string guest1Username)
        {
            return userService.IsSuperGuest(guest1Username);
        }

        public void AddOwnerComment(string commenterUsername, string answer, int forumId)
        {
            commentService.AddOwnerComment(commenterUsername, answer, forumId);
        }

        public bool ReportGuest(int commentId, string ownerUsername)
        {
            return guest1ReportService.ReportGuest(commentId, ownerUsername);
        }
    }
}
