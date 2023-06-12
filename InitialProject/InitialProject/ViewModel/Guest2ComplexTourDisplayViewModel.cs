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
                groupBox.Header = Application.Current.Resources["StrComplTourReq"] as string + complexId.ToString();



                //groupBox.Foreground = (Brush)FindResource("TextColor");
                groupBox.Width = 1492;
                groupBox.Height = 40 + 40 * num + 100;
                groupBox.Margin = new Thickness(-150,0,0,0);

                DataGrid dataGrid = new DataGrid();
                dataGrid.Height = 40+ 40 * num;
                dataGrid.Width = 1300;
                dataGrid.ColumnHeaderHeight = 40;
                dataGrid.RowHeight = 40;
                dataGrid.CanUserAddRows = false;
                dataGrid.AutoGenerateColumns = false;
                dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                dataGrid.SelectionMode = DataGridSelectionMode.Single;
                //dataGrid.Foreground = (Brush)FindResource("TextColor");
                //dataGrid.Background = (Brush)FindResource("BackGrid");
                dataGrid.Margin = new Thickness(0, 0, 0, 0);

                int colWidth = 1300 / 7;


                DataGridTextColumn columnCountry = new DataGridTextColumn();
                columnCountry.Header = Application.Current.Resources["StrCountry"] as string;
                columnCountry.Binding = new Binding("Location.Country");
                columnCountry.Width = colWidth;
                dataGrid.Columns.Add(columnCountry);

                DataGridTextColumn columnCity = new DataGridTextColumn();
                columnCity.Header = Application.Current.Resources["StrCity"] as string;
                columnCity.Binding = new Binding("Location.City");
                columnCity.Width = colWidth;
                dataGrid.Columns.Add(columnCity);

                DataGridTextColumn columnDescription = new DataGridTextColumn();
                columnDescription.Header = Application.Current.Resources["StrDescription"] as string;
                columnDescription.Binding = new Binding("Description");
                columnDescription.Width = colWidth;
                dataGrid.Columns.Add(columnDescription);

                DataGridTextColumn columnLanguage = new DataGridTextColumn();
                columnLanguage.Header = Application.Current.Resources["StrLanguage"] as string;
                columnLanguage.Binding = new Binding("Language");
                columnLanguage.Width = colWidth;
                dataGrid.Columns.Add(columnLanguage);

                DataGridTextColumn columnGuestNum = new DataGridTextColumn(); 
                columnGuestNum.Header = Application.Current.Resources["StrGuestNumber"] as string;
                columnGuestNum.Binding = new Binding("GuestNumber");
                columnGuestNum.Width = colWidth;
                dataGrid.Columns.Add(columnGuestNum);

                DataGridTextColumn statusColumn = new DataGridTextColumn();
                statusColumn.Header = "Status";
                statusColumn.Binding = new Binding("Status");
                statusColumn.Width = colWidth;
                dataGrid.Columns.Add(statusColumn);

                DataGridTextColumn acceptedDateColumn = new DataGridTextColumn();
                columnGuestNum.Header = Application.Current.Resources["StrAcceptedDate"] as string;
                acceptedDateColumn.Binding = new Binding("AcceptedDate");
                acceptedDateColumn.Width = colWidth;
                dataGrid.Columns.Add(acceptedDateColumn);


                dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("tourRequests") { Mode = BindingMode.OneWay });
                dataGrid.DataContext = new { tourRequests };


                StackPanel innerStackPanel = new StackPanel();
                innerStackPanel.Orientation = Orientation.Horizontal;
                innerStackPanel.Children.Add(dataGrid);
                groupBox.Margin = new Thickness(0, 0, 0, 0);
                groupBox.Content = innerStackPanel;
                stackPanel.Children.Add(groupBox);
            }

            this.guest2ComplexTourDisplay.myGrid.Children.Add(stackPanel);
            
        }
    }
}
