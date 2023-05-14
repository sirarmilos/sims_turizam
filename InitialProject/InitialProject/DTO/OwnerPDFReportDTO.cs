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
    public class OwnerPDFReportDTO
    {
        public string AccommodationName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public decimal AverageCleanliness { get; set; }

        public decimal AverageStaff { get; set; }

        public decimal AverageComfort { get; set; }

        public decimal AverageValueForMoney { get; set; }

        public decimal AverageAll { get; set; }

        public int NumberOfReviews { get; set; }

        public OwnerPDFReportDTO()
        {

        }

        public OwnerPDFReportDTO(Accommodation accommodation)
        {
            AccommodationName = accommodation.AccommodationName;
            Country = accommodation.Location.Country;
            City = accommodation.Location.City;
            Address = accommodation.Location.Address;
            AverageCleanliness = 0;
            AverageStaff = 0;
            AverageComfort = 0;
            AverageValueForMoney = 0;
            AverageAll = 0;
            NumberOfReviews = 0;
        }
    }
}
