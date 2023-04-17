using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Repository;
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
    /// Interaction logic for ShowTourAttendance.xaml
    /// </summary>
    /// 
    public partial class ShowTourAttendance : Window
    {
        private readonly string username;

        private readonly TourGuidenceRepository tourGuidenceRepository = new TourGuidenceRepository();
        private readonly TourReservationRepository tourReservationRepository = new TourReservationRepository();
        private readonly TourRepository tourRepository = new TourRepository();

        public ShowTourAttendance(string username)
        {
            InitializeComponent();
            this.username = username;

            List<int> tourReservationIds = tourGuidenceRepository.GetTourReservationsForTracking(username);

            Grid grid = new Grid();


            for (int i = 0; i < tourReservationIds.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            int rowIndex = 0;

            foreach (int tourReservationId in tourReservationIds)
            {
               Model.TourReservation tourReservation = tourReservationRepository.GetById(tourReservationId);

                DataGrid dataGrid = new DataGrid();

                List<Model.TourReservation> tourReservationDisplay = new List<Model.TourReservation>();

                tourReservationDisplay.Add(tourReservation);

                dataGrid.ItemsSource = tourReservationDisplay;

                DataGridTemplateColumn buttonColumn = new DataGridTemplateColumn();
                buttonColumn.Header = "Button Column";

                FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
                buttonFactory.SetValue(Button.ContentProperty, "Rate guide and tour");
                buttonFactory.SetValue(Button.TagProperty, tourReservation.tourGuidenceId);

                TourGuidence tourGuidence = tourGuidenceRepository.GetById(tourReservation.tourGuidenceId);
                if (tourGuidence.Finished == false)
                {
                    buttonFactory.SetValue(Button.IsEnabledProperty, false);
                }
                else
                {
                    buttonFactory.SetValue(Button.IsEnabledProperty, true);
                }


                buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(RateTour));

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

        private void RateTour(object sender, RoutedEventArgs e)
        {
            int tourGuidenceId = (int)((Button)sender).Tag;
            string guideUsername = tourGuidenceRepository.GetGuide(tourGuidenceId);
            View.RateGuide window = new View.RateGuide(username,tourGuidenceId,guideUsername);
            window.Show();
        }



    }

}
