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
    internal class TourGuidenceService
    {
        private readonly TourGuidenceRepository tourGuidenceRepository;

        public TourGuidenceService()
        {
            tourGuidenceRepository = new TourGuidenceRepository();  
        }

        public TourGuidence GetById(int id)
        {
            List<TourGuidence> tourGuidences = tourGuidenceRepository.GetAll();
            TourGuidence tourGuidence = new TourGuidence();

            foreach(TourGuidence tg in tourGuidences)
            {
                if(tg.Id==id)
                {
                    tourGuidence = tg;
                    break;
                }
            }

            return tourGuidence;
        }


        public List<int> NotifyGuestOfTourStarting(string username)
        {
            List<int> results = new List<int>();

            TourReservationService tourReservationService = new TourReservationService();

            foreach (Model.TourReservation tourReservation in tourReservationService.GetAll())
            {
                if (tourReservation.userId.Equals(username))
                {
                    foreach (TourGuidence tourGuidence in tourGuidenceRepository.GetAll())
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
                    foreach (TourGuidence tourGuidence in tourGuidenceRepository.GetAll())
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

            List<TourGuidence> result = tourGuidenceRepository.GetAll();

            foreach (TourGuidence tourGuidence in tourGuidenceRepository.GetAll())
            {
                if (tourGuidence.Equals(reservatedTourGuidence))
                {
                    tourGuidence.FreeSlots = tourGuidence.FreeSlots - numberOfGuests;
                    tourGuidenceRepository.Save(result);
                    break;
                }
            }

        }

        public List<TourGuidence> GetAllForToday()
        {
            List<TourGuidence> todaysTour = new();
            DateTime systemDate = DateTime.Today;

            foreach (TourGuidence t in tourGuidenceRepository.GetAll())
            {
                if (systemDate == t.StartTime.Date && t.StartTime.TimeOfDay>=DateTime.Now.TimeOfDay && t.Finished == false)
                {
                    todaysTour.Add(t);
                }

            }
            return todaysTour;
        }

        public List<TourGuidence> GetAllFutureTours()
        {
            List<TourGuidence> futureTours = new();
            futureTours = tourGuidenceRepository.GetAll().Where(item => item.StartTime >= DateTime.Now).ToList();
            return futureTours;
        }

        public bool CheckValidDateForCancel(DateTime date)
        {
            TimeSpan timeDiff = date - DateTime.Now;
            return timeDiff.TotalHours >= 48;
        }

        public void UpdateStartedField(int guidenceId)
        {
            List<TourGuidence> guidences = new List<TourGuidence>();
            guidences = tourGuidenceRepository.GetAll();
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

            List<TourGuidence> guidences = new List<TourGuidence>();
            guidences = tourGuidenceRepository.GetAll();
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
            List<TourGuidence> guidences = new List<TourGuidence>();
            guidences = tourGuidenceRepository.GetAll();
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

    }
}
