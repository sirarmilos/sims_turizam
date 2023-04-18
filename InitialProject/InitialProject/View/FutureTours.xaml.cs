using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
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
    /// Interaction logic for FutureTours.xaml
    /// </summary>
    public partial class FutureTours : Window
    {

        public static ObservableCollection<TourGuidence> tourGuidences { get; set; }

        private readonly TourGuidenceRepository tourGuidenceRepository;

        private readonly VoucherRepository voucherRepository;

        private readonly TourGuidenceService tourGuidenceService;
        private readonly VoucherService voucherService;

        public FutureTours()
        {
            InitializeComponent();
            DataContext = this;
            tourGuidenceRepository = new TourGuidenceRepository();
            voucherRepository = new VoucherRepository();
            tourGuidenceService = new TourGuidenceService();
            tourGuidences = new ObservableCollection<TourGuidence>(tourGuidenceService.GetAllFutureTours());
            voucherService = new VoucherService();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CancelTourGuidence(object sender, RoutedEventArgs e)
        {
            TourGuidence selectedGuidence = (TourGuidence)dataGrid.SelectedItem;

            if (tourGuidenceService.CheckValidDateForCancel(selectedGuidence.StartTime))
            {
                tourGuidenceService.UpdateCancelledField(selectedGuidence.Id);
                MessageBox.Show("Tour successfully cancelled!");
                voucherService.CreateForCancelledTourGuidence(selectedGuidence.Id);
                FutureTours window = new FutureTours();
                this.Close();
                window.Show();
            }
            else
            {
                MessageBox.Show("You can not cancel tour - less than 48 hours!");
            }
            

        }
    }
}
