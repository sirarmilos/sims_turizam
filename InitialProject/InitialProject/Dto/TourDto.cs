using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
    public class TourDto
    {

        public string TourName { get; set; }

        public Location TourLocation { get; set; }

        public string Description { get; set; }

        public Language Languages { get; set; }

        public int MaxGuests { get; set; }

        public int Duration { get; set; }

        public List<string> Images { get; set; }

        public string Username { get; set; }

        public TourDto() { }

        public TourDto(string tourName, Location location, string description, Language languages, int maxGuests, int duration, List<string> images, string username)
        {
            TourName = tourName;
            TourLocation = location;
            Description = description;
            Languages = languages;
            MaxGuests = maxGuests;
            Duration = duration;
            Images = images;
            Username = username;
        }
    }
}
