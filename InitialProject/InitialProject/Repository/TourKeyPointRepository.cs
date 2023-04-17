﻿using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    internal class TourKeyPointRepository
    {
        private const string FilePathTourKeyPoint = "../../../Resources/Data/tourkeypoints.csv";

        private readonly Serializer<TourKeyPoint> tourKeyPointSerializer;

        private List<TourKeyPoint> tourKeyPoints;  
        

        public TourKeyPointRepository()
        {
            tourKeyPointSerializer = new Serializer<TourKeyPoint>();
            tourKeyPoints = tourKeyPointSerializer.FromCSV(FilePathTourKeyPoint);
        }

        public void Save(List<TourKeyPoint>  tourKeyPoints)
        {
            tourKeyPointSerializer.ToCSV(FilePathTourKeyPoint,tourKeyPoints);
        }

        public int NextId()
        {
            if (tourKeyPoints.Count < 1)
            {
                return 1;
            }
            return tourKeyPoints.Max(c => c.Id) + 1;
        }

        public TourKeyPoint GetById(int id) => tourKeyPoints.FirstOrDefault(x => x.Id == id);

        public List<TourKeyPoint> GetAll()
        {
            return tourKeyPoints;
        }

        internal void SaveToFile(TourKeyPoint tkp)
        {
            tourKeyPoints.Add(tkp);
            Save(tourKeyPoints);
        }

        public void UpdateCheckedKeyPoints(List<TourKeyPoint> keyPoints)
        {
            foreach(TourKeyPoint tourKeyPoint in keyPoints)
            {
                TourKeyPoint kp = GetById(tourKeyPoint.Id);
                kp.Passed = tourKeyPoint.Passed;
            }
            Save(tourKeyPoints);

        }
    }
}
