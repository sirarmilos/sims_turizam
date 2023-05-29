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

        public List<OwnerComment> FindOwnerComments(int forumId)
        {
            return commentRepository.FindOwnerComments(forumId);
        }

        public List<Guest1Comment> FindGuestComments(int forumId)
        {
            return commentRepository.FindGuestComments(forumId);
        }
    }
}
