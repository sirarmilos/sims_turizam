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

        Accommodation FindById(int accommodationId);

        List<Accommodation> FindAllByAccommodationName(List<Accommodation> allAccommodations, string name);

        List<Accommodation> FindAllByCountry(List<Accommodation> allAccommodations, string name);

        List<Accommodation> FindAllByCity(List<Accommodation> allAccommodations, string name);

        List<Accommodation> FindAllByMaxGuestsNumber(List<Accommodation> allAccommodations, int? quantity);

        List<Accommodation> FindAllByType(List<Accommodation> allAccommodations, string name);

        List<Accommodation> FindAllAboveMinReservationDays(List<Accommodation> allAccommodations, int? minDaysReservation);
    }
}
