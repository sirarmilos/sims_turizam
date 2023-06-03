using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IGuest1ReportRepository
    {
        List<Guest1Report> FindAll();

        bool IsReportExist(int commentId, string ownerUsername);

        void Add(Comment comment, string ownerUsername);

        void Save(List<Guest1Report> allGuestReports);

        int NextId();
    }
}
