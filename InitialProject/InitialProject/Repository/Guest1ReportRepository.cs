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
    public class Guest1ReportRepository : IGuest1ReportRepository
    {
        private CommentRepository commentRepository;

        private const string FilePathGuest1Report = "../../../Resources/Data/guest1reports.csv";

        private readonly Serializer<Guest1Report> guest1ReportSerializer;

        private List<Guest1Report> guestReports;

        public Guest1ReportRepository()
        {
            guest1ReportSerializer = new Serializer<Guest1Report>();
        }

        public List<Guest1Report> FindAll()
        {
            commentRepository = new CommentRepository();

            guestReports = guest1ReportSerializer.FromCSV(FilePathGuest1Report);

            foreach (Guest1Report temporaryGuestReport in guestReports.ToList())
            {
                temporaryGuestReport.Guest1Comment = commentRepository.FindById(temporaryGuestReport.Guest1Comment.CommentId);
            }

            return guestReports;
        }

        public bool IsReportExist(int commentId, string ownerUsername)
        {
            return FindAll().Exists(x => x.Guest1Comment.CommentId == commentId == true && x.ReportMaker.Equals(ownerUsername) == true);
        }

        public void Add(Comment comment, string ownerUsername)
        {
            Guest1Report guestReport = new Guest1Report(NextId(), comment, ownerUsername);

            List<Guest1Report> allGuestReports = FindAll();
            allGuestReports.Add(guestReport);
            Save(allGuestReports);
        }

        public void Save(List<Guest1Report> allGuestReports)
        {
            guest1ReportSerializer.ToCSV(FilePathGuest1Report, allGuestReports);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }

            return FindAll().Max(x => x.Guest1ReportId) + 1;
        }
    }
}
