using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
    /// Interaction logic for Guest2ComplexTourDisplay.xaml
    /// </summary>
    public partial class Guest2ComplexTourDisplay : Page
    {
        private string Username;

        private TourRequestService tourRequestService = new TourRequestService();
        public Guest2ComplexTourDisplay(string username)
        {
            InitializeComponent();
            DataContext = this;

            Username = username;

            Dictionary<int, List<TourRequest>> keyValuePairs = tourRequestService.GetComplexTourRequests(Username);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;

            foreach(int complexId in keyValuePairs.Keys)
            {
                List<TourRequest> tourRequests = keyValuePairs[complexId];

                GroupBox groupBox = new GroupBox();
                groupBox.Header = "Complex tour request " + complexId.ToString();
                groupBox.Foreground = (Brush)FindResource("TextColor");
                groupBox.Width = 1492;
                groupBox.Height = 250;

                DataGrid dataGrid = new DataGrid();
                dataGrid.Height = 200;
                dataGrid.Width = 1181;
                dataGrid.ColumnHeaderHeight = 25;
                dataGrid.RowHeight = 50;
                dataGrid.CanUserAddRows = false;
                dataGrid.AutoGenerateColumns = true;
                dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
                dataGrid.SelectionMode = DataGridSelectionMode.Single;
                dataGrid.Foreground = (Brush)FindResource("TextColor");
                dataGrid.Background = (Brush)FindResource("BackGrid");

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

                StackPanel innerStackPanel = new StackPanel();
                innerStackPanel.Orientation = Orientation.Horizontal;
                innerStackPanel.Children.Add(dataGrid);
                groupBox.Content = innerStackPanel;
                stackPanel.Children.Add(groupBox);
            }

            myGrid.Children.Add(stackPanel);
        }
    }
}
