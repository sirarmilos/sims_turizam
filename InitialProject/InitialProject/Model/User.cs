using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class User : ISerializable
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Type { get; set; }

        public User() { }

        public User(string username, string password, string type)
        {
            Username = username;
            Password = password;
            Type = type;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Username.ToString(), Password.ToString(), Type.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Username = values[0];
            Password = values[1];
            Type = values[2];
        }
    }
}
