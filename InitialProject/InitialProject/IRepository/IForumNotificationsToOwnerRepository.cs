using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IForumNotificationsToOwnerRepository
    {
        void Save(List<ForumNotificationsToOwner> allForumNotificationsToOwner);

        void Add(Forum forum, string ownerUsername);
        int NextId();
    }

}
