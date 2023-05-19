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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        private readonly UserService userService;
        private readonly TourGuidenceService tourGuidenceService;

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

            labelErrorUsername.Visibility = Visibility.Hidden;
            labelErrorPassword.Visibility = Visibility.Hidden;

            if (userService.IsUsernameExist(Username) == false)
            {
                /*tbUsername.Text = string.Empty;
                pbPassword.Password = string.Empty;*/
                labelErrorUsername.Visibility = Visibility.Visible;
                tbUsername.Focus();
            }
            else if (userService.IsPasswordCorrect(Username, Password) == false)
            {
                // pbPassword.Password = string.Empty;
                labelErrorPassword.Visibility = Visibility.Visible;
                pbPassword.Focus();
            }
            else
            {
                string type = userService.FindTypeByUsername(Username);

                if (type.Equals("owner") == true)
                {
                    AccommodationStart window = new AccommodationStart(Username);
                    window.Show();
                    Close();
                }
                if (type.Equals("guest1") == true)
                {
                    Guest1MainWindow window = new Guest1MainWindow(Username);
                    window.Show();
                    Close();
                    return;
                }
                if (type.Equals("guide") == true)
                {
                    GuideStart window = new GuideStart(Username);
                    window.Show();
                    Close();

                   /* TourGuidence tg = tourGuidenceService.CheckIfStartedAndNotFinished();
                    if (tg != null)
                    {
                        GuideStart2 window1 = new GuideStart2(Username, tg);
                        window1.Show();
                        Close();
                    }
                    else
                    {
                        GuideStart1 window2 = new GuideStart1(Username);
                        window2.Show();
                        Close();
                    }*/
                    
                }
                if (type.Equals("guest2") == true)
                {
                    //Guest2Start window = new Guest2Start(Username);

                    Guest2MainWindow window = new Guest2MainWindow(Username);
                    window.Show();
                    Close();
                }

                MessageBox.Show("Welcome to the application " + Username + ".", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public LoginForm()
        {
            InitializeComponent();

            DataContext = this;

            userService = new UserService();

            tourGuidenceService = new TourGuidenceService();

            userService.CheckRecentlyRenovatedAccommodation();

            userService.CheckUsersSuperGuestStatus();

            tbUsername.Focus();

            labelErrorUsername.Visibility = Visibility.Hidden;
            labelErrorPassword.Visibility = Visibility.Hidden;
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
