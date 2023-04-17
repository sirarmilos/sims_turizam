using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Repository
{
    public class RateGuestRepository : IRateGuestRepository
    {
        private ReservationRepository reservationRepository;

        private const string FilePathRateGuest = "../../../Resources/Data/rateguests.csv";

        private readonly Serializer<RateGuest> rateGuestSerializer;

        private List<RateGuest> rateGuests;

        public RateGuestRepository()
        {
            rateGuestSerializer = new Serializer<RateGuest>();
        }

        public void Save(List<RateGuest> allRateGuests)
        {
            rateGuestSerializer.ToCSV(FilePathRateGuest, allRateGuests);
        }

        public List<RateGuest> FindAll()
        {
            reservationRepository = new ReservationRepository();

            rateGuests = rateGuestSerializer.FromCSV(FilePathRateGuest);

            foreach(RateGuest temporaryRateGuest in rateGuests.ToList())
            {
                temporaryRateGuest.Reservation = reservationRepository.FindById(temporaryRateGuest.Reservation.ReservationId);
            }

            return rateGuests;
        }

        public List<RateGuest> FindByOwnerUsername(string ownerUsername)
        {
            return FindAll().ToList().FindAll(x => x.Reservation.Accommodation.OwnerUsername.Equals(ownerUsername) == true);
        }

        public RateGuest FindOwnerRateGuestByReservationId(string ownerUsername, int reservationId)
        {
            return FindByOwnerUsername(ownerUsername).ToList().Find(x => x.Reservation.ReservationId == reservationId);
        }

        public void Add(RateGuest rateGuest)
        {
            List<RateGuest> allRateGuests = FindAll();
            allRateGuests.Add(rateGuest);
            Save(allRateGuests);
        }
    }
}
