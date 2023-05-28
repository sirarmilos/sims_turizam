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
    }
}
