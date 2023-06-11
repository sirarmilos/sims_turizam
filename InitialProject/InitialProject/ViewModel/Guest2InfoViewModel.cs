using GalaSoft.MvvmLight.Command;
using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.ViewModel
{
    public class Guest2InfoViewModel
    {
        private readonly Guest2Repository guest2Repository;

        private Guest2Service guest2Service = new Guest2Service();

        private string username;

        public List<VoucherDisplayDTO> VoucherDisplayDTOs { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public ICommand LogOutCommand { get; private set; }

        public ICommand PageLoadCommand { get; private set; }

        private readonly Page _page;

        private void LogOut()
        {
            LoginForm loginForm = new LoginForm();
            Window parentWindow = Window.GetWindow(_page);
            parentWindow.Close();
            loginForm.Show();
        }

        private void MainWindow_ButtonClicked(object sender, EventArgs e)
        {
            Guest2ProfilePreview guest2ProfilePreview = new Guest2ProfilePreview(Username);
            _page.NavigationService.Navigate(guest2ProfilePreview);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            var mainWindow = Window.GetWindow(_page) as Guest2MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ButtonClicked += MainWindow_ButtonClicked;
            }
        }

        private void ExecutePageLoadCommand()
        {
            Page_Loaded(null, null);
        }


        public Guest2InfoViewModel(Page page)
        {
            _page = page;

            this.username = UserClass.Username;
            guest2Repository = new Guest2Repository();

            VoucherDisplayDTOs = new List<VoucherDisplayDTO>();

            LogOutCommand = new RelayCommand(LogOut);
            PageLoadCommand = new RelayCommand(ExecutePageLoadCommand);

            Guest2 guest2 = guest2Service.GetByUsername(username);

            Username = guest2.User.Username.ToString();
            Email = guest2.Email.ToString();
            Address = guest2.Adress.ToString();

            List<VoucherDisplayDTO> voucherDisplayDTOs = new List<VoucherDisplayDTO>();

            List<Voucher> vouchers = guest2Service.GetGuestsVouchers(username);

            foreach (Voucher voucher in vouchers)
            {
                VoucherDisplayDTO voucherDisplayDTO = new VoucherDisplayDTO(voucher.voucherType, voucher.expirationDate);
                VoucherDisplayDTOs.Add(voucherDisplayDTO);
            }

        }
    }
}
