using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
    internal class TourDto
    {
        public string TourName { get; set; }

        public Location TourLocation { get; set; }

        public string Description { get; set; }

        public Language Languages { get; set; }

        public int MaxGuests { get; set; }

        public List<TourKeyPoint> TourKeyPoints { get; set; }

        public List<DateTime> TourDate { get; set; }

        public int Duration { get; set; }

        public List<string> Images { get; set; }

        public TourDto() { }

        public TourDto(string tourName, Location location, string description, Language languages, int maxGuests, List<TourKeyPoint> tourKeyPoints, List<DateTime> tourDate, int duration, List<string> images)
        {
            TourName = tourName;
            TourLocation = location;
            Description = description;
            Languages = languages;
            MaxGuests = maxGuests;
            TourKeyPoints = tourKeyPoints;
            TourDate = tourDate;
            Duration = duration;
            Images = images;
        }
    }
}
