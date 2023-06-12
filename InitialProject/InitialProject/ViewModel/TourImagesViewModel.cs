using GalaSoft.MvvmLight;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.ViewModel
{
    public class TourImagesViewModel : ViewModelBase
    {
        public ObservableCollection<string> ImageSources { get; } = new ObservableCollection<string>
        {
            "/Resources/Images/tour1.jpg",
            "/Resources/Images/tour2.jpg",
            "/Resources/Images/tour3.jpg",
            "/Resources/Images/tour4.jpg",
            "/Resources/Images/tour5.jpg",
            "/Resources/Images/tour6.jpg"
        };

        public ICommand LogoutCommand { get; }
        public ICommand MostPopularTourCommand { get; }
        public ICommand PreviousWindowCommand { get; }

        public ICommand HomePageCommand { get; }
        public ICommand AllTourOccurencesCommand { get; }

        public TourImagesViewModel()
        {
            LogoutCommand = new RelayCommand(Logout);
            MostPopularTourCommand = new RelayCommand(GoToMostPopularTour);
            PreviousWindowCommand = new RelayCommand(GoToPreviousWindow);
            AllTourOccurencesCommand = new RelayCommand(GoToAllTourOccurences);
        }

        private void Logout()
        {
            LoginForm window = new LoginForm();
            window.Show();
            Application.Current.MainWindow?.Close();
        }

        private void GoToMostPopularTour()
        {
            ShowMostPopularTour window = new ShowMostPopularTour();
            window.Show();
            Application.Current.MainWindow?.Close();
        }

        private void GoToAllTourOccurences()
        {
            AllTourOccurences window = new AllTourOccurences();
            window.Show();
            Application.Current.MainWindow?.Close();
        }

        private void GoToPreviousWindow()
        {
            Application.Current.MainWindow?.Close();
        }
    }
}
