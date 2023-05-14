using InitialProject.DTO;
using System.Collections.Generic;
using System;
using System.Windows;

namespace InitialProject.DesignData
{
    public class Guest1RequestsDesignData
    {
        public List<Guest1RebookingRequestsDTO> Guest1RebookingRequestsDTOs { get; set; }

        public Guest1RequestsDesignData()
        {

            Guest1RebookingRequestsDTOs = new List<Guest1RebookingRequestsDTO>();

            for (int i = 0; i < 4; i++)
            {
                Guest1RebookingRequestsDTO tmp = new Guest1RebookingRequestsDTO(
                    1,
                    "Accommodation3",
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    "pending"
                );
                Guest1RebookingRequestsDTOs.Add(tmp);
            }
        }
    }
}