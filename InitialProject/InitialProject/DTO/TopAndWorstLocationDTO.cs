using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.DTO
{
    public class TopAndWorstLocationDTO
    {
        public string Location { get; set; }

        public decimal TotalBusyPercentage { get; set; }

        public TopAndWorstLocationDTO()
        {

        }

        public TopAndWorstLocationDTO(Location location, decimal totalBusyPercentage)
        {
            Location = location.Country + ", " + location.City;
            TotalBusyPercentage = totalBusyPercentage;
        }
    }
}
