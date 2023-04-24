using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ICanceledReservationRepository
    {
        void Save(CanceledReservation canceledReservation);

        void Save(List<CanceledReservation> allCanceledReservations);

        List<CanceledReservation> FindAll();

        List<CanceledReservation> FindByOwnerUsername(string ownerUsername);

        List<CanceledReservation> FindUnreadCancelledReservationsByOwnerUsername(string ownerUsername);

        CanceledReservation FindByDTO(CancelledReservationsNotificationDTO cancelledReservationsNotificationDTO);

        void UpdateViewed(CanceledReservation canceledReservation);
    }
}
