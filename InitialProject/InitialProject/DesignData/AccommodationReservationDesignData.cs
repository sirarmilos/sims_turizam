using InitialProject.Model;
using System.Collections.Generic;
using System;
using System.Windows;

namespace InitialProject.DesignData
{
    public class AccommodationReservationDesignData
    {
        public Accommodation Accommodation { get; set; }

        public AccommodationReservationDesignData()
        {
            Location lokacija = new Location(10, "srbija", "novi sad", "pera cetkovica", 40, 40);

            List<string> images = new List<string>();
            images.Add("https://www.ravensbourne.ac.uk/sites/default/files/2022-03/iQ-Student-Accommodation-London-Greenwich-Bedrooms-Duplicate-Room-48-Medium_Studio_0.jpg");
            images.Add("https://www.ravensbourne.ac.uk/sites/default/files/2022-03/iQ-Student-Accommodation-London-Greenwich-Bedrooms-Duplicate-Room-48-Medium_Studio_0.jpg");

            Accommodation = new Accommodation(
                    10, "Accommodation 1", "ana", lokacija, "Hut", 5, 2, 3, images, true
            );           
        }
    }
}