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

        public string Address { get; set; }

        public string Type { get; set; }

        public string Image { get; set; }   

        public CreateReviewDTO()
        {

        }

        public CreateReviewDTO(int reservationId, string accommodationName, string deadline, string address, string type, string image)
        {
            ReservationId = reservationId;
            AccommodationName = accommodationName;
            Deadline = deadline;
            Address = address;
            Type = type;
            Image = image;
        }
    }
}
