using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class GuestOnTourRepository
    {
        private const string FilePathGuestOnTour = "../../../Resources/Data/guestsontour.csv";

        private readonly Serializer<GuestOnTour> guestOnTourSerializer;

        private List<GuestOnTour> guestsOnTour;

        public GuestOnTourRepository()
        {
            guestOnTourSerializer = new Serializer<GuestOnTour>();
            guestsOnTour = guestOnTourSerializer.FromCSV(FilePathGuestOnTour);
        }

        public GuestOnTour GetById(int id) => guestsOnTour.FirstOrDefault(g => g.Id == id);




    }
}
