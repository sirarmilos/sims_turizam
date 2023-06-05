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

        public void Save(List<Forum> allForums)
        {
            forumSerializer.ToCSV(FilePathForum, allForums);
        }

        public void Add(Forum forum)
        {
            List<Forum> allForums = FindAll();
            allForums.Add(forum);
            Save(allForums);
        }

        public int NextId()
        {
            if (FindAll().Count < 1)
            {
                return 1;
            }

            return FindAll().Max(x => x.ForumId) + 1;
        }

        public bool CheckIsUseful(int forumId)
        {
            return FindById(forumId).IsUseful == true;
        }

        public void MakeUseful(int forumId)
        {
            List<Forum> allForums = FindAll();
            allForums.ToList().Where(x => x.ForumId == forumId).ToList().SetValue(x => x.IsUseful = true);
            Save(allForums);
        }
    }
}
