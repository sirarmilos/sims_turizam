using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
    public class SearchAndShowTourRequestDTO
    {
        public string Country { get; set; }
        public string City { get; set; }

        public int? GuestNumber { get; set; }

        public string Language { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public SearchAndShowTourRequestDTO() { }

        //AccommodationName, Country, City, Type, MaxGuests, MinDaysReservation
        public SearchAndShowTourRequestDTO(string country, string city, int? guestNumber, string? language, DateTime startDate, DateTime endDate)
        {
            Country = country;
            City = city;
            GuestNumber = guestNumber;
            Language = language;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
