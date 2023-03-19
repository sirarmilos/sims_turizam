using InitialProject.Repository;
using InitialProject.Serializer;
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
        public string KeyPointName { get; set; }
        public Location Location { get; set; }

        public TourKeyPoint() { }

        public TourKeyPoint(int id, string keyPointName, Location location)
        {
            Id = id;
            KeyPointName = keyPointName;
            Location = location;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), KeyPointName, Location.Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            KeyPointName = values[1];

            LocationRepository locationRepository = new LocationRepository();
            Location location = locationRepository.GetById(Convert.ToInt32(values[2]));
            Location = location;
        }

       /* public string ToString()
        {
            return KeyPointName + " " + Location.ToString();
        }*/
    }
}
