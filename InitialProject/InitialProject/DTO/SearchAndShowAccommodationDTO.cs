using System.Collections.Generic;
using InitialProject.Model;

namespace InitialProject.DTO
{
    public class SearchAndShowAccommodationDTO
    {
        public string AccommodationName { get; set; }

        public string Country { get; set; }
        public string City { get; set; }

        public string Type { get; set; }

        public int? MaxGuests { get; set; }

        public int? MinDaysReservation { get; set; }

        public SearchAndShowAccommodationDTO() { }

        //AccommodationName, Country, City, Type, MaxGuests, MinDaysReservation
        public SearchAndShowAccommodationDTO(string accommodationName, string country, string city, string type, int? maxGuests, int? minDaysReservation)
        {
            AccommodationName = accommodationName;
            Country = country;
            City = city;
            Type = type;
            MaxGuests = maxGuests;
            MinDaysReservation = minDaysReservation;
        }

    }
}
