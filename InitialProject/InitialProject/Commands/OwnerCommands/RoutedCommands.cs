﻿using System;
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

        public static readonly RoutedUICommand ReadCancelledReservationNotification = new RoutedUICommand
        (
            "Read cancelled reservation to notification",
            "ReadCancelledReservationNotification",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.A, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand OwnerHomePageNotLogin = new RoutedUICommand
        (
            "Home",
            "OwnerHomePageNotLogin",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F1),
                new KeyGesture(Key.H, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand OwnerLogin = new RoutedUICommand
        (
            "Login",
            "OwnerLogin",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F8),
                new KeyGesture(Key.L, ModifierKeys.Alt)
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

        public static readonly RoutedUICommand ConfirmRejection = new RoutedUICommand // isto kao i komanda DeclineRequest, mozda da koristim kao jednu komandu
        (
            "Confirm rejectiont",
            "ConfirmRejection",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.J, ModifierKeys.Control) // pazi na ove sa istim komandama D
            }
        );

        public static readonly RoutedUICommand Cancel = new RoutedUICommand
        (
            "Confirm rejectiont",
            "ConfirmRejection",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Escape)
            }
        );
    }
}