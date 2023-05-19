using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class ShowStatisticsAccommodationDTO
    {
        public int Id { get; set; } // ostavi Id, iako ga ne prikazujes, trebace ti kada se izabere taj neki i klikne neko dugme, da ne moras opet da trazis taj accommodation, vidi i za ranije neke da li si mogao tako da uradis

        public string AccommodationName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Type { get; set; }

        public int MaxGuests { get; set; }

        public int MinDaysReservation { get; set; }

        public int LeftCancelationDays { get; set; }

        public ObservableCollection<string> Images { get; set; }

        public ShowStatisticsAccommodationDTO()
        {

        }

        public ShowStatisticsAccommodationDTO(Accommodation accommodation)
        {
            Id = accommodation.Id;
            AccommodationName = accommodation.AccommodationName;
            Country = accommodation.Location.Country;
            City = accommodation.Location.City;
            Address = accommodation.Location.Address;
            Latitude = accommodation.Location.Latitude;
            Longitude = accommodation.Location.Longitude;
            Type = accommodation.Type;
            MaxGuests = accommodation.MaxGuests;
            MinDaysReservation = accommodation.MinDaysReservation;
            LeftCancelationDays = accommodation.LeftCancelationDays;
            Images = new ObservableCollection<string>(accommodation.Images);
        }
    }
}
