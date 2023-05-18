using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class Guest1NotificationDTO
    {
        public int ReservationId { get; set; }

        public int RequestId { get; set; }

        public string AccommodationName { get; set; }

        public string OwnerUsername { get; set; }

        public bool ViewedByGuest { get; set; }

        public string Status { get; set; }

        public Guest1NotificationDTO()
        {

        }

        public Guest1NotificationDTO(int reservationId, int requestId, string accommodationName, string ownerUsername, bool viewedByGuest, string status)
        {
            ReservationId = reservationId;
            RequestId = requestId;
            AccommodationName = accommodationName;
            OwnerUsername = ownerUsername;
            ViewedByGuest = viewedByGuest;
            Status = status;
        }

        public Guest1NotificationDTO(Guest1RebookingRequestsDTO guest1RebookingRequestsDTO)
        {
            ReservationId = guest1RebookingRequestsDTO.ReservationId;
            AccommodationName = guest1RebookingRequestsDTO.AccommodationName;
            OwnerUsername = guest1RebookingRequestsDTO.OwnerUsername;
            ViewedByGuest = guest1RebookingRequestsDTO.ViewedByGuest;
            Status = guest1RebookingRequestsDTO.Status;
        }

    }
}
