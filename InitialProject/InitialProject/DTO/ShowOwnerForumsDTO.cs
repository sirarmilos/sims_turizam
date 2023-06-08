using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class ShowOwnerForumsDTO
    {
        public int ForumId { get; set; }

        public string CreatorUsername { get; set; }

        public string Location { get; set; }

        public string Question { get; set; }

        public string Useful { get; set; }

        public string Closed { get; set; }

        public ShowOwnerForumsDTO()
        {

        }

        public ShowOwnerForumsDTO(Forum forum)
        {
            ForumId = forum.ForumId;
            CreatorUsername = forum.CreatorUsername + " asked:";
            Location = forum.ForumLocationDTO.City + ", " + forum.ForumLocationDTO.Country;

            if(forum.Question.Length > 763)
            {
                Question = forum.Question.Substring(0, 763) + "...";
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
                Closed = "Closed topic";
            }
            else
            {
                Closed = string.Empty;
            }
        }
    }
}
