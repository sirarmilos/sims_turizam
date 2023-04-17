using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class CreateReservationReschedulingRequestDTO
    {
        public ShowReservationDTO ShowReservationDTO { get; set; }

        public DateTime NewStartDate { get; set; }

        public DateTime NewEndDate { get; set; }

        public CreateReservationReschedulingRequestDTO()
        {

        }

        public CreateReservationReschedulingRequestDTO(ShowReservationDTO showReservationDTO, DateTime newStartDate, DateTime newEndDate)
        {
            ShowReservationDTO = showReservationDTO;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
        }
    }
}
