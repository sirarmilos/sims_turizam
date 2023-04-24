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
    public class CanceledRenovationRepository : ICanceledRenovationRepository
    {
        private AccommodationRepository accommodationRepository;

        private const string FilePathCanceledRenovation = "../../../Resources/Data/canceledrenovations.csv";

        private readonly Serializer<Renovation> canceledRenovationSerializer;

        private List<Renovation> canceledRenovations;

        public CanceledRenovationRepository()
        {
            canceledRenovationSerializer = new Serializer<Renovation>();
        }

        public void Save(List<Renovation> allCanceledRenovations)
        {
            canceledRenovationSerializer.ToCSV(FilePathCanceledRenovation, allCanceledRenovations);
        }

        public List<Renovation> FindAll()
        {
            accommodationRepository = new AccommodationRepository();

            canceledRenovations = canceledRenovationSerializer.FromCSV(FilePathCanceledRenovation);

            foreach (Renovation temporaryRenovation in canceledRenovations)
            {
                temporaryRenovation.Accommodation = accommodationRepository.FindById(temporaryRenovation.Accommodation.Id);
            }

            return canceledRenovations;
        }

        public void Add(Renovation renovation)
        {
            List<Renovation> allCanceledRenovations = FindAll();
            canceledRenovations.Add(renovation);
            Save(allCanceledRenovations);
        }
    }
}
