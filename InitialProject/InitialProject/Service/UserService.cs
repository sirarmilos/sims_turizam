using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class UserService
    {
        private readonly UserRepository userRepository;

        public List<User> AllUsers
        {
            get;
            set;
        }

        public UserService()
        {
            userRepository = new UserRepository();

            ListInitialization();
        }

        public void ListInitialization()
        {
            AllUsers = new List<User>();
        }

        public void UpdateUsers(string owner, string superType)
        {
            AllUsers = userRepository.FindAllUsers();

            AllUsers.Where(x => x.Username.Equals(owner) == true).SetValue(x => x.SuperType = superType).ToList();

            userRepository.UpdateUsers(AllUsers);
        }

        public string FindSuperTypeByOwnerName(string ownerName)
        {
            AllUsers = userRepository.FindAllUsers();

            return AllUsers.Find(x => x.Username.Equals(ownerName) == true).SuperType;
        }
    }
}
