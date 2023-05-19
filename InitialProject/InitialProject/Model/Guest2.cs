using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Guest2 : ISerializable
    {
        public User User { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public int Age { get; set; }


        public Guest2() 
        { }

        public string[] ToCSV()
        {
            string[] csvValues = { User.Username.ToString(), Email.ToString(), Adress.ToString(), Age.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            User = new User() { Username = values[0] };
            Email = values[1];
            Adress = values[2];
            Age = Convert.ToInt32(values[3]);
        }
    }
}
