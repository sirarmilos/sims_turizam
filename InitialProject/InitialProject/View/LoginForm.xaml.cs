using InitialProject.Model;
using InitialProject.Repository;
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
        public User User { get; set; }

        private readonly UserRepository userRepository;

        private string username;
        private string password;
        private string type;
        private string superType;

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

        public string Type
        {
            get { return type; }
            set
            {
                type = value;
            }
        }

        public string SuperType
        {
            get { return superType; }
            set
            {
                superType = value;
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
            userRepository = new UserRepository();
        }

        private void Log_In(object sender, RoutedEventArgs e)
        {
            string temporaryType = null;
            temporaryType = userRepository.LoginUser(Username, Password);

            if(temporaryType.Equals("Greska") == true)
            {
                MessageBox.Show("Ne mozete se logovati.");
            }
            else
            {
                if(temporaryType.Equals("owner") == true)
                {
                    OwnerStart window = new OwnerStart(Username);
                    window.Show();
                    Close();
                }
                if (temporaryType.Equals("guest1") == true)
                {
                    Guest1Start window = new Guest1Start(Username);
                    window.Show();
                    Close();
                }
                if (temporaryType.Equals("guide") == true)
                {
                    GuideStart window = new GuideStart(Username);
                    window.Show();
                    Close();
                }
                if (temporaryType.Equals("guest2") == true)
                {
                    Guest2Start window = new Guest2Start(Username);
                    window.Show();
                    Close();
                }
            }
        }
    }
}
