using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourGuidenceService
    {
        private readonly ITourGuidenceRepository tourGuidenceRepository;

        private readonly TourReservationService tourReservationService; 

        

        public TourGuidenceService()
        {
            tourGuidenceRepository = new TourGuidenceRepository();  

            tourReservationService = new TourReservationService();
        }

        public TourGuidence FindById(int id)
        {
            return tourGuidenceRepository.FindById(id);
        }

        public List<TourGuidence> FindAll()
        {
            return tourGuidenceRepository.FindAll();
        }

        public List<TourGuidence> FindAllForToday()
        {
            List<TourGuidence> todaysTour = new();
            DateTime systemDate = DateTime.Today;

            foreach (TourGuidence t in tourGuidenceRepository.FindAll())
            {
                if (systemDate == t.StartTime.Date && t.StartTime.TimeOfDay >= DateTime.Now.TimeOfDay && t.Finished == false)
                {
                    todaysTour.Add(t);
                }

            }
            return todaysTour;
        }

        public List<TourGuidence> FindAllFutureTours()
        {
            List<TourGuidence> futureTours = new();
            futureTours = tourGuidenceRepository.FindAll().Where(item => item.StartTime >= DateTime.Now).ToList();
            return futureTours;
        }

        public bool CheckValidDateForCancel(DateTime date)
        {
            TimeSpan timeDiff = date - DateTime.Now;
            return timeDiff.TotalHours >= 48;
        }

        public void UpdateStartedField(int guidenceId)
        {
            List<TourGuidence> guidences = tourGuidenceRepository.FindAll();

            foreach (TourGuidence guidence in guidences)
            {
                if (guidence.Id == guidenceId)
                {
                    guidence.Started = true;
                    break;
                }
            }
            tourGuidenceRepository.Save(guidences);
        }

        public void UpdateFinishedField(int tourGuidenceId)
        {

            List<TourGuidence> guidences = tourGuidenceRepository.FindAll();

            foreach (TourGuidence tg in guidences)
            {
                if (tg.Id == tourGuidenceId && tg.Finished == false)
                {
                    tg.Finished = true;
                    break;
                }
            }
            tourGuidenceRepository.Save(guidences);
        }

        public void UpdateCancelledField(int guidenceId)
        {
            List<TourGuidence> guidences = tourGuidenceRepository.FindAll();

            foreach (TourGuidence guidence in guidences)
            {
                if (guidence.Id == guidenceId)
                {
                    guidence.Cancelled = true;
                    break;
                }
            }
            tourGuidenceRepository.Save(guidences);
        }

        public bool CheckGuidencesForStart(List<TourGuidence> guidences)
        {
            foreach (TourGuidence t in guidences)
            {
                if (t.Started == true)
                    return true;
            }
            return false;
        }

        public Tour FindMostVisitedAllTime()
        {
            List<TourGuidence> tourGuidences = tourGuidenceRepository.FindAll();
            TourService tourService = new TourService();
            List<Tour> tours = tourService.FindAll();
            Tour tourMax = new Tour();

            int sum = 0;
            int sumMax = 0;
            int indicator = 0;

            foreach (Tour t in tours)
            {
                foreach (TourGuidence tr in tourGuidences)
                {
                    if (tr.Finished == true && t.Id == tr.Tour.Id)
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

        public Tour FindMostVisitedByYear(int year)
        {
            List<TourGuidence> tourGuidences = tourGuidenceRepository.FindAll();
            TourService tourService = new TourService();
            List<Tour> tours = tourService.FindAll();
            Tour tourMax = new Tour();

            int sum = 0;
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



        public List<int> NotifyGuestOfTourStarting(string username)
        {
            List<int> results = new List<int>();

            TourReservationService tourReservationService = new TourReservationService();

            foreach (Model.TourReservation tourReservation in tourReservationService.GetAll())
            {
                if (tourReservation.userId.Equals(username))
                {
                    foreach (TourGuidence tourGuidence in tourGuidenceRepository.FindAll())
                    {
                        if (tourReservation.tourGuidenceId == tourGuidence.Id)
                        {
                            if (tourGuidence.Finished == false && tourGuidence.Started == true && tourReservation.Confirmed == false)
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

            TourReservationService tourReservationService = new TourReservationService();

            List<Model.TourReservation> tourReservations = tourReservationService.GetAll();
            
            foreach (Model.TourReservation tourReservation in tourReservations)
            {
                if (tourReservation.userId.Equals(username))
                {
                    foreach (TourGuidence tourGuidence in tourGuidenceRepository.FindAll())
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

        public void UpdateTourGuidenceFreeSlot(TourGuidence reservatedTourGuidence, int numberOfGuests)
        {

            List<TourGuidence> result = tourGuidenceRepository.FindAll();

            foreach (TourGuidence tourGuidence in result)
            {
                if (tourGuidence.Equals(reservatedTourGuidence))
                {
                    tourGuidence.FreeSlots = tourGuidence.FreeSlots - numberOfGuests;
                    tourGuidenceRepository.Save(result);
                    break;
                }
            }

        }
    }
}
