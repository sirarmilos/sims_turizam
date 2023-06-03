using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Comment : ISerializable
    {
        public int CommentId { get; set; }

        public Forum Forum { get; set; }

        public string CommenterUsername { get; set; }

        public string CommenterType { get; set; }

        public string Answer { get; set; }

        public bool IsStillOwner { get; set; }

        public bool Visited { get; set; }

        public int NumberOfReports { get; set; }

        public Comment()
        {

        }

        public Comment(int commentId, Forum forum, string commenterUsername, string commenterType, string answer, bool isStillOwner, bool visited, int numberOfReports)
        {
            CommentId = commentId;
            Forum = forum;
            CommenterUsername = commenterUsername;
            CommenterType = commenterType;
            Answer = answer;
            IsStillOwner = isStillOwner;
            Visited = visited;
            NumberOfReports = numberOfReports;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { CommentId.ToString(), Forum.ForumId.ToString(), CommenterUsername, CommenterType, Answer, IsStillOwner.ToString(), Visited.ToString(), NumberOfReports.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            // return if the file was empty
            if (string.IsNullOrWhiteSpace(values[0])) return;

            CommentId = Convert.ToInt32(values[0]);
            Forum = new Forum() { ForumId = Convert.ToInt32(values[1]) };
            CommenterUsername = values[2];
            CommenterType = values[3];
            Answer = values[4];
            IsStillOwner = Convert.ToBoolean(values[5]);
            Visited = Convert.ToBoolean(values[6]);
            NumberOfReports = Convert.ToInt32(values[7]);
        }
    }
}
