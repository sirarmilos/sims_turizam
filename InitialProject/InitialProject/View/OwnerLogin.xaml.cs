using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Shell;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerLogin.xaml
    /// </summary>
    public partial class OwnerLogin : Window
    {
        private readonly UserService userService;

        private string username;
        private string password;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Login_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Login_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Password = pbPassword.Password;

            if (userService.IsUsernameExist(Username) == false)
            {
                tbUsername.Text = string.Empty;
                pbPassword.Password = string.Empty;
                // tbPassword.Text = string.Empty;
                MessageBox.Show("Username you entered does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbUsername.Focus();
            }
            else if (userService.IsPasswordCorrect(Username, Password) == false)
            {
                pbPassword.Password = string.Empty;
                // tbPassword.Text = string.Empty;
                MessageBox.Show("Password you entered is incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                pbPassword.Focus();
                // tbPassword.Focus();
            }
            else
            {
                string type = userService.FindTypeByUsername(Username);

                if (type.Equals("owner") == false)
                {
                    tbUsername.Text = string.Empty;
                    pbPassword.Password = string.Empty;
                    // tbPassword.Text = string.Empty;
                    MessageBox.Show("User who wants to log in is not the owner.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    tbUsername.Focus();
                }
                else
                {
                    AccommodationStart window = new AccommodationStart(Username);
                    window.Show();
                    Close();
                    MessageBox.Show("Welcome to the application " + Username + ".", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public OwnerLogin()
        {
            InitializeComponent();

            DataContext = this;

            userService = new UserService();

            userService.CheckRecentlyRenovatedAccommodation();
        }

        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var label = (Label)sender;
                Keyboard.Focus(label.Target);
            }
        }
    }
}
