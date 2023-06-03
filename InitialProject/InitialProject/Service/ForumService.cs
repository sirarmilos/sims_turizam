using InitialProject.DTO;
using InitialProject.IRepository;
using InitialProject.Model;
using System;
using System.Collections.Generic;
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
            commentService = new CommentService();
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

        public void AddOwnerComment(string commenterUsername, string answer, int forumId)
        {
            commentService.AddOwnerComment(commenterUsername, answer, forumId);
        }
    }
}
