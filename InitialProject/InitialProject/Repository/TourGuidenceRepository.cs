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

        public List<TourGuidence> Load()
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
                if (systemDate == t.StartTime.Date)
                {
                    todaysTour.Add(t);
                }

            }
            return todaysTour;
        }

        public int UpdateStartedField(int guidenceId)
        {
            foreach (TourGuidence guidence in tourGuidences)
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
            return 0;
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

        public bool CreateReservation(string username, TourGuidence tourGuidence, int numberOfGuests)
        {
            try
            {
                TourReservation reservation = new TourReservation(username,tourGuidence.Id, 0, numberOfGuests, false);
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





    }
}
