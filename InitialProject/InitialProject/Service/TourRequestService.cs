using InitialProject.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourRequestService
    {
        private readonly ITourRequestRepository tourRequestRepository;

        public TourRequestService()
        {
            tourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();
        }
    }
}
