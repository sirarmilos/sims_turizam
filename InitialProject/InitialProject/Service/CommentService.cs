using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class CommentService
    {
        private readonly ICommentRepository commentRepository;

        public CommentService()
        {
            commentRepository = Injector.Injector.CreateInstance<ICommentRepository>();
        }

        public List<Comment> FindComments(int forumId)
        {
            return commentRepository.FindComments(forumId);
        }

        public void AddOwnerComment(string commenterUsername, string answer, int forumId)
        {
            commentRepository.Add(commenterUsername, answer, forumId);
        }
    }
}
