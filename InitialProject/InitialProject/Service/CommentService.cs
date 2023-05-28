using InitialProject.IRepository;
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
    }
}
