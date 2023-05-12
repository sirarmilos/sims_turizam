using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Model
{
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public User User { get; set; } 
        public Location Location { get; set; }

        public string Description { get; set; }
        public Language Language { get; set; }

        public int GuestNumber { get; set; }

        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }

        public DateTime CreationDate { get; set; }

        public string Status { get; set; }  

        public TourRequest() { }

        public TourRequest(int id, User user, Location location, string description, Language language, int guestNumber, DateTime startDate, DateTime endDate, DateTime creationDate, string status)
        {
            Id = id;
            User = user;
            Location = location;
            Description = description;
            Language = language;
            GuestNumber = guestNumber;
            StartDate = startDate;
            EndDate = endDate;
            CreationDate = creationDate;
            Status = status;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);

            User = new User () { Username = values[1] };

            Location = new Location() { Id = Convert.ToInt32(values[2]) };

            Description = values[3];

            Language = (Language)Enum.Parse(typeof(Language), values[4]);

            GuestNumber = Convert.ToInt32(values[5]);

            StartDate = Convert.ToDateTime(values[6]);
            EndDate = Convert.ToDateTime(values[7]);
            CreationDate = Convert.ToDateTime(values[8]);

            Status = Convert.ToString(values[9]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), User.Username, Location.Id.ToString(), Description, Language.ToString(), GuestNumber.ToString(), StartDate.Date.ToString(), EndDate.Date.ToString(), CreationDate.Date.ToString(), Status };
            return csvValues;
        }
    }
}
