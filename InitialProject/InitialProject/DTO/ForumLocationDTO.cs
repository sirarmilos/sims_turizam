using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class ForumLocationDTO
    {
        public string Country { get; set; }

        public string City { get; set; }

        public ForumLocationDTO()
        {

        }

        public ForumLocationDTO(string country, string city)
        {
            Country = country;
            City = city;
        }
    }
}
