using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2Info.xaml
    /// </summary>
    public partial class Guest2Info : Window
    {
        private readonly Guest2Repository guest2Repository;

        private string username;
        public Guest2Info(string username)
        {
            InitializeComponent();
            this.username = username;
            guest2Repository = new Guest2Repository();

            Guest2 guest2 = guest2Repository.GetByUsername(username);

            usernameBox.Text = guest2.User.Username.ToString();
            emailBox.Text = guest2.Email.ToString();
            adressBox.Text = guest2.Adress.ToString();

            List<VoucherDisplayDTO> voucherDisplayDTOs = new List<VoucherDisplayDTO>();

            List<Voucher> vouchers = guest2Repository.GetGuestsVouchers(username);

            foreach(Voucher voucher in vouchers) 
            {
                VoucherDisplayDTO voucherDisplayDTO = new VoucherDisplayDTO(voucher.voucherType,voucher.expirationDate);
                voucherDisplayDTOs.Add(voucherDisplayDTO);
            }

            VouchersList.ItemsSource = voucherDisplayDTOs;
        }
    }

    public class VoucherDisplayDTO
    {
        public string voucherType { get; set; }
        public string expirationDate { get; set; }

        public VoucherDisplayDTO()
        { }

        public VoucherDisplayDTO(VoucherType voucherType, DateTime expirationDate)
        {
            this.voucherType = voucherType.ToString();
            this.expirationDate = expirationDate.ToString();

        }
    }
}
