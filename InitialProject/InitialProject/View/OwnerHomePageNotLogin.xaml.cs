using InitialProject.DTO;
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
    /// Interaction logic for OwnerHomePageNotLogin.xaml
    /// </summary>
    public partial class OwnerHomePageNotLogin : Window
    {
        public OwnerHomePageNotLogin()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void OwnerHomePageNotLogin_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OwnerHomePageNotLogin_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OwnerHomePageNotLogin window = new OwnerHomePageNotLogin();
            window.Show();
            Close();
        }

        private void OwnerLogin_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OwnerLogin_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OwnerLogin window = new OwnerLogin();
            window.ShowDialog();
        }
    }
}
