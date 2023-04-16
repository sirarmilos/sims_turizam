using InitialProject.IRepository;
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
        private readonly IUserRepository userRepository;

        private RateGuestsService rateGuestsService;

        public UserService()
        {
            userRepository = new UserRepository();
            // rateGuestsService = new RateGuestsService(Owner);
        }

        public void Update(string owner, string superType)
        {
            userRepository.Update(owner, superType);
        }

        public string FindSuperTypeByOwnerName(string ownerName)
        {
            return userRepository.FindSuperTypeByOwnerName(ownerName);
        }

        public bool IsUsernameExist(string username)
        {
            return userRepository.IsUserExist(username);
        }

        public bool IsPasswordCorrect(string username, string password)
        {
            return userRepository.IsPasswordCorrect(username, password);
        }

        public string FindTypeByUsername(string username)
        {
            return userRepository.FindTypeByUsername(username);
        }

        public int FindNumberOfUnratedGuests(string ownerUsername)
        {
            rateGuestsService = new RateGuestsService(ownerUsername);
            return rateGuestsService.FindNumberOfUnratedGuests(ownerUsername);
        }
    }
}
