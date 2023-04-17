using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    class RateGuide : ISerializable
    {
        public string UserId { get; set; }
        public string GuideId { get; set; }
        public int tourGuidenceId { get; set; }
        public int GuideKnowledge { get; set; }
        public int GuideLanguage { get; set; }
        public int TourExperience { get; set; }
        public string Comment { get; set; }
        public List<string> Images { get; set; }
        public Boolean Valid { get; set; }

        public RateGuide() { }

        public RateGuide(string userId, string guideId, int tourGuidenceId, int guideKnowledge, int guideLanguage, int tourExperience, string comment, List<string> images)
        {
            UserId = userId;
            GuideId = guideId;
            this.tourGuidenceId = tourGuidenceId;
            GuideKnowledge = guideKnowledge;
            GuideLanguage = guideLanguage;
            TourExperience = tourExperience;
            Comment = comment;
            Images = images;
            Valid = true;
        }

        public string[] ToCSV()
        {
            string imageToString = "";

            foreach(string image in Images)
            {
                imageToString += image;
                imageToString += ",";
            }

            if (Images.Count>=2)
            {
                imageToString = imageToString.Substring(0, imageToString.Length - 2);
            }

            string[] csvValues = { UserId.ToString(), GuideId.ToString(), tourGuidenceId.ToString(), GuideKnowledge.ToString(), GuideKnowledge.ToString(), TourExperience.ToString(), Comment.ToString(), imageToString.ToString(), Valid.ToString() };

            return csvValues;
        }


        public void FromCSV(string[] values)
        {
            UserId = values[0];
            GuideId = values[1];   
            tourGuidenceId = Convert.ToInt32(values[2]);   
            GuideKnowledge = Convert.ToInt32(values[3]);
            GuideLanguage = Convert.ToInt32(values[4]);
            TourExperience = Convert.ToInt32(values[5]);
            Comment = values[6];

            string[] ImagesSplit = values[7].Split(',');

            List<string> images = new List<string>();

            foreach (string image in ImagesSplit)
            {
                images.Add(image);
            }

            Images = images;

            Valid = Convert.ToBoolean(values[8]);
        }

    }
}
