using InitialProject.DTO;
using System.Collections.Generic;
using System;
using System.Windows;
using InitialProject.Model;

namespace InitialProject.DesignData
{
    public class Guest1ForumPreviewDesignData
    {

        public ShowGuest1ForumsDTO ShowGuest1ForumsDTO
        {
            get;
            set;
        }

        public List<ShowGuest1ForumCommentsDTO> ShowGuest1ForumCommentsDTOs
        {
            get;
            set;
        }

        public string Guest1
        {
            get;
            set;
        }

        public string Guest1UsernameShow
        {
            get;
            set;
        }

        public Guest1ForumPreviewDesignData()
        {

            Guest1 = "vasaGost";

            Guest1UsernameShow = "vasaGost" + ":";

            Location lokacija = new Location(10, "srbija", "novi sad", "pera cetkovica", 40, 40);

            ShowGuest1ForumsDTO = new ShowGuest1ForumsDTO(15, Guest1, "Belgrade, Serbia", "pitanje sam postavio", "useful", "(closed)");

            ShowGuest1ForumCommentsDTOs = new List<ShowGuest1ForumCommentsDTO>();

            for (int i = 0; i < 4; i++)
            {
                ShowGuest1ForumCommentsDTO tmp = new ShowGuest1ForumCommentsDTO(12, Guest1, "guest", "bravoooo", "visited", "0", "owner");
                ShowGuest1ForumCommentsDTOs.Add(tmp);
            }
        }
    }
}