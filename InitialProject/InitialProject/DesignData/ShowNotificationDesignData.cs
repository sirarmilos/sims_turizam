using InitialProject.DTO;
using System.Collections.Generic;
using System;
using System.Windows;

namespace InitialProject.DesignData
{
    public class ShowNotificationDesignData
    {
        public List<Guest1NotificationDTO> ShowNotificationDTOs { get; set; }

        public ShowNotificationDesignData()
        {

            ShowNotificationDTOs = new List<Guest1NotificationDTO>();

            for (int i = 0; i < 2; i++)
            {
                Guest1NotificationDTO temporary = new Guest1NotificationDTO(1,10,"Perina koliba", "g@zdamiki", false, "accepted");
                ShowNotificationDTOs.Add(temporary);

                temporary = new Guest1NotificationDTO(1, 10, "Perina koliba", "g@zdamiki", true, "rejected");
                ShowNotificationDTOs.Add(temporary);
            }
        }
    }
}