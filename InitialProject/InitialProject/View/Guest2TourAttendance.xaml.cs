﻿using InitialProject.Dto;
using InitialProject.IRepository;
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
using System.Windows.Controls.Primitives;
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
                groupBox.Header = attendanceDTO.TourKeyPoints[0].TourGuidence.Tour.TourName;
                groupBox.Foreground = (Brush)FindResource("TextColor");
                groupBox.Width = 1492;
                groupBox.Height = 162;


                DataGrid dataGrid = new DataGrid();
                dataGrid.Height = 75;
                dataGrid.Width = 1181;
                dataGrid.ColumnHeaderHeight = 25;
                dataGrid.RowHeight = 50;
                dataGrid.CanUserAddRows = false;
                dataGrid.AutoGenerateColumns = false;
                dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                dataGrid.SelectionMode = DataGridSelectionMode.Single;
                dataGrid.Foreground = (Brush)FindResource("TextColor");
                dataGrid.Background = (Brush)FindResource("BackGrid");
                

                dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("tourAttendanceDTOs") { Mode = BindingMode.OneWay });
                dataGrid.DataContext = new { tourAttendanceDTOs };

                DataGridTextColumn guideName = new DataGridTextColumn();
                guideName.Header = "Guide";
                guideName.Width = 300;
                guideName.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
                guideName.Binding = new Binding("GuideUsername");
                //guideName.DisplayIndex = 0;
                dataGrid.Columns.Add(guideName);

           

                DataGridTextColumn date = new DataGridTextColumn();
                date.Header = "Date";
                date.Width = 300;
                date.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
                date.Binding = new Binding("Date");
                dataGrid.Columns.Add(date);

                double tkpWidth = (dataGrid.Width - 600) / tourAttendanceDTOs[0].TourKeyPoints.Count;

                int br = 0;
                foreach (TourKeyPoint tourKeyPoint in tourAttendanceDTOs[0].TourKeyPoints)
                {
                    DataGridTextColumn tkp = new DataGridTextColumn();
                    tkp.Header = tourKeyPoint.KeyPointName;
                    tkp.Width = tkpWidth-1;
                   
                    tkp.Binding = new Binding("TourKeyPoints[0].Passed");
                    tkp.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Left) } };
                    dataGrid.Columns.Add(tkp);
                    br++;

                }

                Button button = new Button();
                button.Tag = tourReservation.tourGuidenceId;
                button.Height = 40;
                button.Width = 189;
                button.Content = FindResource("StrRateGuide");
                button.Margin = new Thickness(45, -40, 0, 0);
                button.FontSize = 28;
                button.HorizontalAlignment = HorizontalAlignment.Center;
                button.VerticalAlignment = VerticalAlignment.Center;

                button.AddHandler(Button.ClickEvent, new RoutedEventHandler(RateTour));

                Label label = new Label();
                label.Content = FindResource("StrTourStillActive");
                label.Foreground = Brushes.Red;
                label.Margin = new Thickness(-160,70, 0, 0);
                label.FontSize = 20;


                TourGuidence tourGuidence = tourGuidenceService.FindById(tourReservation.tourGuidenceId);

                if (tourGuidence.Finished == false)
                {
                    button.IsEnabled = false;
                    label.Visibility = Visibility.Visible;
                }
                else
                {
                    button.IsEnabled = true;
                    label.Visibility = Visibility.Hidden;
                }

                Style cellStyle = new Style(typeof(DataGridCell));
                cellStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                cellStyle.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
                cellStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, 14.0));
                cellStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty,VerticalAlignment.Center));

             

                foreach (DataGridColumn column in dataGrid.Columns)
                {
                    column.CellStyle = cellStyle;
                }


                //dataGrid.Background = Brushes.White;

               // dataGrid.BorderThickness = new Thickness(1);
                //dataGrid.BorderBrush = Brushes.Gray;
               // dataGrid.Margin = new Thickness(30,0,0,0);

                //dataGrid.ColumnHeaderStyle = new Style(typeof(DataGridColumnHeader));
               // dataGrid.ColumnHeaderStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightGray));


                StackPanel innerStackPanel = new StackPanel();
                innerStackPanel.Orientation = Orientation.Horizontal;
                innerStackPanel.Children.Add(dataGrid);
                innerStackPanel.Children.Add(button);
                innerStackPanel.Children.Add(label);

                groupBox.Content = innerStackPanel;



                stackPanel.Children.Add(groupBox);

            }

            myGrid.Children.Add(stackPanel);


        }

        private void RateTour(object sender, RoutedEventArgs e)
        {
            int tourGuidenceId = (int)((Button)sender).Tag;
            Guest2RateTourAndGuide guest2RateTourAndGuide = new Guest2RateTourAndGuide();
            NavigationService.Navigate(guest2RateTourAndGuide);
        }

    }
}
