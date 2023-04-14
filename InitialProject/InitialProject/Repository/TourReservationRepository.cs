using InitialProject.Model;
using InitialProject.Dto;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class TourReservationRepository
    {
        private const string FilePathReservatedTours = "../../../Resources/Data/reservatedtours.csv";

        private readonly Serializer<TourReservation> tourReservationSerializer;

        private List<TourReservation> tourReservations;

        public TourReservationRepository()
        {
            tourReservationSerializer = new Serializer<TourReservation>();
            tourReservations = tourReservationSerializer.FromCSV(FilePathReservatedTours);
        }

        public List<TourReservation> GetAll()
        {
            return tourReservations;
        }

        public List<Dto.ReservationDisplayDto> GetAllForOneTourGuidence(int guidenceId)
        {
            List<Dto.ReservationDisplayDto> reservations = new List<Dto.ReservationDisplayDto>();
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            List<TourKeyPoint> tourKeyPoints = new List<TourKeyPoint>();

            tourKeyPoints = tourKeyPointRepository.Load(guidenceId);           

            foreach(TourReservation tr in tourReservations)
            {
                
                if(tr.tourGuidenceId == guidenceId)
                {
                    Dto.ReservationDisplayDto dto = new Dto.ReservationDisplayDto();
                    dto.userId = tr.userId;
                    dto.tourGuidenceId = tr.tourGuidenceId;
                    dto.TourKeyPointArrival = tr.TourKeyPointArrival;
                    dto.numberOfGuests = tr.numberOfGuests;
                    dto.TourKeyPoints = tourKeyPoints;
                    reservations.Add(dto);
                }
            }
            return reservations;
        }

        public void UpdateKeyPointField(Dto.ReservationDisplayDto reservationDisplayDto)
        {
            


        }

        public List<Boolean> SetArrivalsToFalse(int guidenceId)
        {
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            List<TourKeyPoint> tourKeyPoints = new List<TourKeyPoint>();

            tourKeyPoints = tourKeyPointRepository.Load(guidenceId);
            List<Boolean> retVal = new List<Boolean>();
            foreach(TourKeyPoint kp in tourKeyPoints)
            {
                retVal.Add(false);
            }
            return retVal;

        }

        public void UpdateKeyPointArrivals()
        {
            tourReservationSerializer.ToCSV(FilePathReservatedTours, tourReservations);
        }


    }
}
