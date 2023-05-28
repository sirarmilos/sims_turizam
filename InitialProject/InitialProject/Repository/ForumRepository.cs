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
    public class ForumRepository : IForumRepository
    {
        private const string FilePathForum = "../../../Resources/Data/forums.csv";

        private readonly Serializer<Forum> forumSerializer;

        private List<Forum> forums;

        public ForumRepository()
        {
            forumSerializer = new Serializer<Forum>();
        }

        public List<Forum> FindAll()
        {
            return forumSerializer.FromCSV(FilePathForum);
        }

        public Forum FindById(int forumId)
        {
            return FindAll().ToList().Find(x => x.ForumId == forumId);
        }
    }
}
