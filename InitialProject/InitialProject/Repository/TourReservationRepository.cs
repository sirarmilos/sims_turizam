using InitialProject.Model;
using InitialProject.Dto;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.IRepository;

namespace InitialProject.Repository
{
    internal class TourReservationRepository : ITourReservationRepository
    {
        private const string FilePathReservatedTours = "../../../Resources/Data/reservatedtours.csv";

        private readonly Serializer<TourReservation> tourReservationSerializer;

        private List<TourReservation> tourReservations;

        public TourReservationRepository()
        {
            tourReservationSerializer = new Serializer<TourReservation>();
            //tourReservations = tourReservationSerializer.FromCSV(FilePathReservatedTours);
        }


        public List<TourReservation> FindAll()
        {
            return tourReservationSerializer.FromCSV(FilePathReservatedTours);
        }

        public TourReservation FindById(int tourReservatonId)
        {
            return FindAll().ToList().Find(x => x.Id == tourReservatonId);

        }

        public void Save(List<TourReservation> tourReservations)
        {
            tourReservationSerializer.ToCSV(FilePathReservatedTours, tourReservations);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }

            return FindAll().Max(x => x.Id) + 1;
        }

    }
}
