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

        public Comment()
        {

        }

        public Comment(int commentId, Forum forum, string commenterUsername, string commenterType, string answer)
        {
            CommentId = commentId;
            Forum = forum;
            CommenterUsername = commenterUsername;
            CommenterType = commenterType;
            Answer = answer;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { CommentId.ToString(), Forum.ForumId.ToString(), CommenterUsername, CommenterType, Answer };
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
        }
    }
}
