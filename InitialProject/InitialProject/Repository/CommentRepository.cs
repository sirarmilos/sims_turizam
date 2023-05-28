using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private const string FilePathComment = "../../../Resources/Data/comments.csv";

        private readonly Serializer<Comment> commentSerializer;

        private List<Comment> comments;

        public CommentRepository()
        {
            commentSerializer = new Serializer<Comment>();
        }
    }
}
