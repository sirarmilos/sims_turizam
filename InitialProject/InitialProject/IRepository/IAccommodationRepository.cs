using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IAccommodationRepository
    {
        List<Accommodation> FindAll();

        void Save(List<Accommodation> allAccommodations);

        void Add(Accommodation accommodation);

        bool IsAccommodationExist(string accommodationName);

        int NextId();

        Accommodation FindAccommodationByAccommodationId(int accommodationId);

        List<Accommodation> FindAll(string accommodationName, string country, string city, string type, int? maxGuests, int? minDaysReservation);

        bool AreReservationDaysContained(int? minDaysReservation, out List<Accommodation> minDaysReservationResults);

        bool IsGuestsNumberContained(int? maxGuests, out List<Accommodation> maxGuestsResults);

        bool IsTypeContained(string type, out List<Accommodation> typeResults);

        bool IsCityContained(string city, out List<Accommodation> cityResults);

        bool IsCountryContained(string country, out List<Accommodation> countryResults);

        bool IsNameContained(string accommodationName, out List<Accommodation> accommodationNameResults);

        List<Accommodation> FindAllByAccommodation(string name);

        List<Accommodation> FindAllByCountry(string name);

        List<Accommodation> FindAllByCity(string name);

        List<Accommodation> FindAllByMaxGuestsNumber(int? quantity);

        List<Accommodation> FindAllByType(string name);

        List<Accommodation> FindAllAboveMinReservationDays(int? minDaysReservation);
    }
}
