﻿using InitialProject.DTO;
using InitialProject.Injector;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Service
{
    public class CanceledReservationService
    {
        private ICanceledReservationRepository canceledReservationRepository;

        public CanceledReservationService()
        {
            canceledReservationRepository = Injector.Injector.CreateInstance<ICanceledReservationRepository>();
            //canceledReservationRepository = new CanceledReservationRepository();
        }

        public void Save(CanceledReservation canceledReservation)
        {
            canceledReservationRepository.Save(canceledReservation);
        }

        public List<CancelledReservationsNotificationDTO> FindOwnerUnreadCancelledReservations(string ownerUsername)
        {
            List<CanceledReservation> ownerUnreadCancelledReservations = canceledReservationRepository.FindUnreadCancelledReservationsByOwnerUsername(ownerUsername);

            List<CancelledReservationsNotificationDTO> cancelledReservationsNotificationDTOs = new List<CancelledReservationsNotificationDTO>();

            foreach(CanceledReservation temporaryCanceledReservation in ownerUnreadCancelledReservations.ToList())
            {
                CancelledReservationsNotificationDTO cancelledReservationsNotificationDTO = new CancelledReservationsNotificationDTO(temporaryCanceledReservation);
                cancelledReservationsNotificationDTOs.Add(cancelledReservationsNotificationDTO);
            }

            return cancelledReservationsNotificationDTOs;
        }

        public int FindAccommodationCanceledReservationCountByYear(int accommodationId, int year)
        {
            return canceledReservationRepository.FindAccommodationCanceledReservationCountByYear(accommodationId, year);
        }

        public List<int> FindAccommodationCanceledReservationCountByMonth(int accommodationId, int year)
        {
            List<int> canceledReservationCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            List<CanceledReservation> yearCanceledReservations = canceledReservationRepository.FindAccommodationCanceledReservationsByYear(accommodationId, year);

            foreach(CanceledReservation temporaryCanceledReservation in yearCanceledReservations.ToList())
            {
                canceledReservationCount[temporaryCanceledReservation.CancellationDate.Month - 1]++;
            }

            return canceledReservationCount;
        }

        public void MarkAsReadNotificationsCancelledReservations(List<CancelledReservationsNotificationDTO> unreadCancelledReservations)
        {
            canceledReservationRepository.UpdateViewed(unreadCancelledReservations);
        }
    }
}