﻿using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class RenovationRecommendationRepository : IRenovationRecommendationRepository
    {
        private ReservationRepository reservationRepository;

        private const string FilePathRecommedationRepository = "../../../Resources/Data/renovationrecommendations.csv";

        private readonly Serializer<RenovationRecommendation> renovationRecommendationSerializer;

        private List<RenovationRecommendation> renovationRecommedations;

        public RenovationRecommendationRepository()
        {
            renovationRecommendationSerializer = new Serializer<RenovationRecommendation>();
        }

        public List<RenovationRecommendation> FindAll()
        {
            reservationRepository = new ReservationRepository();

            renovationRecommedations = renovationRecommendationSerializer.FromCSV(FilePathRecommedationRepository);

            foreach(RenovationRecommendation temporaryRenovationRecommendation in renovationRecommedations.ToList())
            {
                temporaryRenovationRecommendation.Reservation = reservationRepository.FindById(temporaryRenovationRecommendation.Reservation.ReservationId);
            }

            return renovationRecommedations;
        }

        public List<RenovationRecommendation> FindByAccommodationId(int accommodationId)
        {
            return FindAll().ToList().FindAll(x => x.Reservation.Accommodation.Id == accommodationId);
        }

        public int FindAccommodationRenovationRecommedationCountByYear(int accommodationId, int year)
        {
            return FindByAccommodationId(accommodationId).ToList().FindAll(x => x.Reservation.StartDate.Year == year).Count; //
        }

        public List<RenovationRecommendation> FindAccommodationRenovationRecommedationsByYear(int accommodationId, int year)
        {
            return FindByAccommodationId(accommodationId).ToList().FindAll(x => x.Reservation.StartDate.Year == year);
        }
    }
}
