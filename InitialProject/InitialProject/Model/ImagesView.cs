using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.Model
{
    public class ImagesView
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }

        public ImagesView()
        {

        }

        public ImagesView(string id, string imageUrl)
        {
            Id = id;
            ImageUrl = imageUrl;
        }

    }
}
