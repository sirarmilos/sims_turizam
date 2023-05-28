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
    public class Guest1ReportRepository : IGuest1ReportRepository
    {
        private const string FilePathForumNotificationsToOwner = "../../../Resources/Data/guest1reports.csv";

        private readonly Serializer<Guest1Report> guest1ReportSerializer;

        private List<Guest1Report> guest1Reports;

        public Guest1ReportRepository()
        {
            guest1ReportSerializer = new Serializer<Guest1Report>();
        }
    }
}
