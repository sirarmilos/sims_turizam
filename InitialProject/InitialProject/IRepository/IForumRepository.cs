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

        void Save(List<Forum> allForums);

        void Add(Forum forum);

        int NextId();

        bool CheckIsUseful(int forumId);

        void MakeUseful(int forumId);
    }
}
