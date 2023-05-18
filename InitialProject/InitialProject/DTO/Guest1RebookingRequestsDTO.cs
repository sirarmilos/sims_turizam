using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class Guest1RebookingRequestsDTO
    {
        public int ReservationId { get; set; }

        public string AccommodationName { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string OwnerUsername { get; set; }

        public DateTime OldStartDate { get; set; }

        public DateTime OldEndDate { get; set; }

        public DateTime NewStartDate { get; set; }

        public DateTime NewEndDate { get; set; }

        public string Comment { get; set; }

        public bool ViewedByGuest { get; set; }

        public string Status { get; set; }

        public Guest1RebookingRequestsDTO()
        {

        }

        public Guest1RebookingRequestsDTO(int reservationId, string accommodationName, DateTime oldStartDate, DateTime oldEndDate, DateTime newStartDate, DateTime newEndDate, string status)
        {
            ReservationId = reservationId;
            AccommodationName = accommodationName;
            OldStartDate = oldStartDate;
            OldEndDate = oldEndDate;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            Status = status;
        }

        public Guest1RebookingRequestsDTO(ReservationReschedulingRequest temporaryReservationReschedulingRequest)
        {
            ReservationId = temporaryReservationReschedulingRequest.Reservation.ReservationId;
            AccommodationName = temporaryReservationReschedulingRequest.Reservation.Accommodation.AccommodationName;
            City = temporaryReservationReschedulingRequest.Reservation.Accommodation.Location.City;
            Country = temporaryReservationReschedulingRequest.Reservation.Accommodation.Location.Country;
            OwnerUsername = temporaryReservationReschedulingRequest.Reservation.Accommodation.OwnerUsername;
            OldStartDate = temporaryReservationReschedulingRequest.OldStartDate;
            OldEndDate = temporaryReservationReschedulingRequest.OldEndDate;
            NewStartDate = temporaryReservationReschedulingRequest.NewStartDate;
            NewEndDate = temporaryReservationReschedulingRequest.NewEndDate;
            Comment = temporaryReservationReschedulingRequest.Comment;
            ViewedByGuest = temporaryReservationReschedulingRequest.ViewedByGuest;
            Status = temporaryReservationReschedulingRequest.Status;
        }

        public Guest1RebookingRequestsDTO(Reservation reservation, ReservationReschedulingRequest temporaryRebookingRequest)
        {
            ReservationId = reservation.ReservationId;
            AccommodationName = reservation.Accommodation.AccommodationName;
            City = reservation.Accommodation.Location.City;
            Country = reservation.Accommodation.Location.Country;
            OwnerUsername = reservation.Accommodation.OwnerUsername;
            OldStartDate = temporaryRebookingRequest.OldStartDate;
            OldEndDate = temporaryRebookingRequest.OldEndDate;
            NewStartDate = temporaryRebookingRequest.NewStartDate;
            NewEndDate = temporaryRebookingRequest.NewEndDate;
            Comment = temporaryRebookingRequest.Comment;
            ViewedByGuest = temporaryRebookingRequest.ViewedByGuest;
            Status = temporaryRebookingRequest.Status;
        }   

    }
}
