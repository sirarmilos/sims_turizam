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
                new KeyGesture(Key.H, ModifierKeys.Control) // ovako radi sa control, jedino sa alt pravi problem
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

        public static readonly RoutedUICommand OwnerHomePageNotLogin = new RoutedUICommand
        (
            "Logout",
            "OwnerHomePageNotLogin",
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
    }
}
