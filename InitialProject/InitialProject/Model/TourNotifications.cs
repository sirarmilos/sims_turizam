using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class TourNotifications:ISerializable
    {
        public TourGuidence TourGuidence { get; set; }  
        public bool IsNotified { get; set; }

        public User User { get; set; }

        public string Type { get; set; }

        public TourNotifications()
        {
            Type = "";
        }

        public TourNotifications(TourGuidence tour) 
        {
            TourGuidence = tour;
            IsNotified = false;
            Type = "";
        }

        public string[] ToCSV()
        {
            return new string[] { TourGuidence.Id.ToString(), IsNotified.ToString(), User.Username.ToString(), Type.ToString() };
        }

        public void FromCSV(string[] values)
        {
            TourGuidence = new TourGuidence() { Id = int.Parse(values[0]) };
            IsNotified = bool.Parse(values[1]);
            User = new User() { Username = values[2] };
            Type = values[3];
        }
    }
}
