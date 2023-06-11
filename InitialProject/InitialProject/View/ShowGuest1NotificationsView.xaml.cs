using GalaSoft.MvvmLight.Command;
using InitialProject.DTO;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace InitialProject.View
{
    public partial class ShowGuest1NotificationsView : Page
    {
        public ShowGuest1NotificationsView(string username, Page page, NavigationService navService)
        {
            InitializeComponent();

            DataContext = new ShowGuest1NotificationsViewModel(username, this, page, navService);
        }

    }
}