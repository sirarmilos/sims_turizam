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

        public void Update(string owner, string superType)
        {
            userRepository.Update(owner, superType);
        }

        public string FindSuperTypeByOwnerName(string ownerName)
        {
            return userRepository.FindSuperTypeByOwnerName(ownerName);
        }
    }
}
