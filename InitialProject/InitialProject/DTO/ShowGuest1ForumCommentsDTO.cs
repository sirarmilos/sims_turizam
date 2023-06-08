using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class ShowGuest1ForumCommentsDTO
    {
        public int CommentId { get; set; }

        public string CommenterUsername { get; set; }

        public string CommenterType { get; set; }

        public string Answer { get; set; }

        public string Visited { get; set; }

        public string NumberOfReports { get; set; }

        public ShowGuest1ForumCommentsDTO()
        {

        }

        public ShowGuest1ForumCommentsDTO(int commentId, string commenterUsername, string commenterType, string answer, string visited, string numberOfReports)
        {
            CommentId = commentId;
            CommenterUsername = commenterUsername;
            CommenterType = commenterType;
            Answer = answer;
            Visited = visited;
            NumberOfReports = numberOfReports;
        }
    
        public ShowGuest1ForumCommentsDTO(Comment comment)
        {
            CommentId = comment.CommentId;
            CommenterUsername = comment.CommenterUsername;
            CommenterType = comment.CommenterType;
            Answer = comment.Answer;

            if(CommenterType.Equals("owner") == true)
            {
                Visited = string.Empty;
                NumberOfReports = string.Empty;
            }
            else
            {
                CommenterType = "guest";
                if(comment.Visited == true)
                {
                    Visited = "visited";
                    NumberOfReports = string.Empty;
                }
                else
                {
                    Visited = "unvisited";
                    NumberOfReports = comment.NumberOfReports.ToString() + " reports";
                }
            }
        }
    }
}
