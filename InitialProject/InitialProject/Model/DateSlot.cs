using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class DateSlot
    {
        public DateTime StartDate  { get; set; }

        public DateTime EndDate { get; set; }

        public DateSlot() { }

        public DateSlot(DateTime startDate, DateTime endDate) {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
