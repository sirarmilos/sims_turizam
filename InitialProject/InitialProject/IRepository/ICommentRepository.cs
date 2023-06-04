using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ICommentRepository
    {
        List<Comment> FindAll();

        List<Comment> FindComments(int forumId);

        void Add(string commenterUsername, string answer, int forumId);

        void Save(List<Comment> allComments);

        int NextId();

        Comment FindById(int commentId);

        void AddReportNumber(int commentId);

        bool IsOwnerStillOwner(int forumId, string ownerUsername);
    }
}
