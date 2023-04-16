using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
    public class LocationDto
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public LocationDto() { }

        public LocationDto(string country, string city, string address, decimal latitude, decimal longitude)
        {
            Country = country;
            City = city;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
