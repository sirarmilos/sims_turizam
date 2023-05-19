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

        int NextId();

        TourGuidence FindById(int id);

        void SaveToFile(TourGuidence t);

        TourGuidence FindByTourAndDate(Tour tour, DateTime date);

        string FindGuide(int tourGuidenceId);

        TourAttendanceDTO FindTourAttendanceDTO(int tourReservationId);

        List<TourGuidence> FindGuideAll(string username);

        List<TourGuidence> FindFinishedByGuideUsername(int tourId, string username);

        List<TourGuidence> FindGuideTodayUpcomming(string guideUsername);

    }
}