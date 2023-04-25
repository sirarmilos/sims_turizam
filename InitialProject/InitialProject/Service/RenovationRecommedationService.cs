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
    public class RenovationRecommedationService
    {
        private IRenovationRecommedationRepository renovationRecommedationRepository;

        public RenovationRecommedationService()
        {
            renovationRecommedationRepository = new RenovationRecommedationRepository();
        }

        /*public List<int> FindAccommodationRenovationRecommedationsYears(int accommodationId)
        {
            List<int> yearsRenovationRecommedations = new List<int>();

            List<RenovationRecommedation> renovationRecommedations = renovationRecommedationRepository.FindByAccommodationId(accommodationId);

            foreach (RenovationRecommedation temporaryrenovationRecommedation in renovationRecommedations.ToList())
            {
                int year = temporaryrenovationRecommedation.StartDate.Year;
                if (yearsRenovationRecommedations.Exists(x => x == year) == false)
                {
                    yearsRenovationRecommedations.Add(year);
                }
            }

            return yearsRenovationRecommedations;
        }*/
    }
}
