using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.ViewModel;

namespace InitialProject.View
{
    public partial class ShowReservations : Window
    {
        public ShowReservations(string username)
        {
            InitializeComponent();
            this.DataContext = new ShowReservationsViewModel(username);
        }
    }

}
