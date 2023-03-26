using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class TourGuestRepository
    {
        private const string FilePathTourGuest = "../../../Resources/Data/tourguests.csv";

        private readonly Serializer<TourGuest> tourGuestSerializer;

        private List<TourGuest> tourGuests;

        public TourGuestRepository()
        {
            tourGuestSerializer = new Serializer<TourGuest>();
            tourGuests = tourGuestSerializer.FromCSV(FilePathTourGuest);
        }
        
        public TourGuest GetById(int id) => tourGuests.FirstOrDefault(t => t.Id == id);

    }
}
