using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IUserRepository
    {
        List<User> FindAll();

        void Update(string owner, string superType);

        void Save(List<User> allUsers);

        string FindSuperTypeByOwnerName(string ownerName);

        bool IsUserExist(string username);

        bool IsPasswordCorrect(string username, string password);

        string FindTypeByUsername(string username);

        User FindByUsername(string username);

    }
}
