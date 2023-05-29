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

        private const string FilePathComment = "../../../Resources/Data/comments.csv";

        private readonly Serializer<Comment> commentSerializer;

        private readonly Serializer<OwnerComment> ownerCommentSerializer;

        private readonly Serializer<Guest1Comment> guestCommentSerializer;

        private List<Comment> comments;

        private List<OwnerComment> ownerComments;

        private List<Guest1Comment> guestComments;

        public CommentRepository()
        {
            commentSerializer = new Serializer<Comment>();
            ownerCommentSerializer = new Serializer<OwnerComment>();
            guestCommentSerializer = new Serializer<Guest1Comment>();
        }

        public List<OwnerComment> FindAllOwnerComments()
        {
            forumRepository = new ForumRepository();

            ownerComments = ownerCommentSerializer.FromCSV(FilePathComment);

            foreach (OwnerComment temporaryOwnerComment in ownerComments.ToList())
            {
                temporaryOwnerComment.Forum = forumRepository.FindById(temporaryOwnerComment.Forum.ForumId);
            }

            return ownerComments;
        }

        public List<Guest1Comment> FindAllGuestComments()
        {
            forumRepository = new ForumRepository();

            guestComments = guestCommentSerializer.FromCSV(FilePathComment);

            foreach (Guest1Comment temporaryGuestComment in guestComments.ToList())
            {
                temporaryGuestComment.Forum = forumRepository.FindById(temporaryGuestComment.Forum.ForumId);
            }

            return guestComments;
        }

        public List<OwnerComment> FindOwnerComments(int forumId)
        {
            return FindAllOwnerComments().ToList().FindAll(x => x.CommenterType.Equals("owner") == true && x.Forum.ForumId == forumId);
        }

        public List<Guest1Comment> FindGuestComments(int forumId)
        {
            return FindAllGuestComments().ToList().FindAll(x => x.CommenterType.Equals("guest") == true && x.Forum.ForumId == forumId);
        }
    }
}
