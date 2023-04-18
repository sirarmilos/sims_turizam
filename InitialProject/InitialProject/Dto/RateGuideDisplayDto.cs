using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
    public class RateGuideDisplayDto
    {
        public string UserId { get; set; }
        public int tourGuidenceId { get; set; }
        public int GuideKnowledge { get; set; }
        public int GuideLanguage { get; set; }
        public int TourExperience { get; set; }
        public string Comment { get; set; }
        public int OrderNumberKeyPoint { get; set; }
        public Boolean Valid { get; set; }


        public RateGuideDisplayDto() { }

        public RateGuideDisplayDto(string userId, int tourGuidenceId, int guideKnowledge, int guideLanguage, int tourExperience, string comment)
        {
            UserId = userId;
            this.tourGuidenceId = tourGuidenceId;
            GuideKnowledge = guideKnowledge;
            GuideLanguage = guideLanguage;
            TourExperience = tourExperience;
            Comment = comment;
            Valid = true;
        }


    }
}
