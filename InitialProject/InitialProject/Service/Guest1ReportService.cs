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
    public class Guest1ReportService
    {
        private readonly IGuest1ReportRepository guest1ReportRepository;

        private readonly CommentService commentService;

        public Guest1ReportService()
        {
            guest1ReportRepository = Injector.Injector.CreateInstance<IGuest1ReportRepository>();
            commentService = new CommentService();
        }

        public bool ReportGuest(int commentId, string ownerUsername)
        {
            if(guest1ReportRepository.IsReportExist(commentId, ownerUsername) == true)
            {
                return false;
            }

            Comment comment = commentService.FindById(commentId);
            guest1ReportRepository.Add(comment, ownerUsername);
            commentService.AddReportNumber(comment.CommentId);

            return true;
        }
    }
}
