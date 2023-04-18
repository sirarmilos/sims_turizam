using InitialProject.Model;
using InitialProject.Repository;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
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

        public LoginForm()
        {
            InitializeComponent();

            DataContext = this;

            userService = new UserService();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            if(userService.IsUsernameExist(Username) == false)
            {
                tbUsername.Text = string.Empty;
                tbPassword.Text = string.Empty;
                MessageBox.Show("Username you entered does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbUsername.Focus();
            }
            else if(userService.IsPasswordCorrect(Username, Password) == false)
            {
                tbPassword.Text = string.Empty;
                MessageBox.Show("Password you entered is incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                tbPassword.Focus();
            }
            else
            {
                string type = userService.FindTypeByUsername(Username);

                if (type.Equals("owner") == true)
                {
                    OwnerStart window = new OwnerStart(Username);
                    window.Show();
                    Close();
                }
                if (type.Equals("guest1") == true)
                {
                    Guest1Start window = new Guest1Start(Username);
                    window.Show();
                    Close();
                }
                if (type.Equals("guide") == true)
                {
                    GuideStart window = new GuideStart(Username);
                    window.Show();
                    Close();
                }
                if (type.Equals("guest2") == true)
                {
                    Guest2Start window = new Guest2Start(Username);
                    window.Show();
                    Close();
                }

                MessageBox.Show("Welcome to the application " + Username + ".", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
