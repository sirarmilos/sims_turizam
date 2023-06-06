using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.Model
{
    public class Guide : ISerializable
    {

        public User User { get; set; }

        public Boolean IsResigned { get; set; } 

        public Guide() { } 

        public Guide(User user, Boolean isResigned)
        {
            User = user;
            IsResigned = isResigned;
        }


        public void FromCSV(string[] values)
        {
            User = new User() { Username = values[0] };
            IsResigned = Convert.ToBoolean(values[1]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { User.Username.ToString(), IsResigned.ToString()};
            return csvValues;
        }
    }
}
