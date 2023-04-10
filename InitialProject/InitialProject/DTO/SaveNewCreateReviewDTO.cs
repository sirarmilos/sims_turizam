using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InitialProject.DTO
{
    public class SaveNewCreateReviewDTO
    {
        public int ReservationId { get; set; }

        public int Cleanliness { get; set; }

        public int Staff { get; set; }

        public int Comfort { get; set; }

        public int ValueForMoney { get; set; }

        public string Comment { get; set; }

        public List<string> Images { get; set; }

        public SaveNewCreateReviewDTO()
        {

        }

        public SaveNewCreateReviewDTO(int reservationId, int cleanliness, int staff, int comfort, int valueForMoney, string comment, ObservableCollection<string> images) // todo: promeniti observable collection !!!
        {
            ReservationId = reservationId;
            Cleanliness = cleanliness;
            Staff = staff;
            Comfort = comfort;
            ValueForMoney = valueForMoney;
            Comment = comment;
            Images = new List<string>(images);
        }
    }
}
