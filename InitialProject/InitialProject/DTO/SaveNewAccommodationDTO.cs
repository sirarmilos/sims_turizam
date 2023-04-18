using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class SaveNewAccommodationDTO
    {
        public string AccommodationName { get; set; }
        public string Owner { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinDaysReservation { get; set; }
        public int LeftCancelationDays { get; set; }
        public List<string> Images { get; set; }

        public SaveNewAccommodationDTO()
        {

        }

        public SaveNewAccommodationDTO(string accommodationName, string owner, string country, string city, string address, decimal latitude, decimal longitude, string type, int maxGuests, int minDaysReservation, int leftCancelationDays, ObservableCollection<string> images)
        {
            AccommodationName = accommodationName;
            Owner = owner;
            Country = country;
            City = city;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
            Type = type;
            MaxGuests = maxGuests;
            MinDaysReservation = minDaysReservation;
            LeftCancelationDays = leftCancelationDays;
            Images = new List<string>(images);
        }
    }
}
