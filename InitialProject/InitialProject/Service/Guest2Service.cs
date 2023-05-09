using InitialProject.Injector;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    internal class Guest2Service
    {
        private readonly IGuest2Repository guest2Repository;

        public Guest2Service()
        {
            guest2Repository = Injector.Injector.CreateInstance<IGuest2Repository>();
            //guest2Repository = new Guest2Repository();
        }

        public int FindAge(string username)
        {
            foreach (Guest2 guest2 in guest2Repository.FindAll())
            {
                if (guest2.User.Username.Equals(username))
                {
                    return guest2.Age;
                }
            }
            return 0;
        }



    }
}
