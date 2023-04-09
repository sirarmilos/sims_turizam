using InitialProject.Model;
using InitialProject.Serializer;
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
            return rateGuests;
        }
    }
}
