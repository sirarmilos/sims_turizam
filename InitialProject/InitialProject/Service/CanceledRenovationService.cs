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
    public class CanceledRenovationService
    {
        private ICanceledRenovationRepository canceledRenovationRepository;

        public CanceledRenovationService()
        {
            canceledRenovationRepository = new CanceledRenovationRepository();
        }

        public void AddRenovation(Renovation renovation)
        {
            canceledRenovationRepository.Add(renovation);
        }
    }
}
