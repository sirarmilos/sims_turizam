using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Repository;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class TourGuidence : ISerializable
    {
        public int Id { get; set; }

        public Tour? Tour { get; set; }

        public DateTime StartTime { get; set; }

        public Boolean Started { get; set; }
        public Boolean Finished { get; set; }
        public Boolean Cancelled { get; set; }

        public int FreeSlots { get; set; }

        public TourGuidence() { }

        public TourGuidence(int id, Tour tour, DateTime startTime, Boolean started, Boolean finished, Boolean cancelled)
        {
            Id = id;
            Tour = tour;
            StartTime = startTime;
            Started = started;
            Finished = finished;
            Cancelled = cancelled;
            FreeSlots = tour.MaxGuests;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Tour.Id.ToString(), StartTime.ToString(), Started.ToString(), Finished.ToString(), Cancelled.ToString(), FreeSlots.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);

            TourRepository tourRepository = new();
            Tour tour = tourRepository.FindById(Convert.ToInt32(values[1]));
            Tour = tour;

            StartTime = Convert.ToDateTime(values[2]);
            Started = Convert.ToBoolean(values[3]);
            Finished = Convert.ToBoolean(values[4]);
            Cancelled = Convert.ToBoolean(values[5]);
            FreeSlots = Convert.ToInt32(values[6]);
        }
    }
}
