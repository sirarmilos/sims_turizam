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
    public class ForumNotificationsToOwnerRepository : IForumNotificationsToOwnerRepository
    {
        private ForumRepository forumRepository;

        private const string FilePathForumNotificationsToOwner = "../../../Resources/Data/forumnotificationstoowners.csv";

        private readonly Serializer<ForumNotificationsToOwner> forumNotificationsToOwnerSerializer;

        private List<ForumNotificationsToOwner> forumNotificationsToOwners;

        public ForumNotificationsToOwnerRepository()
        {
            forumNotificationsToOwnerSerializer = new Serializer<ForumNotificationsToOwner>();
        }

        public List<ForumNotificationsToOwner> FindAll()
        {
            forumRepository = new ForumRepository();

            forumNotificationsToOwners = forumNotificationsToOwnerSerializer.FromCSV(FilePathForumNotificationsToOwner);

            foreach (ForumNotificationsToOwner temporaryForumNotificationsToOwner in forumNotificationsToOwners.ToList())
            {
                temporaryForumNotificationsToOwner.Forum = forumRepository.FindById(temporaryForumNotificationsToOwner.Forum.ForumId);
            }

            return forumNotificationsToOwners;
        }

        public void Save(List<ForumNotificationsToOwner> allForumNotificationsToOwner)
        {
            forumNotificationsToOwnerSerializer.ToCSV(FilePathForumNotificationsToOwner, allForumNotificationsToOwner);
        }

        public void Add(Forum forum, string ownerUsername)
        {
            List<ForumNotificationsToOwner> allForumNotificationsToOwner = FindAll();
            ForumNotificationsToOwner forumNotificationToOwner = new ForumNotificationsToOwner(NextId(), forum, ownerUsername, false);
            allForumNotificationsToOwner.Add(forumNotificationToOwner);
            Save(allForumNotificationsToOwner);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }

            return FindAll().Max(x => x.ForumNotificationsToOwnerId) + 1;
        }
    }
}
