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
    public class UserRepository : IUserRepository
    {
        private const string FilePathUser = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> userSerializer;

        public UserRepository()
        {
            userSerializer = new Serializer<User>();
        }

        public List<User> FindAll()
        {
            return userSerializer.FromCSV(FilePathUser);
        }

        public void Update(string owner, string superType)
        {
            List<User> allUsers = FindAll();
            allUsers.Where(x => x.Username.Equals(owner) == true).SetValue(x => x.SuperType = superType).ToList();
            Save(allUsers);
        }

        public void Save(List<User> allUsers)
        {
            userSerializer.ToCSV(FilePathUser, allUsers);
        }

        public string FindSuperTypeByOwnerName(string ownerName)
        {
            return FindAll().Find(x => x.Username.Equals(ownerName) == true).SuperType;
        }

        public bool IsUserExist(string username)
        {
            return FindAll().Exists(x => x.Username.Equals(username) == true);
        }

        public bool IsPasswordCorrect(string username, string password)
        {
            return FindAll().Exists(x => x.Username.Equals(username) == true && x.Password.Equals(password) == true);
        }

        public string FindTypeByUsername(string username)
        {
            return FindAll().Find(x => x.Username.Equals(username) == true).Type;
        }

        public User FindByUsername(string username)
        {
            User user = new User();

            List<User> users = FindAll();

            foreach (User temporaryUser in users)
            {
                if (temporaryUser.Username.Equals(username))
                {
                    user = temporaryUser;
                }
            }

            return user;
        }
    }
}