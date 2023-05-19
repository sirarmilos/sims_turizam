using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class RenovationCancelDeadlineAndStatusDTO
    {
        public string RenovationCancelDeadline { get; set; }

        public string Status { get; set; }

        public RenovationCancelDeadlineAndStatusDTO()
        {

        }

        public RenovationCancelDeadlineAndStatusDTO(string renovationCancelDeadline, string status)
        {
            RenovationCancelDeadline = renovationCancelDeadline;
            Status = status;
        }
    }
}
