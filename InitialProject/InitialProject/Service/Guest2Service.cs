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
        private readonly Guest2Repository guest2Repository;

        public Guest2Service()
        {
            guest2Repository = new Guest2Repository();
        }



    }
}
