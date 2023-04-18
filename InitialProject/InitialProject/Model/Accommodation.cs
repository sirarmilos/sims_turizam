using InitialProject.DTO;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }

        public string AccommodationName { get; set; }

        public string OwnerUsername { get; set; }

        public Location Location { get; set; }

        public string Type { get; set; }

        public int MaxGuests { get; set; }

        public int MinDaysReservation { get; set; }

        public int LeftCancelationDays { get; set; }

        public List<string> Images { get; set; }

        public Accommodation() { }

        public Accommodation(int id, string name, string ownerUsername, Location location, string type, int maxGuests, int minDaysReservation, int leftCancelationDays, List<string> images)
        {
            Id = id;
            AccommodationName = name;
            OwnerUsername = ownerUsername;
            Location = location;
            Type = type;
            MaxGuests = maxGuests;
            MinDaysReservation = minDaysReservation;
            LeftCancelationDays = leftCancelationDays;
            Images = images;
        }

        public Accommodation(int id, SaveNewAccommodationDTO saveNewAccommodationDTO, Location location)
        {
            Id = id;
            AccommodationName = saveNewAccommodationDTO.AccommodationName;
            OwnerUsername = saveNewAccommodationDTO.Owner;
            Location = location;
            Type = saveNewAccommodationDTO.Type;
            MaxGuests = saveNewAccommodationDTO.MaxGuests;
            MinDaysReservation = saveNewAccommodationDTO.MinDaysReservation;
            LeftCancelationDays = saveNewAccommodationDTO.LeftCancelationDays;
            Images = saveNewAccommodationDTO.Images;
        }

        public string[] ToCSV()
        {
            string imageToString = "";

            foreach(string image in Images)
            {
                imageToString += image;
                imageToString += ", ";
            }

            imageToString = imageToString.Substring(0, imageToString.Length - 2);

            string[] csvValues = { Id.ToString(), AccommodationName.ToString(), OwnerUsername.ToString(), Location.Id.ToString(), Type.ToString(), MaxGuests.ToString(), MinDaysReservation.ToString(), LeftCancelationDays.ToString(), imageToString.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationName = values[1];
            OwnerUsername = values[2];
            Location = new Location() { Id = Convert.ToInt32(values[3]) };
            Type = values[4];
            MaxGuests = Convert.ToInt32(values[5]);
            MinDaysReservation = Convert.ToInt32(values[6]);
            LeftCancelationDays = Convert.ToInt32(values[7]);

            string[] ImagesSplit = values[8].Split(',');
            
            List<string> images = new List<string>();

            foreach(string image in ImagesSplit)
            {
                images.Add(image);
            }

            Images = images;

            // unosi sa zarezima i onda ih ovde odvajam
        }

    }
}
