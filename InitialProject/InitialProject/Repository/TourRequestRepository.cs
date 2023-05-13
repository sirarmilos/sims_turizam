﻿using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.Repository
{
    public class TourRequestRepository : ITourRequestRepository
    {

        private const string FilePathTourRequests = "../../../Resources/Data/tourrequests.csv";

        private readonly Serializer<TourRequest> tourRequestSerializer;

        private List<TourRequest> requests;

        public TourRequestRepository()
        {
            tourRequestSerializer = new Serializer<TourRequest>();
        }

        public List<TourRequest> FindAll()
        {
            UserRepository userRepository = new UserRepository();
            LocationRepository locationRepository = new LocationRepository();

            requests = tourRequestSerializer.FromCSV(FilePathTourRequests);

            foreach (TourRequest temporaryRequest in requests.ToList())
            {
                temporaryRequest.User = userRepository.FindByUsername(temporaryRequest.User.Username);
                temporaryRequest.Location = locationRepository.FindById(temporaryRequest.Location.Id);
            }

            return requests;
        }

        public TourRequest FindById(int id)
        {
            return FindAll().ToList().Find(x => x.Id == id);
        }

        public List<TourRequest> FindByUser(User user)
        {
            return FindAll().ToList().FindAll(x => x.User == user);
        }

        public void Save(List<TourRequest> requests)
        {
            tourRequestSerializer.ToCSV(FilePathTourRequests, requests);
        }

        public List<TourRequest> FindAllByCountry(List<TourRequest> allTourRequests, string name)
        {
            return allTourRequests.FindAll(x => x.Location.Country.ToLower().StartsWith(name.ToLower()) == true);
        }

        public List<TourRequest> FindAllByCity(List<TourRequest> allTourRequests, string name)
        {
            return allTourRequests.FindAll(x => x.Location.City.ToLower().StartsWith(name.ToLower()) == true);
        }

        public List<TourRequest> FindAllByGuestsNumber(List<TourRequest> allTourRequests, int? quantity)
        {
            return allTourRequests.FindAll(x => x.GuestNumber == quantity);
        }

        public List<TourRequest> FindAllByLanguage(List<TourRequest> allTourRequests, string language)
        {
            Enum.TryParse(language, out Language languageEnum);
            return allTourRequests.FindAll(x => x.Language.ToString().ToLower().StartsWith(language.ToLower()));
        }
    }
}
