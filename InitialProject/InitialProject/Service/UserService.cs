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

        public UserService()
        {
            userRepository = new UserRepository();
        }

        public void UpdateUsers(string owner, string superType)
        {
            userRepository.UpdateUsers(owner, superType);
        }

        public string FindSuperTypeByOwnerName(string ownerName)
        {
            return userRepository.FindAllUsers().Find(x => x.Username.Equals(ownerName) == true).SuperType;
        }
    }
}
