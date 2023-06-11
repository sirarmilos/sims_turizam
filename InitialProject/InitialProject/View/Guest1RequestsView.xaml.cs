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

    public partial class Guest1RequestsView : Page
    {
        public Guest1RequestsView(string username, Page page, NavigationService navService)
        {
            InitializeComponent();

            DataContext = new Guest1RequestsViewModel(username, this, page, navService);
        }
    }
}
