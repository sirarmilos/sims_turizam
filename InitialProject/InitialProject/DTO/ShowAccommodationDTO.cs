using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class ShowAccommodationDTO
    {
        public int Id { get; set; } // ostavi Id, iako ga ne prikazujes, trebace ti kada se izabere taj neki i klikne neko dugme, da ne moras opet da trazis taj accommodation, vidi i za ranije neke da li si mogao tako da uradis

        public string AccommodationName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Type { get; set; }

        public string MainImage { get; set; }

        public ShowAccommodationDTO()
        {

        }

        public ShowAccommodationDTO(Accommodation accommodation)
        {
            Id = accommodation.Id;
            AccommodationName = accommodation.AccommodationName;
            Country = accommodation.Location.Country;
            City = accommodation.Location.City;
            Address = accommodation.Location.Address;
            Type = accommodation.Type;
            MainImage = accommodation.Images[0];
        }
    }
}
