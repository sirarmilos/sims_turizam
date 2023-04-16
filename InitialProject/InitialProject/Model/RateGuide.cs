using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    class RateGuide : ISerializable
    {
        public string UserId { get; set; }
        public string GuideId { get; set; }
        public int GuideKnowledge { get; set; }
        public int GuideLanguage { get; set; }
        public int TourExperience { get; set; }
        public List<string> Images { get; set; }

        public RateGuide() { }

        public RateGuide(string userId, string guideId, int guideKnowledge, int guideLanguage, int tourExperience, List<string> images)
        {
            UserId = userId;
            GuideId = guideId;
            GuideKnowledge = guideKnowledge;
            GuideLanguage = guideLanguage;
            TourExperience = tourExperience;
            Images = images;
        }

        public string[] ToCSV()
        {
            string imageToString = "";

            foreach(string image in Images)
            {
                imageToString += image;
                imageToString += ",";
            }

            string[] csvValues = { UserId.ToString(), GuideId.ToString(), GuideKnowledge.ToString(), GuideLanguage.ToString(), TourExperience.ToString(), imageToString.ToString() };

            return csvValues;
        }


        public void FromCSV(string[] values)
        {
            UserId = values[0];
            GuideId = values[1];
            GuideKnowledge = Convert.ToInt32(values[2]);
            GuideLanguage = Convert.ToInt32(values[3]);
            TourExperience = Convert.ToInt32(values[4]);

            string[] ImageSplit = values[5].Split(',');

            List<string> images = new List<string>();

            foreach(string image in ImageSplit)
            {
                images.Add(image);
            }

            Images = images;
        }

    }
}
