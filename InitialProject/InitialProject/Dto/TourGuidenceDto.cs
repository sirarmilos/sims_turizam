using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
    internal class TourGuidenceDto
    {
        public DateTime StartTime { get; set; }

        public TourGuidenceDto() { }

        public TourGuidenceDto(DateTime startTime)
        {
            StartTime = startTime;
        }
    }
}
