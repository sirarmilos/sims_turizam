using InitialProject.Dto;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ITourGuidenceRepository
    {
        List<TourGuidence> FindAll();

        void Save(List<TourGuidence> tourGuidences);

        void Update(TourGuidence tourGuidence);

        int NextId();

        TourGuidence FindById(int id);

        void SaveToFile(TourGuidence t);

        Tour GetMostVisitedAllTime();

        Tour GetMostVisitedByYear(int year);

        TourGuidence FindByTourAndDate(Tour tour, DateTime date);

        string FindGuide(int tourGuidenceId);

        TourAttendanceDTO FindTourAttendanceDTO(int tourReservationId);
        
    }
}