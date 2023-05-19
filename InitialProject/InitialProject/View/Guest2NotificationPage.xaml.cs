using InitialProject.Dto;
using InitialProject.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2NotificationPage.xaml
    /// </summary>
    public partial class Guest2NotificationPage : Page
    {
        private string Username;

        private TourNotificationsService tourNotificationsService;

        private TourGuidenceService tourGuidenceService;

        private TourService tourService;

        private TourReservationService tourReservationService;

        public Guest2NotificationPage(string username)
        {
            InitializeComponent();
            DataContext = this;
            Username = username;

            tourNotificationsService = new TourNotificationsService();

            tourNotificationsService.Update();

            tourGuidenceService = new TourGuidenceService();

            tourService = new TourService();

            tourReservationService = new TourReservationService();

            List<TourGuidence> tourGuidenceDtos = new List<TourGuidence>();   
            foreach(TourNotifications tourNotifications in tourNotificationsService.NotifyOfNewTour(Username))
            {
                tourGuidenceDtos.Add(tourNotifications.TourGuidence);
            }





            dataGrid.ItemsSource = tourGuidenceDtos;


            List<TourRequest> tourRequests = new List<TourRequest>();

           // List<TourDisplayDTO> tourDisplayDTOsResult = new List<TourDisplayDTO>();

           /* foreach(TourDisplayDTO tourDisplayDTO1 in tourService.GetToursForDisplay())
            {
                foreach(TourNotifications tourNotifications in tourNotificationsService.NotifyOfNewTour(Username))
                {
                    if(tourDisplayDTO1.TourName.Equals(tourNotifications.TourGuidence.Tour.TourName) && tourDisplayDTO1.TourDate.Equals(tourNotifications.TourGuidence.StartTime))
                    {
                        tourDisplayDTOsResult.Add(tourDisplayDTO1);
                    }
                }
            }

            dataGrid.ItemsSource = tourDisplayDTOsResult;*/


            List<int> tourReservationIds = tourGuidenceService.NotifyGuestOfTourStarting(username);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;

            foreach (int tourReservationId in tourReservationIds)
            {
                TourDisplayDTO tourDisplayDTO = tourService.GetTourForDisplay(tourReservationId);
                List<TourDisplayDTO> tourDisplayDTOs = new List<TourDisplayDTO>();
                tourDisplayDTOs.Add(tourDisplayDTO);

                GroupBox groupBox = new GroupBox();
                groupBox.Header = "Tour that you booked has started";
                groupBox.Foreground = (Brush)FindResource("TextColor");
                groupBox.Width = 1492;
                groupBox.Height = 100;
                groupBox.Margin = new Thickness(0,10,0,0);


                DataGrid dataGrid = new DataGrid();
                dataGrid.Height = 55;
                dataGrid.Width = 1181;
                dataGrid.ColumnHeaderHeight = 25;
                dataGrid.RowHeight = 30;
                dataGrid.CanUserAddRows = false;
                dataGrid.AutoGenerateColumns = false;
                dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                dataGrid.SelectionMode = DataGridSelectionMode.Single;
                dataGrid.AutoGenerateColumns = true;


                dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("tourDisplayDTOs") { Mode = BindingMode.OneWay });
                dataGrid.DataContext = new { tourDisplayDTOs };

                StackPanel innerStackPanel = new StackPanel();
                innerStackPanel.Orientation = Orientation.Horizontal;
                innerStackPanel.Children.Add(dataGrid);

                groupBox.Content = innerStackPanel;

                stackPanel.Children.Add(groupBox);
            }

            foreach(KeyValuePair<int,int> pair in tourReservationService.AddedToTour(username))
            {
                TourDisplayDTO tourDisplayDTO = tourService.GetTourForDisplay(pair.Key);
                List<TourDisplayDTO> tourDisplayDTOs = new List<TourDisplayDTO>();
                tourDisplayDTOs.Add(tourDisplayDTO);

                GroupBox groupBox = new GroupBox();
                groupBox.Header = "You have been added to tour on key point:" + pair.Value.ToString();
                groupBox.Foreground = (Brush)FindResource("TextColor");
                groupBox.Width = 1492;
                groupBox.Height = 100;
                groupBox.Margin = new Thickness(0, 10, 0, 0);


                DataGrid dataGrid = new DataGrid();
                dataGrid.Height = 55;
                dataGrid.Width = 1181;
                dataGrid.ColumnHeaderHeight = 25;
                dataGrid.RowHeight = 30;
                dataGrid.CanUserAddRows = false;
                dataGrid.AutoGenerateColumns = false;
                dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                dataGrid.SelectionMode = DataGridSelectionMode.Single;
                dataGrid.AutoGenerateColumns = true;

                DataGridTemplateColumn buttonColumn = new DataGridTemplateColumn();
                buttonColumn.Header = "Button Column";

                FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
                buttonFactory.SetValue(Button.ContentProperty, "Attend tour!");
                buttonFactory.SetValue(Button.TagProperty, pair.Key);
                buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(Button_Click));

                buttonColumn.CellTemplate = new DataTemplate();
                buttonColumn.CellTemplate.VisualTree = buttonFactory;


                dataGrid.Columns.Add(buttonColumn);


                dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("tourDisplayDTOs") { Mode = BindingMode.OneWay });
                dataGrid.DataContext = new { tourDisplayDTOs };

                StackPanel innerStackPanel = new StackPanel();
                innerStackPanel.Orientation = Orientation.Horizontal;
                innerStackPanel.Children.Add(dataGrid);

                groupBox.Content = innerStackPanel;

                stackPanel.Children.Add(groupBox);

            }

            myGrid.Children.Add(stackPanel);


        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int tourReservationId = (int)((Button)sender).Tag;
            tourReservationService.ConfirmTourAttendance(Username, tourReservationId);
        }
    }
}
