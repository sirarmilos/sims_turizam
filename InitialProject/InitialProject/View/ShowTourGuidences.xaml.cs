using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for ShowTourGuidences.xaml
    /// </summary>
    public partial class ShowTourGuidences : Window
    {
        public ShowTourGuidences()
        {
            InitializeComponent();
            this.DataContext = new ShowTourGuidencesViewModel();
        }
    }
}
