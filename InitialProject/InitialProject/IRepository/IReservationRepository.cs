﻿using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IReservationRepository
    {
        void SaveReservations(List<Reservation> reservations);

        List<Reservation> FindAll();

        List<Reservation> FindByOwnerUsername(string ownerUsername);

        Reservation FindById(int reservationId);

        string FindOwnerByReservationId(int reservationId);

        void UpdateDatesToSelectedBookingMoveRequest(OwnerBookingMoveRequestsDTO selectedBookingMoveRequest);

        void RemoveById(int reservationId, int cancelledReservationId);

        void RemoveById(int reservationId);

        void Save(string guestUsername, Accommodation accommodation, DateTime startDate, DateTime endDate, int guestsNumber);

        int NextId();

        List<Reservation> FindAllByAccommodation(int id);

        List<Reservation> FindGuest1Reservations(string guest1);

        List<Reservation> FindReservationsByAccommodationName(string accommodationName);

        List<Reservation> FindByAccommodationId(int accommodationId);

        int FindAccommodationReservationCountByYear(int accommodationId, int year);

        List<Reservation> FindAccommodationReservationsByYear(int accommodationId, int year);

        bool HasGuest1MadeAnyReservationAtThisLocation(string username, ForumLocationDTO location);

        int FindNumberOfGuest1Reservations(string guest1Username);
        
        bool IsFutureReservationExistByLocationId(int locationId, string ownerUsername);
    }
}
