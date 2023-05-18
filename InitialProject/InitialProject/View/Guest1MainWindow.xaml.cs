using System.Windows;

namespace InitialProject.View
{
    public partial class Guest1MainWindow : Window
    {
        public Guest1MainWindow(string guest1Username)
        {

            InitializeComponent();
            MainFrame.Content = new SearchAndShowAccommodations(guest1Username, null);

        }
    }
}
