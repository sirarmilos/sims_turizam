using InitialProject.DTO;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class ForumNotificationsToOwner : ISerializable
    {
        public int ForumNotificationsToOwnerId { get; set; }

        public Forum Forum { get; set; }

        public string OwnerUsername { get; set; }

        public bool ViewedByOwner { get; set; }

        public ForumNotificationsToOwner()
        {

        }

        public ForumNotificationsToOwner(int forumNotificationsToOwnerId, Forum forum, string ownerUsername, bool viewedByOwner)
        {
            ForumNotificationsToOwnerId = forumNotificationsToOwnerId;
            Forum = forum;
            OwnerUsername = ownerUsername;
            ViewedByOwner = viewedByOwner;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { ForumNotificationsToOwnerId.ToString(), Forum.ForumId.ToString(), OwnerUsername, ViewedByOwner.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            // return if the file was empty
            if (string.IsNullOrWhiteSpace(values[0])) return;

            ForumNotificationsToOwnerId = Convert.ToInt32(values[0]);
            Forum = new Forum() { ForumId = Convert.ToInt32(values[1]) };
            OwnerUsername = values[2];
            ViewedByOwner = Convert.ToBoolean(values[3]);
        }
    }
}
