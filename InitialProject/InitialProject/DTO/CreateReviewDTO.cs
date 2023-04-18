using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class CreateReviewDTO    
    {
        public int ReservationId { get; set; }
        public string AccommodationName { get; set; }
        public string Deadline { get; set; }

        public CreateReviewDTO()
        {

        }

        public CreateReviewDTO(int reservationId, string accommodationName, string deadline)
        {
            ReservationId = reservationId;
            AccommodationName = accommodationName;
            Deadline = deadline;
        }
    }
}
