using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
    public class ReservationDisplayDto
    {
        public string userId { get; set; }
        public int tourGuidenceId { get; set; }
        public int TourKeyPointArrivalId { get; set; }
        public int numberOfGuests { get; set; }
        public Boolean Confirmed { get; set; }

        public List<TourKeyPoint> TourKeyPoints { get; set; }

        public ReservationDisplayDto() { }

        public ReservationDisplayDto(string userId, int reservatedTourId, int arrivalTourKeyPointId, int numberOfGuests, Boolean confirmed, List<TourKeyPoint> keyPoints)
        {
            this.userId = userId;
            this.tourGuidenceId = reservatedTourId;
            TourKeyPointArrivalId = arrivalTourKeyPointId;
            this.numberOfGuests = numberOfGuests;
            Confirmed = confirmed;
            TourKeyPoints = keyPoints;
        }


    }
}
