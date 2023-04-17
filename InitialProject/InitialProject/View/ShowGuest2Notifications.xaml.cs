using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
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
    /// Interaction logic for ShowGuest2Notifications.xaml
    /// </summary>
    public partial class ShowGuest2Notifications : Window
    {
        private readonly TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();
        private readonly TourRepository tourRepository = new TourRepository();

        private readonly TourGuidenceService tourGuidanceService = new TourGuidenceService();
        private readonly TourReservationService tourReservationService = new TourReservationService();
        private readonly TourService tourService = new TourService();

        private readonly string username;
        public ShowGuest2Notifications(string username)
        {
            InitializeComponent();
            this.username = username;

            List<int> tourReservationIds = tourGuidanceService.NotifyGuestOfTourStarting(username);

            Grid grid = new Grid();

           
            for (int i = 0; i < tourReservationIds.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            int rowIndex = 0;

            foreach(int tourReservationId in tourReservationIds)
            {
                TourDisplayDTO tourDisplayDTO = tourService.GetTourForDisplay(tourReservationId);

                DataGrid dataGrid = new DataGrid();
                List<TourDisplayDTO> tourDisplayDTOs = new List<TourDisplayDTO>();
                tourDisplayDTOs.Add(tourDisplayDTO);
                dataGrid.ItemsSource = tourDisplayDTOs;

                DataGridTemplateColumn buttonColumn = new DataGridTemplateColumn();
                buttonColumn.Header = "Button Column";

                FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
                buttonFactory.SetValue(Button.ContentProperty, "Attend tour!");
                buttonFactory.SetValue(Button.TagProperty, tourReservationId);
                buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(Button_Click));

                buttonColumn.CellTemplate = new DataTemplate();
                buttonColumn.CellTemplate.VisualTree = buttonFactory;

                dataGrid.Columns.Add(buttonColumn);

                dataGrid.CanUserAddRows = false;

                Grid.SetRow(dataGrid, rowIndex); 

                grid.Children.Add(dataGrid);
                rowIndex++;
            }

            Content = grid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int tourGuidenceId = (int)((Button)sender).Tag;
            tourReservationService.ConfirmTourAttendance(username,tourGuidenceId);
        }
    }
}
