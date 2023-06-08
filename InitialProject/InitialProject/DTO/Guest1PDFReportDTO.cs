using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class Guest1PDFReportDTO
    {
        public string OwnerUsername { get; set; }

        public decimal CleanlinessAverage { get; set; }

        public decimal FollowRulesAverage { get; set; }

        public decimal BehaviorAverage { get; set; }

        public decimal CommunicativenessAverage { get; set; }

        public decimal AllAverage { get; set; }

        public int NumberOfReservations { get; set; }

        public Guest1PDFReportDTO()
        {

        }

        //public Guest1PDFReportDTO(Accommodation accommodation)
        //{
        //    AccommodationName = accommodation.AccommodationName;
        //    Country = accommodation.Location.Country;
        //    City = accommodation.Location.City;
        //    Address = accommodation.Location.Address;
        //    AverageCleanliness = 0;
        //    AverageStaff = 0;
        //    AverageComfort = 0;
        //    AverageValueForMoney = 0;
        //    AverageAll = 0;
        //    NumberOfReviews = 0;
        //}
    }
}
