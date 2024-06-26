﻿using InitialProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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
    /// Interaction logic for Guest2ProfilePreview.xaml
    /// </summary>
    public partial class Guest2ProfilePreview : Page
    {
        public Guest2ProfilePreview(string username)
        {
            InitializeComponent();
            this.DataContext = new Guest2InfoViewModel(username);
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();   
            loginForm.Show();   
        }
    }
}
