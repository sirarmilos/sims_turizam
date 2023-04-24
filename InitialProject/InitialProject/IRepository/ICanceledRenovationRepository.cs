using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ICanceledRenovationRepository
    {
        void Save(List<Renovation> allCanceledRenovations);

        List<Renovation> FindAll();

        void Add(Renovation renovation);
    }
}
