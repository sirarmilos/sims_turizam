using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Serializer;
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


        public TourGuidenceRepository()
        {
            tourGuidenceSerializer = new Serializer<TourGuidence>();
            tourGuidences = tourGuidenceSerializer.FromCSV(FilePathTourGuidence);

            tourReservationSerializer = new Serializer<TourReservation>();
            tourReservations = tourReservationSerializer.FromCSV(FilePathReservatedTours);

        }

        public TourGuidence Save(DateTime startTime)
        {
            TourGuidence tourGuidence = new(NextIdTourGuidence(), null, startTime, false, false, false);
            tourGuidences.Add(tourGuidence);
            //tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
            return tourGuidence;
        }

        public void Update(TourGuidence tourGuidence)
        {
            TourGuidence tG = tourGuidences.FirstOrDefault(x => x.Id == tourGuidence.Id);
            tG = tourGuidence;
            tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
        }

        public List<int> NotifyGuestOfTourStarting(string username)
        {
            List<int> results = new List<int>();
            foreach (TourReservation tourReservation in tourReservations)
            {
                if (tourReservation.userId.Equals(username))
                {
                    foreach (TourGuidence tourGuidence in tourGuidences)
                    {
                        if (tourReservation.tourGuidenceId == tourGuidence.Id)
                        {
                            if(tourGuidence.Finished==false && tourGuidence.Started==true && tourReservation.Confirmed==false)
                            {
                                results.Add(tourReservation.Id);
                            }
                        }
                    }
                }
            }

            return results;
        }

        public List<int> GetTourReservationsForTracking(string username)
        {
            List<int> results = new List<int>();
            foreach (TourReservation tourReservation in tourReservations)
            {
                if (tourReservation.userId.Equals(username))
                {
                    foreach (TourGuidence tourGuidence in tourGuidences)
                    {
                        if (tourReservation.tourGuidenceId == tourGuidence.Id)
                        {
                            if (tourGuidence.Started == true && tourReservation.Confirmed == true)
                            {
                                results.Add(tourReservation.Id);
                            }
                        }
                    }
                }
            }

            return results;
        }

        public void ConfirmTourAttendance(string username, int tourReservationId)
        {
            List<TourReservation> result = tourReservations;
            foreach(TourReservation tourReservation in tourReservations)
            {
                if(tourReservation.Id==tourReservationId && tourReservation.userId.Equals(username))
                {
                    tourReservation.Confirmed = true;
                    tourReservationSerializer.ToCSV(FilePathReservatedTours,result);
                    break;
                }
            }
        }


        public void UpdateTourGuidenceFreeSlot(TourGuidence reservatedTourGuidence, int numberOfGuests)
        {

            List<TourGuidence> result = tourGuidences;

            foreach (TourGuidence tourGuidence in tourGuidences)
            {
                if (tourGuidence.Equals(reservatedTourGuidence))
                {
                    tourGuidence.FreeSlots = tourGuidence.FreeSlots - numberOfGuests;
                    tourGuidenceSerializer.ToCSV(FilePathTourGuidence, result);
                    break;
                }
            }

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
            int a = 7;
        }

        public List<TourGuidence> GetAllForToday()
        {
            List<TourGuidence> todaysTour = new();
            DateTime systemDate = DateTime.Today;
            /*foreach(TourGuidence t in tourGuidences)
            {
                if(DateTime.Compare(t.StartTime, DateTime.Now) > 0)
                {
                    todaysTour.Add(t);
                }

            }*/
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
            /*foreach (TourGuidence guidence in tourGuidences)
            {
                if (guidence.Started == true && guidence.Finished == false && guidence.Id != guidenceId)
                {
                    
                    return -1;
                }

            }

            foreach (TourGuidence guidence in tourGuidences)
            {
                if (guidence.Id == guidenceId)
                {
                    if (guidence.Started == false)
                    {
                        guidence.Started = true;
                    }
                    tourGuidenceSerializer.ToCSV(FilePathTourGuidence, tourGuidences);
                    return 1;
                }
            }
            return 0;*/

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
                        sumMax += tourReservationRepository.GetSumGuestNumber(tr.Id);
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
                        sum += tourReservationRepository.GetSumGuestNumber(tr.Id);
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
                        sumMax += tourReservationRepository.GetSumGuestNumber(tr.Id);
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
                        sum += tourReservationRepository.GetSumGuestNumber(tr.Id);
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

        public bool CreateReservation(string username, TourGuidence tourGuidence, List<Boolean> arrivals, int numberOfGuests, int voucherId, int Id)
        {
            try
            {
                TourReservation reservation = new TourReservation(username,tourGuidence.Id, arrivals, numberOfGuests, false,voucherId,Id);
                tourReservations.Add(reservation);
                tourReservationSerializer.ToCSV(FilePathReservatedTours,tourReservations);
                UpdateTourGuidenceFreeSlot(tourGuidence,numberOfGuests);
                return true;
            }
            catch
            {
                return false;
            }
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
