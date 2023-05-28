using InitialProject.IRepository;
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

        public Guest1ReportService()
        {
            guest1ReportRepository = Injector.Injector.CreateInstance<IGuest1ReportRepository>();
        }
    }
}
