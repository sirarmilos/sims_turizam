﻿using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
    class TourDisplayDTO
    {
        public string TourName { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public int MaxGuests { get; set; }
        public string TourKeyPoints { get; set; }
        public string TourDate { get; set; }
        public int Duration { get; set; }
        public List<string> Images { get; set; }

        public TourDisplayDTO() { }

        public TourDisplayDTO CreateDTO(Tour tour)
        {
            TourDisplayDTO dto = new TourDisplayDTO();

            dto.TourName = tour.TourName;
            dto.Location = tour.Location;
            dto.Description = tour.Description;
            dto.Language = tour.Language;
            dto.MaxGuests = tour.MaxGuests;

            dto.TourKeyPoints = "";

            foreach(TourKeyPoint tourKeyPoint in tour.TourKeyPoints)
            {
                dto.TourKeyPoints += tourKeyPoint.KeyPointName + ",";
            }

            dto.TourKeyPoints = dto.TourKeyPoints.Remove(dto.TourKeyPoints.Length - 1);

            foreach(DateTime dateTime in  tour.TourDate)
            {
                dto.TourDate += dateTime.ToString() + ",";
            }

            dto.TourDate = dto.TourDate.Remove(dto.TourDate.Length-1);

            dto.Duration = tour.Duration;
            dto.Images = tour.Images;

            return dto;
        }

        
    }
}
