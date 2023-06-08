using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class CreateForumDTO
    {
        public string Guest1Username { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Question { get; set; }

        public CreateForumDTO()
        {

        }

        public CreateForumDTO(string guest1Username, string city, string country, string question)
        {
            Guest1Username = guest1Username;
            City = city;    
            Country = country;
            Question = question;    
        }
    }
}
