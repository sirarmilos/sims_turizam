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
    class Guest2 : ISerializable
    {
        private readonly UserRepository userRepository = new UserRepository();
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
            User = userRepository.FindByUsername(values[0]);
            Email = values[1];
            Adress = values[2];
            Age = Convert.ToInt32(values[3]);
        }
    }
}
