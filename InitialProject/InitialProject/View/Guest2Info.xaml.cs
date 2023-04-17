using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.ViewModel;
using System;
using System.Collections.Generic;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2Info.xaml
    /// </summary>
    public partial class Guest2Info : Window
    {
        public Guest2Info(string username)
        {
            InitializeComponent();
            this.DataContext = new Guest2InfoViewModel(username);
        }
    }

  
}
