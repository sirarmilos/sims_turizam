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
    public class RateGuestRepository
    {
        private ReservationRepository reservationRepository;

        private const string FilePathRateGuest = "../../../Resources/Data/rateguests.csv";

        private readonly Serializer<RateGuest> rateGuestSerializer;

        private List<RateGuest> rateGuests;

        public RateGuestRepository()
        {
            rateGuestSerializer = new Serializer<RateGuest>();
            rateGuests = rateGuestSerializer.FromCSV(FilePathRateGuest);
        }

        public void SaveRateGuests(List<RateGuest> allRateGuests)
        {
            rateGuestSerializer.ToCSV(FilePathRateGuest, allRateGuests);
        }

        public List<RateGuest> FindAllRateGuests()
        {
            reservationRepository = new ReservationRepository();

            rateGuests = rateGuestSerializer.FromCSV(FilePathRateGuest);

            foreach(RateGuest temporaryRateGuest in rateGuests.ToList())
            {
                temporaryRateGuest.Reservation = reservationRepository.FindReservationByReservationId(temporaryRateGuest.Reservation.ReservationId);
            }

            return rateGuests;
        }

        public List<RateGuest> FindRateGuestsByOwnerUsername(string ownerUsername)
        {
            return FindAllRateGuests().ToList().FindAll(x => x.Reservation.Accommodation.OwnerUsername.Equals(ownerUsername) == true);
        }

        public RateGuest FindOwnerRateGuestByReservationId(string ownerUsername, int reservationId)
        {
            return FindRateGuestsByOwnerUsername(ownerUsername).ToList().Find(x => x.Reservation.ReservationId == reservationId);
        }

        public void UpdateRateGuests(RateGuest rateGuest)
        {
            List<RateGuest> allRateGuests = FindAllRateGuests();
            allRateGuests.Add(rateGuest);
            SaveRateGuests(allRateGuests);
        }
    }
}
