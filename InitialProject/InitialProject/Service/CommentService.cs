using InitialProject.DTO;
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

        public void AddGuest1Comment(string commenterUsername, string answer, int forumId)
        {
            commentRepository.AddGuest1Comment(commenterUsername, answer, forumId); 
        }
        public Comment FindById(int commentId)
        {
            return commentRepository.FindById(commentId);
        }

        public void AddReportNumber(int commentId)
        {
            commentRepository.AddReportNumber(commentId);
        }

        public bool CheckComments(int forumId)
        {
            return CheckOwnerComments(forumId) && CheckGuest1Coments(forumId);
        }

        public bool CheckOwnerComments(int forumId)
        {
            return commentRepository.CountOwnerComments(forumId) >= 10;
        }

        public bool CheckGuest1Coments(int forumId)
        {
            return commentRepository.CountGuest1Comments(forumId) >= 20;
        }

        public void CheckIsStillOwner(string country, string city)
        {
            commentRepository.CheckIsStillOwner(country, city);
        }
    }
}
