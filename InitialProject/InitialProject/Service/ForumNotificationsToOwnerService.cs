using InitialProject.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using System.Text.RegularExpressions;


namespace InitialProject.Service
{
    public class ForumNotificationsToOwnerService
    {
        private readonly IForumNotificationsToOwnerRepository forumNotificationsToOwnerRepository;

        private readonly AccommodationService accommodationService;

        public ForumNotificationsToOwnerService()
        {
            forumNotificationsToOwnerRepository = Injector.Injector.CreateInstance<IForumNotificationsToOwnerRepository>();

            accommodationService = new AccommodationService();
        }
    
        public void CreateOwnersNotification(Forum forum)
        {
            List<Accommodation> allAccommodations = accommodationService.FindAll();
            List<string> eligibleOwners = new List<string>();

            string oneWordAccommodationCity;
            string oneWordAccommodationCountry;
            string oneWordForumCity = Regex.Replace(forum.ForumLocationDTO.City, @"\s+", " ");
            string oneWordForumCountry = Regex.Replace(forum.ForumLocationDTO.Country, @"\s+", " ");

            foreach (var accommodation in allAccommodations)
            {
                oneWordAccommodationCity = Regex.Replace(accommodation.Location.City, @"\s+", " ");
                oneWordAccommodationCountry = Regex.Replace(accommodation.Location.Country, @"\s+", " ");

                if (oneWordAccommodationCity.Equals(oneWordForumCity, StringComparison.OrdinalIgnoreCase)
                    && oneWordAccommodationCountry.Equals(oneWordForumCountry, StringComparison.OrdinalIgnoreCase))
                    if (!eligibleOwners.Contains(accommodation.OwnerUsername))
                        eligibleOwners.Add(accommodation.OwnerUsername);
            }

            eligibleOwners.ForEach(ownerUsername => forumNotificationsToOwnerRepository.Add(forum, ownerUsername));
        }
    }
}
