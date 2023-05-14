using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository
{
    public class SuperGuestRepository : ISuperGuestRepository
    {
        private const string FilePathSuperGuest = "../../../Resources/Data/superguests.csv";

        private readonly Serializer<SuperGuest> superGuestSerializer;

        private List<SuperGuest> superGuests;

        public SuperGuestRepository()
        {
            superGuestSerializer = new Serializer<SuperGuest>();
        }

        public List<SuperGuest> FindAll()
        {
            return superGuestSerializer.FromCSV(FilePathSuperGuest);
        }

        public List<SuperGuest> FindAllLatestSuperGuests() 
        {
            List<SuperGuest> latestSuperGuests = new List<SuperGuest>();

            foreach (SuperGuest temporarySuperGuest in FindAll())
            {
                SuperGuest latestSuperGuest = FindByGuest1(temporarySuperGuest.Guest1Username);

                foreach (SuperGuest superGuestCandidate in FindAllByGuest1(temporarySuperGuest.Guest1Username))
                {
                    if (latestSuperGuest.StartDate <= superGuestCandidate.StartDate) // = da bih dobio poslednjeg dodatog
                        latestSuperGuest = superGuestCandidate;
                }
            
                latestSuperGuests.Add(latestSuperGuest);
            }

            return latestSuperGuests;
        }

        public List<SuperGuest> FindAllByGuest1(string guest1Username)
        {
            return FindAll().FindAll(x => x.Guest1Username.Equals(guest1Username));
        }

        public SuperGuest FindByGuest1(string guest1Username)
        {
            return FindAll().Find(x => x.Guest1Username.Equals(guest1Username));
        }

        public SuperGuest FindLatestByGuest1(string guest1Username)
        {
            return FindAllLatestSuperGuests().Find(x => x.Guest1Username.Equals(guest1Username));
        }

        public void Add(SuperGuest superGuest)
        {
            superGuests = FindAll();
            superGuests.Add(superGuest);
            Save(superGuests);
        }

        public void Save(List<SuperGuest> superGuests)
        {
            superGuestSerializer.ToCSV(FilePathSuperGuest, superGuests);
        }

        public void Update(SuperGuest superGuest)
        {
            Save(RemoveLatestByGuest1Username(superGuest.Guest1Username));
            Add(superGuest);
        }

        public List<SuperGuest> RemoveLatestByGuest1Username(string guest1Username)
        {
            List<SuperGuest> superGuests = FindAll();
            SuperGuest latestSuperGuest = FindLatestByGuest1(guest1Username);
            superGuests.Remove(superGuests.Find(x => 
                x.Guest1Username.Equals(guest1Username) 
                && x.StartDate == latestSuperGuest.StartDate 
                && x.NumberOfBonusPoints == latestSuperGuest.NumberOfBonusPoints));
            return superGuests;
        }


    }
}
