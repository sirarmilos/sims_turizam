using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }

        public string TourName { get; set; }

        public Location Location { get; set; }

        public string Description { get; set; }

        public Languages Languages { get; set; }

        public int MaxGuests { get; set; }

        public List<TourKeyPoints> TourKeyPoints { get; set; }

        public List<DateTime> TourDate { get; set; }    

        public int Duration { get; set; }

        public List<string> Images { get; set; }
    
        public Tour() { }

        public Tour(int id, string tourName, Location location, string description, Languages languages, int maxGuests, List<TourKeyPoints> tourKeyPoints, List<DateTime> tourDate, int duration, List<string> images)
        {
            Id = id;
            TourName = tourName;
            Location = location;
            Description = description;
            Languages = languages;
            MaxGuests = maxGuests;
            TourKeyPoints = tourKeyPoints;
            TourDate = tourDate;
            Duration = duration;
            Images = images;
        }

        public string[] ToCSV()
        {
            string imageToString = "";

            foreach (string image in Images)
            {
                imageToString += image;
                imageToString += ", ";
            }

            imageToString = imageToString.Substring(0, imageToString.Length - 2);

            string dateToString = "";

            foreach (DateTime date in TourDate)
            {
                dateToString += date;
                dateToString += ", ";
            }

            dateToString = dateToString.Substring(0, dateToString.Length - 2);

            string tourKeyPointsToString = "";

            foreach (TourKeyPoints tourKeyPoint in TourKeyPoints)
            {
                tourKeyPointsToString += tourKeyPoint.Id;
                tourKeyPointsToString += ", ";
            }

            tourKeyPointsToString = tourKeyPointsToString.Substring(0, tourKeyPointsToString.Length - 2);


            string[] csvValues = { Id.ToString(), TourName, Location.Id.ToString(), Description, Languages.ToString(), MaxGuests.ToString(), tourKeyPointsToString, dateToString, Duration.ToString(), imageToString};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourName = values[1];
            //Location = new Location() { Id = Convert.ToInt32(values[2]) };
            Description = values[3];

            MaxGuests = Convert.ToInt32(values[5]);
            


            Duration = Convert.ToInt32(values[8]);  
            


        }

        
    }
}
