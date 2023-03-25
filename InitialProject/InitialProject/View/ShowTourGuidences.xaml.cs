using InitialProject.Model;
using InitialProject.Repository;
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
        public static ObservableCollection<TourGuidence> tourGuidences { get; set; }

        private readonly TourGuidenceRepository tourGuidenceRepository;



        public ShowTourGuidences()
        {
            InitializeComponent();
            DataContext = this;
            tourGuidenceRepository = new TourGuidenceRepository();
            tourGuidences = new ObservableCollection<TourGuidence>(tourGuidenceRepository.Load());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartTourGuidence(object sender, RoutedEventArgs e)
        {
            TourGuidence selectedGuidence = (TourGuidence)dataGrid.SelectedItem;

            int reVal = tourGuidenceRepository.UpdateStartedField(selectedGuidence.Id);
            if (reVal >= 0)
            {
                ShowKeyPointsInStartedTourGuidence window = new ShowKeyPointsInStartedTourGuidence(selectedGuidence.Id);
                window.Show();
            }
            var same_window = Application.Current.MainWindow;
            same_window.UpdateLayout();


        }
    }
}
