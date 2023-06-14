using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using LiveCharts.Wpf;
using LiveCharts;
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
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace InitialProject.View
{

    public partial class ShowOwnerReviewsView : Page
    {

        public ShowOwnerReviewsView(string username, Page page, NavigationService navService)
        {
            InitializeComponent();

            DataContext = new ShowOwnerReviewsViewModel(username, this, page, navService);

        }

    }
}
