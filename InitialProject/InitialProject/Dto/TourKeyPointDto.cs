using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
    internal class TourKeyPointDto
    {
        public string? TourKeyPointName { get; set; }
        public Location? Location { get; set; }


        public TourKeyPointDto() { }

        public TourKeyPointDto(string keyPointName, Location location)
        {
            TourKeyPointName = keyPointName;
            Location = location;
        }

    }
}
