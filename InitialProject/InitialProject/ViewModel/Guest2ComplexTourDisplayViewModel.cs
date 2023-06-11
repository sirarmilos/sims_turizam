using InitialProject.Model;
using InitialProject.Service;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Resources;


namespace InitialProject.ViewModel
{
    internal class Guest2ComplexTourDisplayViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private TourRequestService tourRequestService;

        private readonly Page _page;

        private string Username;

        private Guest2ComplexTourDisplay guest2ComplexTourDisplay;


        private readonly ResourceManager resourceManager;


        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guest2ComplexTourDisplayViewModel(Guest2ComplexTourDisplay guest2ComplexTourDisplay,ResourceDictionary resource)
        {

            resourceManager = Properties.Resources.ResourceManager;

            this.guest2ComplexTourDisplay = guest2ComplexTourDisplay;

            Username = UserClass.Username;

            tourRequestService = new TourRequestService();  

            Dictionary<int, List<TourRequest>> keyValuePairs = tourRequestService.GetComplexTourRequests(Username);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;

            foreach (int complexId in keyValuePairs.Keys)
            {
                List<TourRequest> tourRequests = keyValuePairs[complexId];
                int num = tourRequests.Count;

                GroupBox groupBox = new GroupBox();
                groupBox.Header = "Complex tour request " + complexId.ToString();



                //groupBox.Foreground = (Brush)FindResource("TextColor");
                groupBox.Width = 1400;
                groupBox.Height = num * 70 + 100;

                DataGrid dataGrid = new DataGrid();
                dataGrid.Height = 70 * num;
                dataGrid.Width = 1300;
                dataGrid.ColumnHeaderHeight = 25;
                dataGrid.RowHeight = 50;
                dataGrid.CanUserAddRows = false;
                dataGrid.AutoGenerateColumns = false;
                dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                dataGrid.SelectionMode = DataGridSelectionMode.Single;
                //dataGrid.Foreground = (Brush)FindResource("TextColor");
                //dataGrid.Background = (Brush)FindResource("BackGrid");
                dataGrid.Margin = new Thickness(35, 20, 30, 10);

                int colWidth = 1181 / 7;

                DataGridTextColumn columnCountry = new DataGridTextColumn();
                columnCountry.Header = "Country";
                columnCountry.Binding = new Binding("Location.Country");
                columnCountry.Width = colWidth;
                dataGrid.Columns.Add(columnCountry);

                DataGridTextColumn columnCity = new DataGridTextColumn();
                columnCity.Header = "City";
                columnCity.Binding = new Binding("Location.City");
                columnCity.Width = colWidth;
                dataGrid.Columns.Add(columnCity);

                DataGridTextColumn columnDescription = new DataGridTextColumn();
                columnDescription.Header = "Description";
                columnDescription.Binding = new Binding("Description");
                columnDescription.Width = colWidth;
                dataGrid.Columns.Add(columnDescription);

                DataGridTextColumn columnLanguage = new DataGridTextColumn();
                columnLanguage.Header = "Language";
                columnLanguage.Binding = new Binding("Language");
                columnLanguage.Width = colWidth;
                dataGrid.Columns.Add(columnLanguage);

                DataGridTextColumn columnGuestNum = new DataGridTextColumn();
                columnGuestNum.Header = "Number of guests";
                columnGuestNum.Binding = new Binding("GuestNumber");
                columnGuestNum.Width = colWidth;
                dataGrid.Columns.Add(columnGuestNum);

                DataGridTextColumn statusColumn = new DataGridTextColumn();
                statusColumn.Header = "Status";
                statusColumn.Binding = new Binding("Status");
                statusColumn.Width = colWidth;
                dataGrid.Columns.Add(statusColumn);

                DataGridTextColumn acceptedDateColumn = new DataGridTextColumn();
                acceptedDateColumn.Header = "Accepted date";
                acceptedDateColumn.Binding = new Binding("AcceptedDate");
                acceptedDateColumn.Width = colWidth;
                dataGrid.Columns.Add(acceptedDateColumn);


                dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("tourRequests") { Mode = BindingMode.OneWay });
                dataGrid.DataContext = new { tourRequests };

                Style cellStyle = new Style(typeof(DataGridCell));
                cellStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                cellStyle.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
                cellStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, 14.0));
                cellStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));



                foreach (DataGridColumn column in dataGrid.Columns)
                {
                    column.CellStyle = cellStyle;
                }


               // dataGrid.Background = (Brush)FindResource("BackGrid");
               //dataGrid.Foreground = (Brush)FindResource("TextColor");
                dataGrid.BorderThickness = new Thickness(1);
                //dataGrid.BorderBrush = Brushes.Gray;
                dataGrid.Margin = new Thickness(30, 0, 0, 0);

                dataGrid.ColumnHeaderStyle = new Style(typeof(DataGridColumnHeader));
                //dataGrid.ColumnHeaderStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightGray));


                StackPanel innerStackPanel = new StackPanel();
                innerStackPanel.Orientation = Orientation.Horizontal;
                innerStackPanel.Children.Add(dataGrid);
                groupBox.Margin = new Thickness(40, 0, 0, 0);
                groupBox.Content = innerStackPanel;
                stackPanel.Children.Add(groupBox);
            }

            this.guest2ComplexTourDisplay.myGrid.Children.Add(stackPanel);
            
        }
    }
}
