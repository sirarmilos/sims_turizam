using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InitialProject.Model
{
    public class TourKeyPoints : ISerializable
    {
        public int Id { get; set; }
        public string KeyPointName { get; set; }
        public Location Location { get; set; }

        public TourKeyPoints() { }

        public TourKeyPoints(int id, string keyPointName, Location location)
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
            Location = new Location() { Id = Convert.ToInt32(values[2]) };
        }
    }
}
