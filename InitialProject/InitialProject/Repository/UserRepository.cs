﻿using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    class UserRepository
    {
        private const string FilePathUser = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> userSerializer;

        private List<User> users;

        public UserRepository()
        {
            userSerializer = new Serializer<User>();
            users = userSerializer.FromCSV(FilePathUser);
        }

        public List<User> FindAllUsers()
        {
            return users;
        }

        public void UpdateUsers(List<User> allUsers)
        {
            userSerializer.ToCSV(FilePathUser, allUsers);
        }

        public string LoginUser(string username, string password)
        {
            User user = new User();
            if (IsUserExist(username) == false)
            {
                return "Greska";
            }

            user = IsPasswordCorect(username, password);

            if (user == null)
            {
                return "Greska";
            }
            else
            {
                return user.Type;
            }
        }

        private bool IsUserExist(string username)
        {
            foreach (User temporaryUser in users)
            {
                if (temporaryUser.Username.Equals(username) == true)
                {
                    return true;
                }
            }

            return false;
        }

        private User IsPasswordCorect(string username, string password)
        {
            foreach (User temporaryUser in users)
            {
                if (temporaryUser.Username.Equals(username) == true)
                {
                    if (temporaryUser.Password.Equals(password) == true)
                    {
                        return temporaryUser;
                    }

                    return null;
                }
            }

            return null;
        }


    }
}
