using InitialProject.Repository;
using InitialProject.Serializer;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InitialProject.Model
{
    public class TourKeyPoint : ISerializable
    {
        public int Id { get; set; }
        public string? KeyPointName { get; set; }
        public Location Location { get; set; }
        public TourGuidence? TourGuidence { get; set; }

        public Boolean Passed { get; set; }

        public TourKeyPoint() { }

        public TourKeyPoint(int id, string keyPointName, Location location, TourGuidence tourGuidence, Boolean passed)
        {
            Id = id;
            KeyPointName = keyPointName;
            Location = location;
            TourGuidence = tourGuidence;
            Passed = passed;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), KeyPointName, Location.Id.ToString(), TourGuidence.Id.ToString(), Passed.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {

            Id = Convert.ToInt32(values[0]);
            KeyPointName = values[1];

            LocationRepository locationRepository = new();
            Location location = locationRepository.FindById(Convert.ToInt32(values[2]));
            Location = location;


            TourGuidenceService tourGuidanceService = new TourGuidenceService();
            TourGuidence tourGuidence = tourGuidanceService.GetById(Convert.ToInt32(values[3]));
            TourGuidence = tourGuidence;

            Passed = Convert.ToBoolean(values[4]);
        }
    }
}
