using InitialProject.DTO;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Model
{
    public class Location : ISerializable
    {
        public int Id { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Location() { }

        public Location(int id, string country, string city, string address, decimal latitude, decimal longitude)
        {
            Id = id;
            Country = country;
            City = city;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }

        public Location(int id, SaveNewAccommodationDTO saveNewAccommodationDTO)
        {
            Id = id;
            Country = saveNewAccommodationDTO.Country;
            City = saveNewAccommodationDTO.City;
            Address = saveNewAccommodationDTO.Address;
            Latitude = saveNewAccommodationDTO.Latitude;
            Longitude = saveNewAccommodationDTO.Longitude;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Country.ToString(), City.ToString(), Address.ToString(), Latitude.ToString(), Longitude.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
                Id = Convert.ToInt32(values[0]);
                Country = values[1];
                City = values[2];
                Address = values[3];
                Latitude = Convert.ToDecimal(values[4]);
                Longitude = Convert.ToDecimal(values[5]);
        }
    }
}
