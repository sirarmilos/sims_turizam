using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class GuestOnTour : ISerializable
    {
        public int Id { get; set; }
        public TourGuest? Guest { get; set; }
        public TourKeyPoint? TourKeyPoint { get; set; }

        public GuestOnTour() { }

        public GuestOnTour(int id, TourGuest guest, TourKeyPoint tourKeyPoint)
        {
            Id = id;
            Guest = guest;
            TourKeyPoint = tourKeyPoint;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Guest.Id.ToString(), TourKeyPoint.Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);

            TourGuestRepository tourGuestRepository = new TourGuestRepository();
            TourGuest guest = tourGuestRepository.GetById(Convert.ToInt32(values[1]));
            Guest = guest;

            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            TourKeyPoint tourKeyPoint = tourKeyPointRepository.GetById(Convert.ToInt32(values[2]));
            TourKeyPoint = tourKeyPoint;





        }


    }
}
