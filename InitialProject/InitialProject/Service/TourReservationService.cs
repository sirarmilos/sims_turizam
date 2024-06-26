﻿using InitialProject.IRepository;
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
    public class TourReservationService
    {
        private readonly ITourReservationRepository tourReservationRepository;

        private readonly TourKeyPointService tourKeyPointService;

        public TourReservationService()
        {
            tourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
            //tourReservationRepository = new TourReservationRepository();    
            tourKeyPointService = new TourKeyPointService();
        }

        public List<Model.TourReservation> FindAll()
        {
            return tourReservationRepository.FindAll();
        }

        public int UpdateKeyPointArrivals(int guidenceId, string username, int keyPoint)
        {
            List<Model.TourReservation> tourReservations = tourReservationRepository.FindAll();

            foreach (Model.TourReservation tr in tourReservations)
            {
                if (username == tr.userId && tr.tourGuidenceId == guidenceId)
                {
                    if (keyPoint > tr.TourKeyPointArrival.Count)
                    {
                        return -1;
                    }

                    for (int i = 0; i < tr.TourKeyPointArrival.Count; i++)
                    {
                        if (tr.TourKeyPointArrival[i] == true)
                            return -1;
                    }

                    tr.TourKeyPointArrival[keyPoint - 1] = true;
                    tourReservationRepository.Save(tourReservations);
                    return 1;
                }
            }
            return -1;
        }

        public Model.TourReservation FindByGuestAndGuidence(string userId, int tourGuidenceId)
        {
            Model.TourReservation retVal = new Model.TourReservation();
            foreach (Model.TourReservation reservation in tourReservationRepository.FindAll())
            {
                if (reservation.userId == userId && reservation.tourGuidenceId == tourGuidenceId)
                {
                    retVal = reservation;
                    break;
                }
            }
            return retVal;
        }

        public List<Dto.ReservationDisplayDto> GetAllForOneTourGuidence(int guidenceId)
        {
            List<Dto.ReservationDisplayDto> reservations = new List<Dto.ReservationDisplayDto>();
            List<TourKeyPoint> tourKeyPoints = new List<TourKeyPoint>();
            tourKeyPoints = tourKeyPointService.FindByTourGuidance(guidenceId);

            foreach (Model.TourReservation tr in tourReservationRepository.FindAll())
            {

                if (tr.tourGuidenceId == guidenceId)
                {
                    Dto.ReservationDisplayDto dto = new Dto.ReservationDisplayDto();
                    dto.userId = tr.userId;
                    dto.tourGuidenceId = tr.tourGuidenceId;
                    dto.TourKeyPointArrival = tr.TourKeyPointArrival;
                    dto.numberOfGuests = tr.numberOfGuests;
                    dto.TourKeyPoints = tourKeyPoints;
                    reservations.Add(dto);
                }
            }
            return reservations;
        }

        public int GetSumGuestNumber(int guidenceId)
        {
            int sum = 0;
            foreach (Model.TourReservation tr in tourReservationRepository.FindAll())
            {
                if (tr.tourGuidenceId == guidenceId && tr.Confirmed == true)
                {
                    sum += tr.numberOfGuests;
                }
            }
            return sum;
        }

        public bool CreateReservation(string username, TourGuidence tourGuidence, int numberOfGuests, int voucherId, int Id)
        {
            try
            {
                List<Boolean> arrivals = new List<Boolean>();
                arrivals = SetArrivalsToFalse(tourGuidence.Id);
                TourGuidenceService tourGuidanceService = new TourGuidenceService();

                List<Model.TourReservation> tourReservations = tourReservationRepository.FindAll();
                Model.TourReservation reservation = new Model.TourReservation(username, tourGuidence.Id, arrivals, numberOfGuests, false, voucherId, Id);
                tourReservations.Add(reservation);

                tourReservationRepository.Save(tourReservations);
                tourGuidanceService.UpdateTourGuidenceFreeSlot(tourGuidence, numberOfGuests);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Boolean> SetArrivalsToFalse(int guidenceId)
        {
            List<TourKeyPoint> tourKeyPoints = new List<TourKeyPoint>();   
            tourKeyPoints = tourKeyPointService.FindByTourGuidance(guidenceId);
            List<Boolean> retVal = new List<Boolean>();
            foreach (TourKeyPoint kp in tourKeyPoints)
            {
                retVal.Add(false);
            }
            return retVal;
        }

        public void ConfirmTourAttendance(string username, int tourReservationId)
        {
            List<Model.TourReservation> result = tourReservationRepository.FindAll();
            foreach (Model.TourReservation tourReservation in result)
            {
                if (tourReservation.Id == tourReservationId && tourReservation.userId.Equals(username))
                {
                    tourReservation.Confirmed = true;
                    tourReservationRepository.Save(result);
                    break;
                }
            }
        }

        public Model.TourReservation FindById(int id)
        {
            return tourReservationRepository.FindById(id);
        }

        public int NextId()
        { return tourReservationRepository.NextId();}

        public Dictionary<int,int> AddedToTour(string username)
        {
            TourGuidenceService tourGuidenceService = new TourGuidenceService();
            List<int> startedToursForGuest = tourGuidenceService.NotifyGuestOfTourStarting(username);
            List<int> result = new List<int>();

            Dictionary<int,int> keyValuePairs = new Dictionary<int,int>();  

            foreach(int id in startedToursForGuest)
            {
                foreach(Model.TourReservation tourReservation in tourReservationRepository.FindAll())
                {
                    if(id==tourReservation.Id)
                    {
                        int br = 0;
                        foreach (bool accepted in tourReservation.TourKeyPointArrival)
                        {
                            br++;
                            if(accepted == true)
                            {
                                keyValuePairs[tourReservation.Id] = br;
                            }
                        }
                    }
                }
            }

            return keyValuePairs;

        }

        public void UpdateIsUsedForVoucher(int id)
        {
            tourReservationRepository.UpdateIsUsedForVoucher(id);
        }
    }
}
