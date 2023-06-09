using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        public ComplexTour FindById(int id)
        {
            return FindAll().ToList().Find(x => x.Id == id);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }
            return FindAll().Max(c => c.Id) + 1;
        }

        public bool CheckIfGuideAcceptedPartOfComplex(int complexId, string guideUsername)
        {
            ComplexTour ct = FindById(complexId);
            if (ct == null)
                return true;
            foreach(Tour t in ct.Tour)
            {
                if (t.GuideUsername.Equals(guideUsername))
                    return false;
            }
            return true;
        }

        public void UpdateApprovedField(int complexId)
        {
            List<ComplexTour> complexTours = FindAll();
            foreach(ComplexTour ct in complexTours)
            {
                if(ct.Id == complexId)
                {
                    ct.Approved = true;
                    break;
                }
            }
            Save(complexTours);
        }

    }
}
