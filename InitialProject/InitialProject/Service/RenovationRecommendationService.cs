using InitialProject.DTO;
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
    public class RenovationRecommendationService
    {
        private IRenovationRecommendationRepository renovationRecommedationRepository;

        public RenovationRecommendationService()
        {
            renovationRecommedationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
        }

        public int FindAccommodationRenovationRecommedationCountByYear(int accommodationId, int year)
        {
            return renovationRecommedationRepository.FindAccommodationRenovationRecommedationCountByYear(accommodationId, year);
        }

        public List<int> FindAccommodationRenovationRecommedationCountByMonth(int accommodationId, int year)
        {
            List<int> renovationRecommendationCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            List<RenovationRecommendation> yearRenovationRecommendations = renovationRecommedationRepository.FindAccommodationRenovationRecommedationsByYear(accommodationId, year);

            foreach (RenovationRecommendation temporaryRenovationRecommendation in yearRenovationRecommendations.ToList())
            {
                renovationRecommendationCount[temporaryRenovationRecommendation.CreationDate.Month - 1]++;
            }

            return renovationRecommendationCount;
        }





        public void SaveRenovationRecommendation(RenovationRecommendation renovationRecommendation)
        {
            renovationRecommedationRepository.Add(renovationRecommendation);
        }

    }
}
