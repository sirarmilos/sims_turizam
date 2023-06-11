using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.Commands.OwnerCommands
{
    public static class RoutedCommands
    {
        public static readonly RoutedUICommand OwnerHomePageLogin = new RoutedUICommand
        (
            "Home",
            "OwnerHomePageLogin",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F1),
                new KeyGesture(Key.H, ModifierKeys.Alt),
            }
        );

        public static readonly RoutedUICommand AccommodationStart = new RoutedUICommand
        (
            "Accommodation",
            "AccommodationStart",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F2),
                new KeyGesture(Key.A, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand ShowOwnerManageBookingMoveRequests = new RoutedUICommand
        (
            "Management of booking move requests",
            "ShowOwnerManageBookingMoveRequests",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F3),
                new KeyGesture(Key.M, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand ShowAndCancellationRenovation = new RoutedUICommand
        (
            "Renovate",
            "ShowAndCancellationRenovation",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F4),
                new KeyGesture(Key.V, ModifierKeys.Alt) // R ne moze, jer alt + r vec nesto radi
            }
        );

        public static readonly RoutedUICommand RateGuests = new RoutedUICommand
        (
            "Rate guests",
            "RateGuests",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.T, ModifierKeys.Control),
                new KeyGesture(Key.F5),
                new KeyGesture(Key.T, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand ShowGuestReviews = new RoutedUICommand
        (
            "Guest reviews",
            "ShowGuestReviews",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F6),
                new KeyGesture(Key.G, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand OwnerForum = new RoutedUICommand
        (
            "Forum",
            "OwnerForum",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F, ModifierKeys.Control),
                new KeyGesture(Key.F7),
                new KeyGesture(Key.F, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand Logout = new RoutedUICommand
        (
            "Logout",
            "Logout",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F8),
                new KeyGesture(Key.L, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand Notifications = new RoutedUICommand
        (
            "Notifications",
            "Notifications",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F9),
                new KeyGesture(Key.N, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand Login = new RoutedUICommand
        (
            "Login",
            "Login",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.L, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand AddNewAccommodation = new RoutedUICommand
        (
            "Add new accommodation",
            "AddNewAccommodation",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.M, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand RenovateAccommodation = new RoutedUICommand
        (
            "Renovate accommodation",
            "RenovateAccommodation",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand AccommodationStatistics = new RoutedUICommand
        (
            "Accommodation statistics",
            "AccommodationStatistics",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.G, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand AcceptRequest = new RoutedUICommand
        (
            "Accept request",
            "AcceptRequest",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand DeclineRequest = new RoutedUICommand
        (
            "Decline request",
            "DeclineRequest",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand ConfirmRejection = new RoutedUICommand
        (
            "Confirm rejectiont",
            "ConfirmRejection",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D, ModifierKeys.Control) // J
            }
        );

        public static readonly RoutedUICommand Cancel = new RoutedUICommand
        (
            "Cancel",
            "Cancel",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Escape)
            }
        );

        public static readonly RoutedUICommand CancelRenovation = new RoutedUICommand
        (
            "Cancel renovation",
            "CancelRenovation",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Q, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand FindAvailableDates = new RoutedUICommand
        (
            "Find available dates",
            "FindAvailableDates",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Rate = new RoutedUICommand
        (
            "Rate guest",
            "Rate",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand PDFReport = new RoutedUICommand
        (
            "Report on average Guest rate for each accommodation",
            "PDFReport",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.O, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand PDF = new RoutedUICommand
        (
            "PDF",
            "PDF",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.P, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand BackFromPDFReport = new RoutedUICommand
        (
            "Back",
            "BackFromPDFReport",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.B, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand SaveAccommodation = new RoutedUICommand
        (
            "Save accommodation",
            "SaveAccommodation",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.M, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand DEMO = new RoutedUICommand
        (
            "DEMO",
            "DEMO",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand AddImage = new RoutedUICommand
        (
            "Add image",
            "AddImage",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.I, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand RemoveImage = new RoutedUICommand
        (
            "Remove image",
            "RemoveImage",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand ChooseImage = new RoutedUICommand
        (
            "Choose image",
            "ChooseImage",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.P, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand AddTopLocationAccommodation = new RoutedUICommand
        (
            "Add top location accommodation",
            "AddTopLocationAccommodation",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.K, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand RemoveWorstLocationAccommodation = new RoutedUICommand
        (
            "Remove worst location accommodation",
            "RemoveWorstLocationAccommodation",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand ReadMore = new RoutedUICommand
        (
            "Read more",
            "ReadMore",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Close = new RoutedUICommand
        (
            "Close",
            "Close",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Escape)
            }
        );

        public static readonly RoutedUICommand AddComment = new RoutedUICommand
        (
            "Add comment",
            "AddComment",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand CloseDEMO = new RoutedUICommand
        (
            "Close DEMO",
            "CloseDEMO",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E, ModifierKeys.Control)
            }
        );
    }
}
