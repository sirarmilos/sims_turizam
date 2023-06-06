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
    public class RenovationRepository : IRenovationRepository
    {
        private AccommodationRepository accommodationRepository;

        private const string FilePathRenovation = "../../../Resources/Data/renovations.csv";

        private readonly Serializer<Renovation> renovationSerializer;

        private List<Renovation> renovations;

        public RenovationRepository()
        {
            renovationSerializer = new Serializer<Renovation>();
        }

        public void Save(List<Renovation> allRenovations)
        {
            renovationSerializer.ToCSV(FilePathRenovation, allRenovations);
        }

        public List<Renovation> FindAll()
        {
            accommodationRepository = new AccommodationRepository();

            renovations = renovationSerializer.FromCSV(FilePathRenovation);

            foreach(Renovation temporaryRenovation in renovations)
            {
                temporaryRenovation.Accommodation = accommodationRepository.FindById(temporaryRenovation.Accommodation.Id);
            }

            return renovations;
        }

        public List<Renovation> FindByOwnerUsername(string ownerUsername)
        {
            return FindAll().ToList().FindAll(x => x.Accommodation.OwnerUsername.Equals(ownerUsername) == true && x.Accommodation.Removed == false);
        }

        public Renovation FindById(int renovationId)
        {
            return FindAll().ToList().Find(x => x.RenovationId == renovationId);
        }

        public void RemoveById(int renovationId)
        {
            List<Renovation> allRenovations = FindAll();
            allRenovations.Remove(allRenovations.Find(x => x.RenovationId == renovationId));
            Save(allRenovations);
        }

        public List<Renovation> FindByAccommodationName(string accommodationName)
        {
            return FindAll().ToList().FindAll(x => x.Accommodation.AccommodationName.Equals(accommodationName) == true);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }
            return FindAll().Max(c => c.RenovationId) + 1;
        }

        public void Add(Renovation renovation)
        {
            List<Renovation> allRenovations = FindAll();
            allRenovations.Add(renovation);
            Save(allRenovations);
        }
    }
}
