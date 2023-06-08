using InitialProject.DTO;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Forum : ISerializable
    {
        public int ForumId { get; set; }

        public string CreatorUsername { get; set; }

        public ForumLocationDTO ForumLocationDTO { get; set; }

        public string Question { get; set; }

        public bool IsUseful { get; set; }

        public bool Closed { get; set; }

        public Forum()
        {

        }

        public Forum(int forumId, string creatorUsername, ForumLocationDTO forumLocationDTO, string question, bool isUseful, bool closed)
        {
            ForumId = forumId;
            CreatorUsername = creatorUsername;
            ForumLocationDTO = forumLocationDTO;
            Question = question;
            IsUseful = isUseful;
            Closed = closed;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { ForumId.ToString(), CreatorUsername, ForumLocationDTO.Country, ForumLocationDTO.City, Question, IsUseful.ToString(), Closed.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            // return if the file was empty
            if (string.IsNullOrWhiteSpace(values[0])) return;

            ForumId = Convert.ToInt32(values[0]);
            CreatorUsername = values[1];
            ForumLocationDTO = new ForumLocationDTO() { Country = values[2], City = values[3] };
            Question = values[4];
            IsUseful = Convert.ToBoolean(values[5]);
            Closed = Convert.ToBoolean(values[6]);
        }
    }
}
