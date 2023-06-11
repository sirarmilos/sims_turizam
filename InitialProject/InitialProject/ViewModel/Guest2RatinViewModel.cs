using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Command;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.View;
using Microsoft.Win32;

public class Guest2RateTourAndGuideViewModel : INotifyPropertyChanged
{
    private readonly Guest2Service guest2Service;
    private readonly string username;
    private readonly string guideUsername;
    private readonly int tourGuidenceId;

    private List<string> pictures;

    private string generalKnowledgeVal;
    private string languageKnowledgeVal;
    private string tourExperienceVal;

    public string TourName { get; set; }
    public string GuideName { get; set; }

    public string GeneralKnowledgeVal
    {
        get { return generalKnowledgeVal; }
        set
        {
            if (generalKnowledgeVal != value)
            {
                generalKnowledgeVal = value;
                OnPropertyChanged();
            }
        }
    }

    public string LanguageKnowledgeVal
    {
        get { return languageKnowledgeVal; }
        set
        {
            if (languageKnowledgeVal != value)
            {
                languageKnowledgeVal = value;
                OnPropertyChanged();
            }
        }
    }

    public string TourExperienceVal
    {
        get { return tourExperienceVal; }
        set
        {
            if (tourExperienceVal != value)
            {
                tourExperienceVal = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand AddPictureCommand { get; private set; }
    public ICommand RateGuideCommand { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;

    private readonly Page _page;

    public Guest2RateTourAndGuideViewModel(Page page)
    {
        _page = page;
    }

    public Guest2RateTourAndGuideViewModel(string username, TourGuidence tourGuidence)
    {
        this.username = username;
        guest2Service = new Guest2Service();

        TourName = tourGuidence.Tour.TourName;
        GuideName = tourGuidence.Tour.GuideUsername;

        guideUsername = tourGuidence.Tour.GuideUsername;
        tourGuidenceId = tourGuidence.Id;

        pictures = new List<string>();

        AddPictureCommand = new RelayCommand(AddPicture);
        RateGuideCommand = new RelayCommand(RateGuide);
    }

    private void AddPicture()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
        bool? result = openFileDialog.ShowDialog();

        if (result == true)
        {
            string selectedFilePath = openFileDialog.FileName;
            pictures.Add(selectedFilePath);
        }
    }

    private void RateGuide()
    {
        guest2Service.GuideRating(username, guideUsername, tourGuidenceId, Convert.ToInt32(GeneralKnowledgeVal), Convert.ToInt32(LanguageKnowledgeVal), Convert.ToInt32(TourExperienceVal), "", pictures);
        Guest2SuccesRating guest2SuccesRating = new Guest2SuccesRating(username);
        _page.NavigationService.Navigate(guest2SuccesRating);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
