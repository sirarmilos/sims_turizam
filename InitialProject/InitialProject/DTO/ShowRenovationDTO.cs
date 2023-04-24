using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class ShowRenovationDTO
    {
        public int RenovationId { get; set; }

        public string AccommodationName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string RenovationCancelDeadline { get; set; }

        public string Status { get; set; }

        public ShowRenovationDTO() { }

        public ShowRenovationDTO(Renovation renovation, RenovationCancelDeadlineAndStatusDTO renovationCancelDeadlineAndStatusDTO)
        {
            RenovationId = renovation.RenovationId;
            AccommodationName = renovation.Accommodation.AccommodationName;
            StartDate = renovation.StartDate;
            EndDate = renovation.EndDate;
            RenovationCancelDeadline = renovationCancelDeadlineAndStatusDTO.RenovationCancelDeadline;
            Status = renovationCancelDeadlineAndStatusDTO.Status;
        }
    }
}
