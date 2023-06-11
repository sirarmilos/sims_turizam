using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using InitialProject.Dto;
using InitialProject.Model;
using System.Windows.Data;
using InitialProject.View;

namespace InitialProject.ViewModel
{
    internal class Guest2NotificationPageViewModel : INotifyPropertyChanged
    {
        private string Username;

        private TourNotificationsService tourNotificationsService;

        private TourGuidenceService tourGuidenceService;

        private TourService tourService;

        private TourReservationService tourReservationService;

        private Guest2NotificationPage guest2NotificationPage;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guest2NotificationPageViewModel(Guest2NotificationPage guest2NotificationPage)
        {
            this.guest2NotificationPage = guest2NotificationPage;

            tourNotificationsService = new TourNotificationsService();

            tourNotificationsService.Update();

            tourGuidenceService = new TourGuidenceService();

            tourService = new TourService();

            tourReservationService = new TourReservationService();

            Username = UserClass.Username;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;

            foreach (TourNotifications tourNotifications in tourNotificationsService.NotifyOfNewTour(Username))
            {
                TourGuidence tourGuidence = tourNotifications.TourGuidence;
                List<TourGuidence> tourGuidences = new List<TourGuidence>();
                tourGuidences.Add(tourGuidence);

                GroupBox groupBox = new GroupBox();
                groupBox.Header = "New tour has been added, you may like it";
                //groupBox.Foreground = (Brush)FindResource("TextColor");
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

                DataGridTextColumn tourNameColumn = new DataGridTextColumn();
                tourNameColumn.Header = "Tour name";
                tourNameColumn.Binding = new Binding("Tour.TourName");
                dataGrid.Columns.Add(tourNameColumn);

                DataGridTextColumn countryColumn = new DataGridTextColumn();
                countryColumn.Header = "Country";
                countryColumn.Binding = new Binding("Tour.Location.Country");
                dataGrid.Columns.Add(countryColumn);

                DataGridTextColumn cityColumn = new DataGridTextColumn();
                cityColumn.Header = "City";
                cityColumn.Binding = new Binding("Tour.Location.City");
                dataGrid.Columns.Add(cityColumn);

                DataGridTextColumn descriptionColumn = new DataGridTextColumn();
                descriptionColumn.Header = "Description";
                descriptionColumn.Binding = new Binding("Tour.Description");
                dataGrid.Columns.Add(descriptionColumn);

                DataGridTextColumn languageColumn = new DataGridTextColumn();
                languageColumn.Header = "Language";
                languageColumn.Binding = new Binding("Tour.Language");
                dataGrid.Columns.Add(languageColumn);

                DataGridTextColumn slotsColumn = new DataGridTextColumn();
                slotsColumn.Header = "Free slots";
                slotsColumn.Binding = new Binding("Tour.MaxGuests");
                dataGrid.Columns.Add(slotsColumn);


                DataGridTextColumn durationColumn = new DataGridTextColumn();
                durationColumn.Header = "Duration";
                durationColumn.Binding = new Binding("Tour.Duration");
                dataGrid.Columns.Add(durationColumn);

                DataGridTextColumn dateColumn = new DataGridTextColumn();
                dateColumn.Header = "Date";
                dateColumn.Binding = new Binding("StartTime");
                dataGrid.Columns.Add(dateColumn);


                dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("tourGuidences") { Mode = BindingMode.OneWay });
                dataGrid.DataContext = new { tourGuidences };

                StackPanel innerStackPanel = new StackPanel();
                innerStackPanel.Orientation = Orientation.Horizontal;
                innerStackPanel.Children.Add(dataGrid);

                groupBox.Content = innerStackPanel;

                stackPanel.Children.Add(groupBox);
            }





            List<TourRequest> tourRequests = new List<TourRequest>();


            List<int> tourReservationIds = tourGuidenceService.NotifyGuestOfTourStarting(Username);

            foreach (int tourReservationId in tourReservationIds)
            {
                TourDisplayDTO tourDisplayDTO = tourService.GetTourForDisplay(tourReservationId);
                List<TourDisplayDTO> tourDisplayDTOs = new List<TourDisplayDTO>();
                tourDisplayDTOs.Add(tourDisplayDTO);

                GroupBox groupBox = new GroupBox();
                groupBox.Header = "Tour that you booked has started";
                //groupBox.Foreground = (Brush)FindResource("TextColor");
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

                DataGridTextColumn tourNameColumn = new DataGridTextColumn();
                tourNameColumn.Header = "Tour name";
                tourNameColumn.Binding = new Binding("TourName");
                dataGrid.Columns.Add(tourNameColumn);

                DataGridTextColumn countryColumn = new DataGridTextColumn();
                countryColumn.Header = "Country";
                countryColumn.Binding = new Binding("Location.Country");
                dataGrid.Columns.Add(countryColumn);

                DataGridTextColumn cityColumn = new DataGridTextColumn();
                cityColumn.Header = "City";
                cityColumn.Binding = new Binding("Location.City");
                dataGrid.Columns.Add(cityColumn);

                DataGridTextColumn descriptionColumn = new DataGridTextColumn();
                descriptionColumn.Header = "Description";
                descriptionColumn.Binding = new Binding("Description");
                dataGrid.Columns.Add(descriptionColumn);

                DataGridTextColumn languageColumn = new DataGridTextColumn();
                languageColumn.Header = "Language";
                languageColumn.Binding = new Binding("Language");
                dataGrid.Columns.Add(languageColumn);

                DataGridTextColumn slotsColumn = new DataGridTextColumn();
                slotsColumn.Header = "Free slots";
                slotsColumn.Binding = new Binding("FreeSlots");
                dataGrid.Columns.Add(slotsColumn);


                DataGridTextColumn durationColumn = new DataGridTextColumn();
                durationColumn.Header = "Duration";
                durationColumn.Binding = new Binding("Duration");
                dataGrid.Columns.Add(durationColumn);

                DataGridTextColumn dateColumn = new DataGridTextColumn();
                dateColumn.Header = "Date";
                dateColumn.Binding = new Binding("TourDate");
                dataGrid.Columns.Add(dateColumn);

                dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("tourDisplayDTOs") { Mode = BindingMode.OneWay });
                dataGrid.DataContext = new { tourDisplayDTOs };

                StackPanel innerStackPanel = new StackPanel();
                innerStackPanel.Orientation = Orientation.Horizontal;
                innerStackPanel.Children.Add(dataGrid);

                groupBox.Content = innerStackPanel;

                stackPanel.Children.Add(groupBox);
            }

            foreach (KeyValuePair<int, int> pair in tourReservationService.AddedToTour(Username))
            {
                TourDisplayDTO tourDisplayDTO = tourService.GetTourForDisplay(pair.Key);
                List<TourDisplayDTO> tourDisplayDTOs = new List<TourDisplayDTO>();
                tourDisplayDTOs.Add(tourDisplayDTO);

                GroupBox groupBox = new GroupBox();
                groupBox.Header = "You have been added to tour on key point:" + pair.Value.ToString();
                //groupBox.Foreground = (Brush)FindResource("TextColor");
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

                DataGridTextColumn tourNameColumn = new DataGridTextColumn();
                tourNameColumn.Header = "Name";
                tourNameColumn.Binding = new Binding("TourName");
                dataGrid.Columns.Add(tourNameColumn);

                DataGridTextColumn countryColumn = new DataGridTextColumn();
                countryColumn.Header = "Country";
                countryColumn.Binding = new Binding("Location.Country");
                dataGrid.Columns.Add(countryColumn);

                DataGridTextColumn cityColumn = new DataGridTextColumn();
                cityColumn.Header = "City";
                cityColumn.Binding = new Binding("Location.City");
                dataGrid.Columns.Add(cityColumn);

                DataGridTextColumn descriptionColumn = new DataGridTextColumn();
                descriptionColumn.Header = "Description";
                descriptionColumn.Binding = new Binding("Description");
                dataGrid.Columns.Add(descriptionColumn);

                DataGridTextColumn languageColumn = new DataGridTextColumn();
                languageColumn.Header = "Language";
                languageColumn.Binding = new Binding("Language");
                dataGrid.Columns.Add(languageColumn);

                DataGridTextColumn slotsColumn = new DataGridTextColumn();
                slotsColumn.Header = "Free slots";
                slotsColumn.Binding = new Binding("FreeSlots");
                dataGrid.Columns.Add(slotsColumn);


                DataGridTextColumn durationColumn = new DataGridTextColumn();
                durationColumn.Header = "Duration";
                durationColumn.Binding = new Binding("Duration");
                dataGrid.Columns.Add(durationColumn);

                DataGridTextColumn dateColumn = new DataGridTextColumn();
                dateColumn.Header = "Date";
                dateColumn.Binding = new Binding("TourDate");
                dataGrid.Columns.Add(dateColumn);

                DataGridTemplateColumn buttonColumn = new DataGridTemplateColumn();
                //buttonColumn.Header = "Button Column";

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

            this.guest2NotificationPage.myGrid.Children.Add(stackPanel);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int tourReservationId = (int)((Button)sender).Tag;
            tourReservationService.ConfirmTourAttendance(Username, tourReservationId);
        }
    }
}
