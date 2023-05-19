using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IRenovationRecommendationRepository
    {
        List<RenovationRecommendation> FindAll();

        List<RenovationRecommendation> FindByAccommodationId(int accommodationId);

        int FindAccommodationRenovationRecommedationCountByYear(int accommodationId, int year);

        List<RenovationRecommendation> FindAccommodationRenovationRecommedationsByYear(int accommodationId, int year);




        void Add(RenovationRecommendation renovationRecommendation);

        void Save(List<RenovationRecommendation> allRenovationRecommedations);

    }
}
