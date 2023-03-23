using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class RateGuestsDTO
    {
        public int ReservationId { get; set; }
        public string GuestUsername { get; set; }
        public string RateGuestsDeadline { get; set; }

        public RateGuestsDTO()
        {

        }

        public RateGuestsDTO(int reservationId, string guestUsername, string rateGuestsDeadline)
        {
            ReservationId = reservationId;
            GuestUsername = guestUsername;
            RateGuestsDeadline = rateGuestsDeadline;
        }
    }
}
