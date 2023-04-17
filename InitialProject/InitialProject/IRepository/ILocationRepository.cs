using InitialProject.Dto;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ILocationRepository
    {
        List<Location> FindAll();

        void Save(List<Location> allLocations);

        void Add(Location location);

        Location Save(LocationDto locationDto);

        int NextId();

        Location FindById(int locationId);
    }
}
