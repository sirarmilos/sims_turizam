using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class ComplexTour : ISerializable
    {
        public int Id { get; set; }

        public List<Tour> Tour { get; set; }

        public Boolean Approved { get; set; }

        public ComplexTour() { }

        public ComplexTour(int id, List<Tour> tour, bool approved)
        {
            Id = id;
            Tour = tour;
            Approved = approved;
        }

        public void FromCSV(string[] values)
        {
            TourRepository tourRepository = new TourRepository();

            Id = Convert.ToInt32(values[0]);

            string[] tourSplit = values[1].Split(',');

            List<Tour> tours = new List<Tour>();

            foreach (string t in tourSplit)
            {
                int id = Convert.ToInt32(t);
                tours.Add(tourRepository.FindById(id));
            }

            Tour = tours;

            Approved = Convert.ToBoolean(values[2]);
        }

        public string[] ToCSV()
        {
            string tourToString = "";

            foreach (Tour t in Tour)
            {
                tourToString += t.Id.ToString();
                tourToString += ", ";
            }

            tourToString = tourToString.Substring(0, tourToString.Length - 2);


            string[] csvValues = { Id.ToString(), tourToString.ToString(), Approved.ToString()};
            return csvValues;
        }
    }
}
