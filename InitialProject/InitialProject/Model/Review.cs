using InitialProject.DTO;
using InitialProject.Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using System.Xml.Linq;

namespace InitialProject.Model
{
    public class Review : ISerializable
    {
        public Reservation Reservation { get; set; }

        public int Cleanliness { get; set;}

        public int Staff { get; set; }

        public int Comfort { get; set; }

        public int ValueForMoney { get; set; }

        public string Comment { get; set; }

        public List<string> Images { get; set; }

        public Review() 
        {

        }

        public Review(Reservation reservation, int cleanliness, int staff, int comfort, int valueForMoney, string comment, List<string> images)
        {
            Reservation = reservation;
            Cleanliness = cleanliness;
            Staff = staff;
            Comfort = comfort;
            ValueForMoney = valueForMoney;
            Comment = comment;
            Images = images;
        }

        public Review(Reservation reservation, SaveNewCreateReviewDTO saveNewCreateReviewDTO)
        {
            Reservation = reservation;
            Cleanliness = saveNewCreateReviewDTO.Cleanliness;
            Staff = saveNewCreateReviewDTO.Staff;
            Comfort = saveNewCreateReviewDTO.Comfort;
            ValueForMoney = saveNewCreateReviewDTO.ValueForMoney;
            Comment = saveNewCreateReviewDTO.Comment;
            Images = saveNewCreateReviewDTO.Images;
        }

        public string[] ToCSV()
        {
            string imageToString = "";

            foreach (string image in Images)
            {
                imageToString += image;
                imageToString += ", ";
            }
            if (imageToString != "")    
                imageToString = imageToString.Substring(0, imageToString.Length - 2);

            string[] csvValues = { Reservation.ReservationId.ToString(), Cleanliness.ToString(), Staff.ToString(), Comfort.ToString(), ValueForMoney.ToString(), Comment.ToString(), imageToString.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (string.IsNullOrWhiteSpace(values[0])) return;

            Reservation = new Reservation() { ReservationId = Convert.ToInt32(values[0]) };
            Cleanliness = Convert.ToInt32(values[1]);
            Staff = Convert.ToInt32(values[2]);
            Comfort = Convert.ToInt32(values[3]);
            ValueForMoney = Convert.ToInt32(values[4]);
            Comment = values[5];

            string[] ImagesSplit = values[6].Split(',');

            List<string> images = new List<string>();

            foreach (string image in ImagesSplit)
            {
                images.Add(image);
            }

            Images = images;
        }
    }
}
