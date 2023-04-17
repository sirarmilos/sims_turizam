﻿ using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.Repository
{
    internal class TourGuidenceRepository
    {
        private const string FilePathTourGuidence = "../../../Resources/Data/tourguidences.csv";

        private const string FilePathReservatedTours = "../../../Resources/Data/reservatedtours.csv";

        
        private readonly Serializer<TourGuidence> tourGuidenceSerializer;

        private readonly Serializer<TourReservation> tourReservationSerializer;


        private List<TourGuidence> tourGuidences;

        private List<TourReservation> tourReservations;


        private TourReservationService tourReservationService;


        public TourGuidenceRepository()
        {
            tourGuidenceSerializer = new Serializer<TourGuidence>();
            tourGuidences = tourGuidenceSerializer.FromCSV(FilePathTourGuidence);

            tourReservationSerializer = new Serializer<TourReservation>();
            tourReservations = tourReservationSerializer.FromCSV(FilePathReservatedTours);


            tourReservationService = new TourReservationService();
        }

        public void Save(List<TourGuidence> tourGuidences)
        {
            tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
        }

        public void Update(TourGuidence tourGuidence)
        {
            TourGuidence tG = tourGuidences.FirstOrDefault(x => x.Id == tourGuidence.Id);
            tG = tourGuidence;
            tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
        }
 
        public int NextIdTourGuidence()
        {
            if (tourGuidences.Count < 1)
            {
                return 1;
            }
            return tourGuidences.Max(c => c.Id) + 1;
        }

        public TourGuidence GetById(int id) => tourGuidences.FirstOrDefault(x => x.Id == id);

        public List<TourGuidence> GetAll()
        {
            return tourGuidences;
        }

        public void SaveToFile(TourGuidence t)
        {
            t.Id = NextIdTourGuidence();
            tourGuidences.Add(t);
            tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
        }

        public List<TourGuidence> GetAllForToday()
        {
            List<TourGuidence> todaysTour = new();
            DateTime systemDate = DateTime.Today;

            foreach (TourGuidence t in tourGuidences)
            {
                if (systemDate == t.StartTime.Date && t.Finished==false)
                {
                    todaysTour.Add(t);
                }

            }
            return todaysTour;
        }

        public List<TourGuidence> GetAllFutureTours()
        {
            List<TourGuidence> futureTours = new();
            futureTours = tourGuidences.Where(item => item.StartTime >= DateTime.Today).ToList();
            return futureTours;
        }

        public bool CheckValidDateForCancel(DateTime date)
        {
            TimeSpan timeDiff = date - DateTime.Now;
            return timeDiff.TotalHours >= 48;
        }

        public void UpdateCancelField(int guidenceId)
        {
            foreach(TourGuidence guidence in tourGuidences)
            {
                if(guidence.Id == guidenceId)
                {
                    guidence.Cancelled = true;
                    break;  
                }
            }
            tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
        }

        public bool CheckGuidencesForStart(List<TourGuidence> guidences)
        {
            foreach(TourGuidence t in guidences)
            {
                if (t.Started == true)
                    return true;
            }
            return false;
        }

        public void UpdateStartedField(int guidenceId)
        {
            foreach (TourGuidence guidence in tourGuidences)
            {
                if (guidence.Id == guidenceId)
                {
                    guidence.Started = true;
                    break;
                }
            }
            tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
        }

        public void UpdateFinishedField(int tourGuidenceId)
        {
            foreach (TourGuidence tg in tourGuidences)
            {
                if (tg.Id == tourGuidenceId && tg.Finished == false)
                {
                    tg.Finished = true;
                    break;
                }
            }
            tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
        }

        public Tour GetMostVisitedAllTime()
        {
            int sum = 0;
            Tour tourMax = new Tour();
            TourReservationRepository tourReservationRepository = new();
            TourRepository tourRepository = new();
            List<Tour> tours = tourRepository.Load();
            int sumMax = 0;
            int indicator = 0;

            foreach(Tour t in tours)
            {
                foreach (TourGuidence tr in tourGuidences)
                {
                    if (tr.Finished == true && t.Id == tr.Tour.Id)
                    {
                        sumMax += tourReservationService.GetSumGuestNumber(tr.Id);
                        if(sumMax != 0)
                            indicator++;
                    }
                }
                if (indicator != 0)
                {
                    tourMax = t;
                    break;
                }
            }
            if (sumMax == 0)
                return null;




            foreach (Tour t in tours)
            {
                sum = 0;
                foreach (TourGuidence tr in tourGuidences)
                {
                    if (tr.Finished == true && t.Id == tr.Tour.Id)
                    {
                        sum += tourReservationService.GetSumGuestNumber(tr.Id);
                    }
                }
                if (sum > sumMax)
                {
                    sumMax = sum;
                    tourMax = t;
                }
            }
            return tourMax;
        }

        public Tour GetMostVisitedByYear(int year)
        {
            int sum = 0;
            Tour tourMax = new Tour();
            TourReservationRepository tourReservationRepository = new();
            TourRepository tourRepository = new();
            List<Tour> tours = tourRepository.Load();
            int sumMax = 0;
            int indicator = 0;

            foreach (Tour t in tours)
            {
                foreach (TourGuidence tr in tourGuidences)
                {
                    if (tr.Finished == true && t.Id == tr.Tour.Id && year == tr.StartTime.Year)
                    {
                        sumMax += tourReservationService.GetSumGuestNumber(tr.Id);
                        if (sumMax != 0)
                            indicator++;
                    }
                }
                if (indicator != 0)
                {
                    tourMax = t;
                    break;
                }
            }
            if (sumMax == 0)
                return null;




            foreach (Tour t in tours)
            {
                sum = 0;
                foreach (TourGuidence tr in tourGuidences)
                {
                    if (tr.Finished == true && t.Id == tr.Tour.Id && year == tr.StartTime.Year)
                    {
                        sum += tourReservationService.GetSumGuestNumber(tr.Id);
                    }
                }
                if (sum > sumMax)
                {
                    sumMax = sum;
                    tourMax = t;
                }
            }
            return tourMax;
        }




        public TourGuidence GetByTourAndDate(Tour tour, DateTime date)
        {
            TourGuidence tourGuidence = new TourGuidence();

            foreach(TourGuidence tourG in tourGuidences)
            {
                if(tour.Id == tourG.Tour.Id)
                {
                    if (DateTime.Compare(date, tourG.StartTime) == 0)
                    {
                        tourGuidence = tourG;
                        break;
                    }
                }
            }

            return tourGuidence;
        }


        public string GetGuide(int tourGuidenceId)
        {
            string guideUsername= "";

            foreach(TourGuidence tourGuidence in tourGuidences)
            {
                if(tourGuidence.Id==tourGuidenceId)
                {
                    guideUsername += tourGuidence.Tour.GuideUsername;
                    break;
                }

            }

            return guideUsername;

        }
        
        public TourAttendanceDTO GetTourAttendanceDTO(int tourReservationId)
        {
            TourAttendanceDTO dto = new TourAttendanceDTO();

            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();   

            TourReservation tourReservation = tourReservationRepository.GetById(tourReservationId);
            TourGuidence tourGuidence = GetById(tourReservation.tourGuidenceId);


            foreach(TourKeyPoint tourKeyPoint in tourKeyPointRepository.GetAll())
            {
                if(tourKeyPoint.TourGuidence.Id==tourGuidence.Id)
                {
                    dto.TourKeyPoints.Add(tourKeyPoint);
                }
            }

            dto.Date = tourGuidence.StartTime;
            dto.GuideUsername = tourGuidence.Tour.GuideUsername;

            return dto;
        }

    }
}
