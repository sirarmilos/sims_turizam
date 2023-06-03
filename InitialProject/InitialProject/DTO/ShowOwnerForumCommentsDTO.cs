using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class ShowOwnerForumCommentsDTO
    {
        public int CommentId { get; set; }

        public string CommenterUsername { get; set; }

        public string CommenterType { get; set; }

        public string Answer { get; set; }

        public string Visited { get; set; }

        public string NumberOfReports { get; set; }

        public string IsStillOwner { get; set; }

        public ShowOwnerForumCommentsDTO()
        {

        }
        
        public ShowOwnerForumCommentsDTO(Comment comment)
        {
            CommentId = comment.CommentId;
            CommenterUsername = comment.CommenterUsername;
            CommenterType = comment.CommenterType;
            Answer = comment.Answer;

            if(CommenterType.Equals("owner") == true)
            {
                Visited = string.Empty;
                NumberOfReports = string.Empty;

                if(comment.IsStillOwner == true)
                {
                    IsStillOwner = "Accommodation owner at this location";
                }
                else
                {
                    IsStillOwner = "Was Owner of the accommodation at this location";
                }
            }
            else
            {
                if(comment.Visited == true)
                {
                    Visited = "Visited this location";
                    NumberOfReports = string.Empty;
                }
                else
                {
                    Visited = "Not visited this location";
                    NumberOfReports = "Number user reports: " + comment.NumberOfReports.ToString();
                }

                IsStillOwner = string.Empty;
            }
        }
    }
}
