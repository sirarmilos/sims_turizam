using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class ShowGuest1ForumsDTO
    {
        public int ForumId { get; set; }

        public string CreatorUsername { get; set; }

        public string Location { get; set; }

        public string Question { get; set; }

        public string Useful { get; set; }

        public string Closed { get; set; }

        public ShowGuest1ForumsDTO()
        {

        }

        public ShowGuest1ForumsDTO(int forumId, string creatorUsername, string location, string question, string useful, string closed)
        {
            ForumId = forumId;
            CreatorUsername = creatorUsername;
            Location = location;
            Question = question;
            Useful = useful;
            Closed = closed;
        }   

        public ShowGuest1ForumsDTO(Forum forum)
        {
            ForumId = forum.ForumId;
            CreatorUsername = forum.CreatorUsername + " asked:";
            Location = forum.ForumLocationDTO.City + ", " + forum.ForumLocationDTO.Country;

            if(forum.Question.Length > 80)
            {
                Question = forum.Question.Substring(0, 80) + "...";
            }
            else
            {
                Question = forum.Question;
            }

            if (forum.IsUseful == true)
            {
                Useful = "useful";
            }
            else
            {
                Useful = string.Empty;
            }

            if(forum.Closed == true)
            {
                Closed = "(closed)";
            }
            else
            {
                Closed = string.Empty;
            }
        }
    }
}
