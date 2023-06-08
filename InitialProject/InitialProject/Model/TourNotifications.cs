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

        public TourNotifications() { }

        public TourNotifications(TourGuidence tour) 
        {
            TourGuidence = tour;
            IsNotified = false;
        }

        public string[] ToCSV()
        {
            return new string[] { TourGuidence.Id.ToString(), IsNotified.ToString(), User.Username.ToString() };
        }

        public void FromCSV(string[] values)
        {
            TourGuidence = new TourGuidence() { Id = int.Parse(values[0]) };
            IsNotified = bool.Parse(values[1]);
            User = new User() { Username = values[2] };
        }
    }
}
