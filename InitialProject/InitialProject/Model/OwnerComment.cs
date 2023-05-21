using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class OwnerComment : Comment
    {
        public bool IsStillOwner { get; set; }

        public OwnerComment() : base()
        {

        }

        public OwnerComment(int commentId, Forum forum, string commenterUsername, string commenterType, string answer, bool isStillOwner) : base(commentId, forum, commenterUsername, commenterType, answer)
        {
            IsStillOwner = isStillOwner;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { CommentId.ToString(), Forum.ForumId.ToString(), CommenterUsername, CommenterType, Answer, "x", "x", IsStillOwner.ToString() };
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
            IsStillOwner = Convert.ToBoolean(values[7]);
        }
    }
}
