using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IRateGuestRepository
    {
        void Save(List<RateGuest> allRateGuests);

        List<RateGuest> FindAll();

        List<RateGuest> FindByOwnerUsername(string ownerUsername);

        RateGuest FindOwnerRateGuestByReservationId(string ownerUsername, int reservationId);

        void Add(RateGuest rateGuest);
    }
}
