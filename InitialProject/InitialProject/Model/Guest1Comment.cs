using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Guest1Comment : Comment
    {
        public bool Visited { get; set; }

        public int NumberOfReports { get; set; }

        public Guest1Comment() : base()
        {

        }

        public Guest1Comment(int commentId, Forum forum, string commenterUsername, string commenterType, string answer, bool visited, int numberOfReports) : base(commentId, forum, commenterUsername, commenterType, answer)
        {
            Visited = visited;
            NumberOfReports = numberOfReports;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { CommentId.ToString(), Forum.ForumId.ToString(), CommenterUsername, CommenterType, Answer, Visited.ToString(), NumberOfReports.ToString(), "x" };
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
            Visited = Convert.ToBoolean(values[5]);
            NumberOfReports = Convert.ToInt32(values[6]);
        }
    }
}
