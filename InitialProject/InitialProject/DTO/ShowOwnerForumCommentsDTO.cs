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
        
        public ShowOwnerForumCommentsDTO(OwnerComment ownerComment)
        {
            CommentId = ownerComment.CommentId;
            CommenterUsername = ownerComment.CommenterUsername;
            CommenterType = ownerComment.CommenterType;
            Answer = ownerComment.Answer;
            Visited = string.Empty;
            NumberOfReports = string.Empty;

            if(ownerComment.IsStillOwner == true)
            {
                IsStillOwner = "Accommodation owner at this location";
            }
            else
            {
                IsStillOwner = "Was Owner of the accommodation at this location";
            }
        }

        public ShowOwnerForumCommentsDTO(Guest1Comment guestComment)
        {
            CommentId = guestComment.CommentId;
            CommenterUsername = guestComment.CommenterUsername;
            CommenterType = guestComment.CommenterType;
            Answer = guestComment.Answer;

            if(guestComment.Visited == true)
            {
                Visited = "Visited this location";
            }
            else
            {
                Visited = "Not visited this location";
            }

            NumberOfReports = "Number user reports: " + guestComment.NumberOfReports.ToString();

            IsStillOwner = string.Empty;
        }
    }
}
