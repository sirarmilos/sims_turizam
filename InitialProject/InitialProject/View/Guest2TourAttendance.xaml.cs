using InitialProject.Dto;
using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2TourAttendance.xaml
    /// </summary>
    public partial class Guest2TourAttendance : Page
    {
        private string Username;

        public Guest2TourAttendance()
        {
            InitializeComponent();
            DataContext = new Guest2TourAttendanceViewModel(this);

        }
    }
}
