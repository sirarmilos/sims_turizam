using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
    public class TourAttendanceDTO
    {
        public DateTime Date;
        public string GuideUsername;
        public List<TourKeyPoint> TourKeyPoints;


        public TourAttendanceDTO() 
        {
            this.Date = DateTime.Now;
            GuideUsername = string.Empty;
            TourKeyPoints = new List<TourKeyPoint>();
        }
       
        public TourAttendanceDTO(DateTime date, string guideUsername, List<TourKeyPoint> tourKeyPoints)
        {
            Date = date;
            GuideUsername = guideUsername;
            TourKeyPoints = tourKeyPoints;
        }


    }
}
