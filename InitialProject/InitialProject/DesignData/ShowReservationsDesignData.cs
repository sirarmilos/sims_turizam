using InitialProject.DTO;
using System.Collections.Generic;
using System;
using System.Windows;
using InitialProject.Model;


namespace InitialProject.DesignData
{
    public class ShowReservationsDesignData
    {
        public List<ShowReservationDTO> ShowReservationDTOs { get; set; }

        public ShowReservationsDesignData()
        {

            ShowReservationDTOs = new List<ShowReservationDTO>();

            Location lokacija = new Location(10, "srbija", "novi sad", "pera cetkovica", 40, 40);

            List<string> images = new List<string>();
            images.Add("https://www.ravensbourne.ac.uk/sites/default/files/2022-03/iQ-Student-Accommodation-London-Greenwich-Bedrooms-Duplicate-Room-48-Medium_Studio_0.jpg");
            images.Add("https://www.ravensbourne.ac.uk/sites/default/files/2022-03/iQ-Student-Accommodation-London-Greenwich-Bedrooms-Duplicate-Room-48-Medium_Studio_0.jpg");


            for (int i = 0; i < 4; i++)
            {
                Accommodation tmpAccommodation = new Accommodation(
                    10, "Accommodation 1", "ana", lokacija, "Hut", 5, 2, 3, images, true
                );

                ShowReservationDTO tmpShowReservationDTO = new ShowReservationDTO(
                    10, tmpAccommodation, DateTime.Now, DateTime.Now, 2
                );

                ShowReservationDTOs.Add(tmpShowReservationDTO);
            }
        }
    }
}