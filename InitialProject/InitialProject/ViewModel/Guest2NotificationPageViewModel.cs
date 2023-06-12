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
                groupBox.Header = Application.Current.Resources["StrAddedTour"] as string;
                //groupBox.Foreground = (Brush)FindResource("TextColor");
                groupBox.Width = 1492;
                groupBox.Height = 140;
                groupBox.Margin = new Thickness(0, 10, 0, 0);

                int colWidth = 1181 / 8;

                DataGrid dataGrid = new DataGrid();
                dataGrid.Height = 120;
                dataGrid.Width = 1181;
                dataGrid.ColumnHeaderHeight = 50;
                dataGrid.RowHeight = 50;
                dataGrid.CanUserAddRows = false;
                dataGrid.AutoGenerateColumns = false;
                dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                dataGrid.SelectionMode = DataGridSelectionMode.Single;

                DataGridTextColumn tourNameColumn = new DataGridTextColumn();
                tourNameColumn.Header = Application.Current.Resources["StrName"] as string;
                tourNameColumn.Binding = new Binding("Tour.TourName");
                tourNameColumn.Width = colWidth;
                dataGrid.Columns.Add(tourNameColumn);

                DataGridTextColumn countryColumn = new DataGridTextColumn();
                countryColumn.Header = Application.Current.Resources["StrCountry"] as string;
                countryColumn.Binding = new Binding("Tour.Location.Country");
                countryColumn.Width = colWidth;
                dataGrid.Columns.Add(countryColumn);

                DataGridTextColumn cityColumn = new DataGridTextColumn();
                cityColumn.Header = Application.Current.Resources["StrCity"] as string;
                cityColumn.Binding = new Binding("Tour.Location.City");
                cityColumn.Width = colWidth;    
                dataGrid.Columns.Add(cityColumn);

                DataGridTextColumn descriptionColumn = new DataGridTextColumn();
                descriptionColumn.Header = Application.Current.Resources["StrDescription"] as string;
                descriptionColumn.Binding = new Binding("Tour.Description");
                descriptionColumn.Width = colWidth;
                dataGrid.Columns.Add(descriptionColumn);

                DataGridTextColumn languageColumn = new DataGridTextColumn();
                languageColumn.Header = Application.Current.Resources["StrLanguage"] as string;
                languageColumn.Binding = new Binding("Tour.Language");
                languageColumn.Width = colWidth;
                dataGrid.Columns.Add(languageColumn);

                DataGridTextColumn slotsColumn = new DataGridTextColumn();
                slotsColumn.Header = Application.Current.Resources["StrFreeSlots"] as string;
                slotsColumn.Binding = new Binding("Tour.MaxGuests");
                slotsColumn.Width = colWidth;
                dataGrid.Columns.Add(slotsColumn);


                DataGridTextColumn durationColumn = new DataGridTextColumn();
                durationColumn.Header = Application.Current.Resources["StrTourDuration"] as string;
                durationColumn.Binding = new Binding("Tour.Duration");
                durationColumn.Width = colWidth;
                dataGrid.Columns.Add(durationColumn);

                DataGridTextColumn dateColumn = new DataGridTextColumn();
                dateColumn.Header = Application.Current.Resources["StrDate"] as string;
                dateColumn.Binding = new Binding("StartTime");
                dateColumn.Width = colWidth;
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
                groupBox.Header = Application.Current.Resources["StrTourStart"] as string;
                //groupBox.Foreground = (Brush)FindResource("TextColor");
                groupBox.Width = 1492;
                groupBox.Height = 140;
                groupBox.Margin = new Thickness(0, 10, 0, 0);

                int colWidth = 1181 / 8;

                DataGrid dataGrid = new DataGrid();
                dataGrid.Height = 120;
                dataGrid.Width = 1181;
                dataGrid.ColumnHeaderHeight = 50;
                dataGrid.RowHeight = 50;
                dataGrid.CanUserAddRows = false;
                dataGrid.AutoGenerateColumns = false;
                dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                dataGrid.SelectionMode = DataGridSelectionMode.Single;

                DataGridTextColumn tourNameColumn = new DataGridTextColumn();
                tourNameColumn.Header = Application.Current.Resources["StrName"] as string;
                tourNameColumn.Binding = new Binding("TourName");
                tourNameColumn.Width = colWidth;
                dataGrid.Columns.Add(tourNameColumn);

                DataGridTextColumn countryColumn = new DataGridTextColumn();
                countryColumn.Header = Application.Current.Resources["StrCountry"] as string;
                countryColumn.Binding = new Binding("Location.Country");
                countryColumn.Width = colWidth;
                dataGrid.Columns.Add(countryColumn);

                DataGridTextColumn cityColumn = new DataGridTextColumn();
                cityColumn.Header = Application.Current.Resources["StrCity"] as string;
                cityColumn.Binding = new Binding("Location.City");
                cityColumn.Width = colWidth;
                dataGrid.Columns.Add(cityColumn);

                DataGridTextColumn descriptionColumn = new DataGridTextColumn();
                descriptionColumn.Header = Application.Current.Resources["StrDescription"] as string;
                descriptionColumn.Binding = new Binding("Description");
                descriptionColumn.Width = colWidth;
                dataGrid.Columns.Add(descriptionColumn);

                DataGridTextColumn languageColumn = new DataGridTextColumn();
                languageColumn.Header = Application.Current.Resources["StrLanguage"] as string;
                languageColumn.Binding = new Binding("Language");
                languageColumn.Width = colWidth;
                dataGrid.Columns.Add(languageColumn);

                DataGridTextColumn slotsColumn = new DataGridTextColumn();
                slotsColumn.Header = Application.Current.Resources["StrFreeSlots"] as string;
                slotsColumn.Binding = new Binding("FreeSlots");
                slotsColumn.Width = colWidth;
                dataGrid.Columns.Add(slotsColumn);


                DataGridTextColumn durationColumn = new DataGridTextColumn();
                durationColumn.Header = Application.Current.Resources["StrTourDuration"] as string;
                durationColumn.Binding = new Binding("Duration");
                durationColumn.Width = colWidth;
                dataGrid.Columns.Add(durationColumn);

                DataGridTextColumn dateColumn = new DataGridTextColumn();
                dateColumn.Header = Application.Current.Resources["StrDate"] as string;
                dateColumn.Binding = new Binding("TourDate");
                dateColumn.Width = colWidth;
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
                groupBox.Header = Application.Current.Resources["StrAddedToTour"] as string + pair.Value.ToString();
                //groupBox.Foreground = (Brush)FindResource("TextColor");
                groupBox.Width = 1492;
                groupBox.Height = 140;
                groupBox.Margin = new Thickness(0, 10, 0, 0);

                int colWidth = 1181 / 8;

                DataGrid dataGrid = new DataGrid();
                dataGrid.Height = 120;
                dataGrid.Width = 1181;
                dataGrid.ColumnHeaderHeight = 50;
                dataGrid.RowHeight = 50;
                dataGrid.CanUserAddRows = false;
                dataGrid.AutoGenerateColumns = false;
                dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                dataGrid.SelectionMode = DataGridSelectionMode.Single;

                DataGridTextColumn tourNameColumn = new DataGridTextColumn();
                tourNameColumn.Header = Application.Current.Resources["StrName"] as string;
                tourNameColumn.Binding = new Binding("TourName");
                tourNameColumn.Width = colWidth;
                dataGrid.Columns.Add(tourNameColumn);

                DataGridTextColumn countryColumn = new DataGridTextColumn();
                countryColumn.Header = Application.Current.Resources["StrCountry"] as string;
                countryColumn.Binding = new Binding("Location.Country");
                countryColumn.Width = colWidth;
                dataGrid.Columns.Add(countryColumn);

                DataGridTextColumn cityColumn = new DataGridTextColumn();
                cityColumn.Header = Application.Current.Resources["StrCity"] as string;
                cityColumn.Binding = new Binding("Location.City");
                cityColumn.Width = colWidth;
                dataGrid.Columns.Add(cityColumn);

                DataGridTextColumn descriptionColumn = new DataGridTextColumn();
                descriptionColumn.Header = Application.Current.Resources["StrDescription"] as string;
                descriptionColumn.Binding = new Binding("Description");
                descriptionColumn.Width = colWidth;
                dataGrid.Columns.Add(descriptionColumn);

                DataGridTextColumn languageColumn = new DataGridTextColumn();
                languageColumn.Header = Application.Current.Resources["StrLanguage"] as string;
                languageColumn.Binding = new Binding("Language");
                languageColumn.Width = colWidth;
                dataGrid.Columns.Add(languageColumn);

                DataGridTextColumn slotsColumn = new DataGridTextColumn();
                slotsColumn.Header = Application.Current.Resources["StrFreeSlots"] as string;
                slotsColumn.Binding = new Binding("FreeSlots");
                slotsColumn.Width = colWidth;
                dataGrid.Columns.Add(slotsColumn);


                DataGridTextColumn durationColumn = new DataGridTextColumn();
                durationColumn.Header = Application.Current.Resources["StrTourDuration"] as string;
                durationColumn.Binding = new Binding("Duration");
                durationColumn.Width = colWidth;
                dataGrid.Columns.Add(durationColumn);

                DataGridTextColumn dateColumn = new DataGridTextColumn();
                dateColumn.Header = Application.Current.Resources["StrDate"] as string;
                dateColumn.Binding = new Binding("TourDate");
                dateColumn.Width = colWidth;
                dataGrid.Columns.Add(dateColumn);

                Button button = new Button();
                button.Tag = pair.Key;
                button.Height = 40;
                button.Width = 200;
                button.Content = Application.Current.Resources["StrTourAttend"] as string;
                button.Margin = new Thickness(45, 0, 0, 0);
                button.FontSize = 28;
                button.HorizontalAlignment = HorizontalAlignment.Center;
                button.VerticalAlignment = VerticalAlignment.Center;
                // button.Background = (Brush)FindResource("ButtonColor");
                // button.Foreground = (Brush)FindResource("ButtonText");

                button.AddHandler(Button.ClickEvent, new RoutedEventHandler(Button_Click));



                dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("tourDisplayDTOs") { Mode = BindingMode.OneWay });
                dataGrid.DataContext = new { tourDisplayDTOs };

                StackPanel innerStackPanel = new StackPanel();
                innerStackPanel.Orientation = Orientation.Horizontal;
                innerStackPanel.Children.Add(dataGrid);
                innerStackPanel.Children.Add(button);

                groupBox.Content = innerStackPanel;

                stackPanel.Children.Add(groupBox);
            }

            AddRequestedTour(stackPanel);

            this.guest2NotificationPage.myGrid.Children.Add(stackPanel);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int tourReservationId = (int)((Button)sender).Tag;
            tourReservationService.ConfirmTourAttendance(Username, tourReservationId);
            Guest2TourAttendance guest2TourAttendance = new Guest2TourAttendance();
            guest2NotificationPage.NavigationService.Navigate(guest2TourAttendance);
        }

        private void AddRequestedTour(StackPanel stackPanel)
        {
            GroupBox groupBox = new GroupBox(); 
            groupBox.Header = Application.Current.Resources["StrConfirmedTour"] as string; ;
            //groupBox.Foreground = (Brush)FindResource("TextColor");
            groupBox.Width = 1492;
            groupBox.Height = 140;
            groupBox.Margin = new Thickness(0, 10, 0, 0);

            int colWidth = 1181 / 8;

            DataGrid dataGrid = new DataGrid();
            dataGrid.Height = 120;
            dataGrid.Width = 1181;
            dataGrid.ColumnHeaderHeight = 50;
            dataGrid.RowHeight = 50;
            dataGrid.CanUserAddRows = false;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            dataGrid.SelectionMode = DataGridSelectionMode.Single;

            DataGridTextColumn tourNameColumn = new DataGridTextColumn();
            tourNameColumn.Header = Application.Current.Resources["StrName"] as string;
            tourNameColumn.Binding = new Binding("TourName");
            tourNameColumn.Width = colWidth;
            dataGrid.Columns.Add(tourNameColumn);

            DataGridTextColumn countryColumn = new DataGridTextColumn();
            countryColumn.Header = Application.Current.Resources["StrCountry"] as string;
            countryColumn.Binding = new Binding("Location.Country");
            countryColumn.Width = colWidth;
            dataGrid.Columns.Add(countryColumn);

            DataGridTextColumn cityColumn = new DataGridTextColumn();
            cityColumn.Header = Application.Current.Resources["StrCity"] as string;
            cityColumn.Binding = new Binding("Location.City");
            cityColumn.Width = colWidth;
            dataGrid.Columns.Add(cityColumn);

            DataGridTextColumn descriptionColumn = new DataGridTextColumn();
            descriptionColumn.Header = Application.Current.Resources["StrDescription"] as string;
            descriptionColumn.Binding = new Binding("Description");
            descriptionColumn.Width = colWidth;
            dataGrid.Columns.Add(descriptionColumn);

            DataGridTextColumn languageColumn = new DataGridTextColumn();
            languageColumn.Header = Application.Current.Resources["StrLanguage"] as string;
            languageColumn.Binding = new Binding("Language");
            languageColumn.Width = colWidth;
            dataGrid.Columns.Add(languageColumn);

            DataGridTextColumn slotsColumn = new DataGridTextColumn();
            slotsColumn.Header = Application.Current.Resources["StrFreeSlots"] as string;
            slotsColumn.Binding = new Binding("FreeSlots");
            slotsColumn.Width = colWidth;
            dataGrid.Columns.Add(slotsColumn);


            DataGridTextColumn durationColumn = new DataGridTextColumn();
            durationColumn.Header = Application.Current.Resources["StrTourDuration"] as string;
            durationColumn.Binding = new Binding("Duration");
            durationColumn.Width = colWidth;
            dataGrid.Columns.Add(durationColumn);

            DataGridTextColumn dateColumn = new DataGridTextColumn();
            dateColumn.Header = Application.Current.Resources["StrDate"] as string;
            dateColumn.Binding = new Binding("TourDate");
            dateColumn.Width = colWidth;
            dataGrid.Columns.Add(dateColumn);

            List<TourDisplayDTO> listTours = new List<TourDisplayDTO>();
            TourDisplayDTO tourDisplayDTO1 = new TourDisplayDTO();
            tourDisplayDTO1.TourName = "Tour";

            Location location = new Location(1000, "Country", "City","Address", 0, 0);

            tourDisplayDTO1.Location = location;
            tourDisplayDTO1.Description = "Description";
            tourDisplayDTO1.Language = Language.ALL;
            tourDisplayDTO1.FreeSlots = 100;
            tourDisplayDTO1.Duration = 100;
            tourDisplayDTO1.TourDate = DateTime.Now;

            listTours.Add(tourDisplayDTO1);
            dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("listTours") { Mode = BindingMode.OneWay });
            dataGrid.DataContext = new { listTours };


            StackPanel innerStackPanel = new StackPanel();
            innerStackPanel.Orientation = Orientation.Horizontal;
            innerStackPanel.Children.Add(dataGrid);

            groupBox.Content = innerStackPanel;

            stackPanel.Children.Add(groupBox);
        }
    }
}
