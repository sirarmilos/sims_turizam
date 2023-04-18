using InitialProject.Dto;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ITourRepository
    {

        List<Tour> FindAll();

        Tour FindById(int id);

        Tour FindByName(string id);

        Tour Save(TourDto tourDto);

        int NextId();

        List<int> GetGuestNumber(int tourId);

    }
}
