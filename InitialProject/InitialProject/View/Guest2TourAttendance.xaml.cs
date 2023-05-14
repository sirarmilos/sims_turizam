using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2TourAttendance.xaml
    /// </summary>
    public partial class Guest2TourAttendance : Page
    {
        private string Username;

        private TourGuidenceService tourGuidenceService = new TourGuidenceService();
        private TourReservationService tourReservationService = new TourReservationService();

        public Guest2TourAttendance(string username)
        {
            InitializeComponent();
            Username = username;
            DataContext = this;

            List<int> tourReservationIds = tourGuidenceService.GetTourReservationsForTracking(username);

            double topMargin = 50; 


            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical; 

 
            foreach (var tourReservationId in tourReservationIds)
            {
                Model.TourReservation tourReservation = tourReservationService.FindById(tourReservationId);
                List<Model.TourReservation> tourReservations = new List<Model.TourReservation>();

                TourAttendanceDTO attendanceDTO = tourGuidenceService.FindTourAttendanceDTO(tourReservationId);
                List<TourAttendanceDTO> tourAttendanceDTOs = new List<TourAttendanceDTO>();
                tourAttendanceDTOs.Add(attendanceDTO);


                GroupBox groupBox = new GroupBox();
                groupBox.Header = attendanceDTO.GuideUsername;
                groupBox.Width = 1492;
                groupBox.Height = 162;


                DataGrid dataGrid = new DataGrid();
                dataGrid.Height = 75;
                dataGrid.Width = 1181;
                dataGrid.CanUserAddRows = false;
                dataGrid.AutoGenerateColumns = false;

                dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("tourAttendanceDTOs") { Mode = BindingMode.OneWay });
                dataGrid.DataContext = new { tourAttendanceDTOs };

                DataGridTextColumn guideName = new DataGridTextColumn();
                guideName.Header = "Guide";
                guideName.Binding = new Binding("GuideUsername");
                //guideName.DisplayIndex = 0;
                dataGrid.Columns.Add(guideName);


                DataGridTextColumn date = new DataGridTextColumn();
                date.Header = "Date";
                date.Binding = new Binding("Date");
                dataGrid.Columns.Add(date);

                int br = 0;
                foreach (TourKeyPoint tourKeyPoint in tourAttendanceDTOs[0].TourKeyPoints)
                {
                    DataGridTextColumn tkp = new DataGridTextColumn();
                    tkp.Header = tourKeyPoint.KeyPointName;
                    tkp.Binding = new Binding("tourKeyPoint.Passed");
                    dataGrid.Columns.Add(tkp);
                    br++;
                }

                Button button = new Button();
                button.Tag = tourReservation.tourGuidenceId;
                button.Height = 40;
                button.Width = 189;
                button.Content = FindResource("StrRateGuide");
                button.Margin = new Thickness(55, -40, 0, 0);


                StackPanel innerStackPanel = new StackPanel();
                innerStackPanel.Orientation = Orientation.Horizontal;
                innerStackPanel.Children.Add(dataGrid);
                innerStackPanel.Children.Add(button);

                groupBox.Content = innerStackPanel;

                stackPanel.Children.Add(groupBox);
            }

            myGrid.Children.Add(stackPanel);


        }
    }
}
