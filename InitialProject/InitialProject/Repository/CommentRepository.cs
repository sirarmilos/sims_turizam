using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private ForumRepository forumRepository;

        private ReservationRepository reservationRepository;

        private const string FilePathComment = "../../../Resources/Data/comments.csv";

        private readonly Serializer<Comment> commentSerializer;

        private List<Comment> comments;

        public CommentRepository()
        {
            commentSerializer = new Serializer<Comment>();
        }

        public List<Comment> FindAll()
        {
            forumRepository = new ForumRepository();

            comments = commentSerializer.FromCSV(FilePathComment);

            foreach(Comment temporaryComment in comments.ToList())
            {
                temporaryComment.Forum = forumRepository.FindById(temporaryComment.Forum.ForumId);
            }

            return comments;
        }

        public List<Comment> FindComments(int forumId)
        {
            return FindAll().ToList().FindAll(x => x.Forum.ForumId == forumId);
        }

        public void Add(string commenterUsername, string answer, int forumId)
        {
            forumRepository = new ForumRepository();

            Comment ownerComment = new Comment(NextId(), forumRepository.FindById(forumId), commenterUsername, "owner", answer, true, false, -1);

            List<Comment> allComments = FindAll();
            allComments.Add(ownerComment);
            Save(allComments);
        }

        public void Save(List<Comment> allComments)
        {
            commentSerializer.ToCSV(FilePathComment, allComments);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }

            return FindAll().Max(x => x.CommentId) + 1;
        }

        public void AddGuest1Comment(string commenterUsername, string answer, int forumId)
        {
            forumRepository = new ForumRepository();

            reservationRepository = new ReservationRepository();

            Comment guest1Comment = new Comment(NextId(), forumRepository.FindById(forumId), commenterUsername, "guest1", answer, false, reservationRepository.HasGuest1MadeAnyReservation(commenterUsername), 0);

            List<Comment> allComments = FindAll();
            allComments.Add(guest1Comment);
            Save(allComments);
        }
        public Comment FindById(int commentId)
        {
            return FindAll().ToList().Find(x => x.CommentId == commentId);
        }

        public void AddReportNumber(int commentId)
        {
            List<Comment> allComments = FindAll();
            allComments.ToList().Where(x => x.CommentId == commentId).SetValue(x => x.NumberOfReports += 1);
            Save(allComments);
        }

        public bool IsOwnerStillOwner(int forumId, string ownerUsername)
        {
            return FindComments(forumId).ToList().Exists(x => x.CommenterType.Equals("owner") == true && x.CommenterUsername.Equals(ownerUsername) == true && x.IsStillOwner == false);
        }

        public int CountOwnerComments(int forumId)
        {
            return FindComments(forumId).ToList().FindAll(x => x.CommenterType.Equals("owner") == true).Count;
        }

        public int CountGuest1Comments(int forumId)
        {
            return FindComments(forumId).ToList().FindAll(x => x.CommenterType.Equals("guest") == true && x.Visited == true).Count;
        }
    }
}
