using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ISuperGuestRepository
    {
        List<SuperGuest> FindAll();

        SuperGuest FindByGuest1(string guest1Username);

        void Add(SuperGuest superGuest);

        void Save(List<SuperGuest> superGuests);

        List<SuperGuest> FindAllLatestSuperGuests();

        List<SuperGuest> FindAllByGuest1(string guest1Username);

        void Update(SuperGuest superGuest);

        List<SuperGuest> RemoveLatestByGuest1Username(string guest1Username);

        SuperGuest FindLatestByGuest1(string guest1Username);

    }
}
