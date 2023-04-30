using InitialProject.DTO;
using InitialProject.Injector;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void SaveViewed(CancelledReservationsNotificationDTO cancelledReservationsNotificationDTO)
        {
            canceledReservationRepository.UpdateViewed(canceledReservationRepository.FindByDTO(cancelledReservationsNotificationDTO));
        }

        public List<int> FindAccommodationCanceledReservationsYears(int accommodationId)
        {
            List<int> yearsCanceledReservations = new List<int>();

            List<CanceledReservation> canceledReservations = canceledReservationRepository.FindByAccommodationId(accommodationId);

            foreach(CanceledReservation temporaryCanceledReservation in canceledReservations.ToList())
            {
                int year = temporaryCanceledReservation.StartDate.Year;
                if (yearsCanceledReservations.Exists(x => x == year) == false)
                {
                    yearsCanceledReservations.Add(year);
                }
            }

            return yearsCanceledReservations;
        }
    }
}