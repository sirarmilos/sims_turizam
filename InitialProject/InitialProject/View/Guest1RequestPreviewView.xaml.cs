using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.View
{
    public partial class Guest1RequestPreviewView : Page
    {
        public Guest1RequestPreviewView(string username, Page page, NavigationService navService, Guest1RebookingRequestsDTO guest1RebookingRequestsDTO, string callingWindow)
        {
            InitializeComponent();

            DataContext = new Guest1RequestPreviewViewModel(username, this, page, navService, guest1RebookingRequestsDTO, callingWindow);
        }
    }
}
