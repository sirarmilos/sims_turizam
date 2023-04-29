using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class AccommodationStatisticsDataDTO
    {
        public int PeriodTime { get; set; }

        public int ReservationsCount { get; set; }

        public int CanceledReservationsCount { get; set; }

        public int RescheduledReservationCount { get; set; }

        public int RenovationRecommedationsCount { get; set; }

        public AccommodationStatisticsDataDTO()
        {

        }

        public AccommodationStatisticsDataDTO(int periodTime, int reservationsCount, int canceledReservationsCount, int rescheduledReservationCount, int renovationRecommedationsCount)
        {
            PeriodTime = periodTime;
            ReservationsCount = reservationsCount;
            CanceledReservationsCount = canceledReservationsCount;
            RescheduledReservationCount = rescheduledReservationCount;
            RenovationRecommedationsCount = renovationRecommedationsCount;
        }
    }
}
