using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }

        public string? TourName { get; set; }

        public Location? Location { get; set; }

        public string? Description { get; set; }

        public Language Language { get; set; }

        public int MaxGuests { get; set; }   

        public int Duration { get; set; }

        public List<string> Images { get; set; }

        public string GuideUsername { get; set; }

        public Tour() { }

        public Tour(int id, string tourName, Location location, string description, Language languages, int maxGuests, int duration, List<string> images, string username)
        {
            Id = id;
            TourName = tourName;
            Location = location;
            Description = description;
            Language = languages;
            MaxGuests = maxGuests;
            Duration = duration;
            Images = images;
            GuideUsername = username;
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

            string[] csvValues = { Id.ToString(), TourName, Location.Id.ToString(), Description, Language.ToString(), MaxGuests.ToString(), Duration.ToString(), imageToString, GuideUsername };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {

            Id = Convert.ToInt32(values[0]);

            TourName = values[1];

            LocationRepository locationRepository = new LocationRepository();
            Location location = locationRepository.FindById(Convert.ToInt32(values[2]));
            Location = location;

            Description = values[3];

            Language = (Language)Enum.Parse(typeof(Language), values[4]);

            MaxGuests = Convert.ToInt32(values[5]);

            Duration = Convert.ToInt32(values[6]);

            string[] ImagesSplit = values[7].Split(',');

            List<string> images = new List<string>();

            foreach (string image in ImagesSplit)
            {
                images.Add(image);
            }

            Images = images;

            GuideUsername = values[8];

        } 


    }
}
