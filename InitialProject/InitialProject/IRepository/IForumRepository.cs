using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IForumRepository
    {
        List<Forum> FindAll();

        Forum FindById(int forumId);
    }
}
