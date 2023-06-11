using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// <summary>
    /// Interaction logic for Guest2TourRequestCreation.xaml
    /// </summary>
    public partial class Guest2TourRequestCreation : Page
    {
      
        public Guest2TourRequestCreation()
        {
            InitializeComponent();
            DataContext = new Guest2CreateTourRequestViewModel(this);
        }



    }
}
