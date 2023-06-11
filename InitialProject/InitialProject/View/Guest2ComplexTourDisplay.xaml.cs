using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
using InitialProject;
using InitialProject.ViewModel;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2ComplexTourDisplay.xaml
    /// </summary>
    public partial class Guest2ComplexTourDisplay : Page
    {

        public Guest2ComplexTourDisplay(string username)
        {
           InitializeComponent();
           DataContext = new Guest2ComplexTourDisplayViewModel(this,Resources);
        }
    }
}
