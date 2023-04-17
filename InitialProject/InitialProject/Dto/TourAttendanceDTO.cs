using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
    class TourAttendanceDTO
    {
        public DateTime Date;
        public string GuideUsername;
        public List<Boolean> KeyPointActive;


        public TourAttendanceDTO() { }
        public TourAttendanceDTO(DateTime date, string guideUsername, List<bool> keyPointActive)
        {
            Date = date;
            GuideUsername = guideUsername;
            KeyPointActive = keyPointActive;
        }
    }
}
