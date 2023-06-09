using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IComplexTourRepository
    {
        List<ComplexTour> FindAll();

        void Save(List<ComplexTour> tours);

        int NextId();

        ComplexTour FindById(int id);

        bool CheckIfGuideAcceptedPartOfComplex(int complexId, string guideUsername);
    }
}
