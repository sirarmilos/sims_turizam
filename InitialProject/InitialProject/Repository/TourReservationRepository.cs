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
            tourReservations = tourReservationSerializer.FromCSV(FilePathReservatedTours);
        }


        public List<TourReservation> FindAll()
        {
            return tourReservations;
        }

        public TourReservation FindById(int id)
        {
            TourReservation result = new TourReservation();
            foreach(TourReservation tourReservation in tourReservations)
            {
                if(id==tourReservation.Id)
                {
                    result = tourReservation;
                    break;
                }
            }

            return result;

        }

        public void Save(List<TourReservation> tourReservations)
        {
            tourReservationSerializer.ToCSV(FilePathReservatedTours, tourReservations);
        }

        public int NextId()
        {
            tourReservations = tourReservationSerializer.FromCSV(FilePathReservatedTours);
            if (tourReservations.Count < 1)
            {
                return 1;
            }
            return tourReservations.Max(c => c.Id) + 1;
        }

    }
}
