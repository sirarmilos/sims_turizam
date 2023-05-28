using InitialProject.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class ForumNotificationsToOwnerService
    {
        private readonly IForumNotificationsToOwnerRepository forumNotificationsToOwnerRepository;

        public ForumNotificationsToOwnerService()
        {
            forumNotificationsToOwnerRepository = Injector.Injector.CreateInstance<IForumNotificationsToOwnerRepository>();
        }
    }
}
