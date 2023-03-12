using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }

        public string AccommodationName { get; set; }

        public Location Location { get; set; }

        public string Type { get; set; }

        public int MaxGuests { get; set; }

        public int MinDaysReservation { get; set; }

        public int LeftCancelationDays { get; set; }

        public List<string> Images { get; set; }

        public Accommodation() { }

        public Accommodation(int id, string name, Location location, string type, int maxGuests, int minDaysReservation, int leftCancelationDays, List<string> images)
        {
            Id = id;
            AccommodationName = name;
            Location = location;
            Type = type;
            MaxGuests = maxGuests;
            MinDaysReservation = minDaysReservation;
            LeftCancelationDays = leftCancelationDays;
            Images = images;
        }

        public Accommodation(int id, string name, Location location, string type, int maxGuests, int minDaysReservation, int leftCancelationDays)
        {
            Id = id;
            AccommodationName = name;
            Location = location;
            Type = type;
            MaxGuests = maxGuests;
            MinDaysReservation = minDaysReservation;
            LeftCancelationDays = leftCancelationDays;
            Images = new List<string>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationName.ToString(), Location.Id.ToString(), Type.ToString(), MaxGuests.ToString(), MinDaysReservation.ToString(), LeftCancelationDays.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationName = values[1];
            Location = new Location() { Id = Convert.ToInt32(values[2]) };
            Type = values[3];
            MaxGuests = Convert.ToInt32(values[4]);
            MinDaysReservation = Convert.ToInt32(values[5]);
            LeftCancelationDays = Convert.ToInt32(values[6]);

        }

    }
}
