using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IRenovationRepository
    {
        void Save(List<Renovation> allRenovations);

        List<Renovation> FindAll();

        List<Renovation> FindByOwnerUsername(string ownerUsername);

        Renovation FindById(int renovationId);

        void RemoveById(int renovationId);

        List<Renovation> FindByAccommodationName(string accommodationName);

        int NextId();

        void Add(Renovation renovation);
    }
}
