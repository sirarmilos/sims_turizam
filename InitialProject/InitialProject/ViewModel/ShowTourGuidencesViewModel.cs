using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Prism.Commands;
using System.Windows.Input;
using InitialProject.DTO;
using GalaSoft.MvvmLight.Command;

namespace InitialProject.ViewModel
{
    public class ShowTourGuidencesViewModel
    {
        public ObservableCollection<TourGuidence> tourGuidences { get; set; }

        private readonly TourGuidenceService tourGuidenceService;

        public TourGuidence selectedGuidence { get; set; }

        public ICommand DetailsTourGuidenceCommand { get; set; } 

        public ShowTourGuidencesViewModel()
        {
            tourGuidenceService = new TourGuidenceService();
            tourGuidences = new ObservableCollection<TourGuidence>(tourGuidenceService.GetAllForToday());
            DetailsTourGuidenceCommand = new RelayCommand<TourGuidence>(DetailsTourGuidence);
        }

        public void DetailsTourGuidence(TourGuidence tourGuidence)
        {
            ShowKeyPointsInStartedTourGuidence window = new ShowKeyPointsInStartedTourGuidence(tourGuidence, tourGuidences.ToList());
            window.Show();
        }
    }
}
