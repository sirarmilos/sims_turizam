using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class ComplexTourRepository : IComplexTourRepository
    {
        private const string FilePathComplexTour = "../../../Resources/Data/complextours.csv";

        private readonly Serializer<ComplexTour> complexTourSerializer;

        private List<ComplexTour> complexTours;

        public ComplexTourRepository()
        {
            complexTourSerializer = new Serializer<ComplexTour>();
        }

        public List<ComplexTour> FindAll()
        {
            return complexTourSerializer.FromCSV(FilePathComplexTour);
        }

        public void Save(List<ComplexTour> tours)
        {
            complexTourSerializer.ToCSV(FilePathComplexTour, tours);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }
            return FindAll().Max(c => c.Id) + 1;
        }
    }
}
