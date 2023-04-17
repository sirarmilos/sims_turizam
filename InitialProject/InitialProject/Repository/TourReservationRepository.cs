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

        public TourReservation GetById(int id)
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

        public int GetSumGuestNumber(int guidenceId)
        {
            int sum = 0;
            foreach(TourReservation tr in tourReservations)
            {
                if(tr.tourGuidenceId == guidenceId && tr.Confirmed == true)
                {
                    sum += tr.numberOfGuests;
                }
            }
            return sum;
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

        public int UpdateKeyPointArrivals(int guidenceId, string username, int keyPoint)
        {

            foreach(TourReservation tr in tourReservations)
            {
                if(username == tr.userId && tr.tourGuidenceId== guidenceId)
                {
                    if(keyPoint > tr.TourKeyPointArrival.Count)
                    {
                        return -1;
                    }

                    for (int i = 0; i < tr.TourKeyPointArrival.Count; i++)
                    {
                        if (tr.TourKeyPointArrival[i] == true)
                            return -1;
                    }

                    tr.TourKeyPointArrival[keyPoint-1] = true;
                    tourReservationSerializer.ToCSV(FilePathReservatedTours, tourReservations);
                    return 1;
                }
            }
            return -1;
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

        public TourReservation FindByGuestAndGuidence(string userId, int tourGuidenceId)
        {
            TourReservation retVal = new TourReservation();
            foreach(TourReservation reservation in tourReservations)
            {
                if(reservation.userId == userId && reservation.tourGuidenceId == tourGuidenceId)
                {
                    retVal = reservation;
                    break;
                }
            }
            return retVal;
        }
    }
}
