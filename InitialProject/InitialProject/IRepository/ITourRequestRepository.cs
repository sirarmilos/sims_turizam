using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ITourRequestRepository
    {
        List<TourRequest> FindAll();

        TourRequest FindById(int id);

        void Save(List<TourRequest> requests);

        void Save(TourRequest tourRequest);

        int NextId();

        List<TourRequest> FindAllByCountry(List<TourRequest> allTourRequests, string name);

        List<TourRequest> FindAllByCity(List<TourRequest> allTourRequests, string name);

        List<TourRequest> FindAllByGuestsNumber(List<TourRequest> allTourRequests, int? quantity);

        List<TourRequest> FindAllByLanguage(List<TourRequest> allTourRequests, string? language);

        List<TourRequest> FindAllByDateRange(List<TourRequest> allTourRequests, DateTime filterStartDate, DateTime filterEndDate);
        
        List<TourRequest> FindAllByUser(string username);

        void Invalidate();
    }
}
