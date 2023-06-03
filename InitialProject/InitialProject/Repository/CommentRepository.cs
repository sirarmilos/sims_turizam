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
    }
}
