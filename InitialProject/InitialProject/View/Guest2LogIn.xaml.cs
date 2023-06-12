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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2LogIn.xaml
    /// </summary>
    public partial class Guest2LogIn
    {
        private readonly UserService userService;
        public Guest2LogIn()
        {
            InitializeComponent();
            frame.Content = new Guest2LogInPage();
        }
    }
}
