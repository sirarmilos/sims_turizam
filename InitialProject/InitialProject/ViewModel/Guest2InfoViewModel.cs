using InitialProject.Dto;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.ViewModel
{
    public class Guest2InfoViewModel
    {
        private readonly Guest2Repository guest2Repository;

        private string username;

        public List<VoucherDisplayDTO> VoucherDisplayDTOs { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public Guest2InfoViewModel(string username)
        {
            this.username = username;
            guest2Repository = new Guest2Repository();

            VoucherDisplayDTOs = new List<VoucherDisplayDTO>();

            Guest2 guest2 = guest2Repository.GetByUsername(username);

            Username = guest2.User.Username.ToString();
            Email = guest2.Email.ToString();
            Address = guest2.Adress.ToString();

            List<VoucherDisplayDTO> voucherDisplayDTOs = new List<VoucherDisplayDTO>();

            List<Voucher> vouchers = guest2Repository.GetGuestsVouchers(username);

            foreach (Voucher voucher in vouchers)
            {
                VoucherDisplayDTO voucherDisplayDTO = new VoucherDisplayDTO(voucher.voucherType, voucher.expirationDate);
                VoucherDisplayDTOs.Add(voucherDisplayDTO);
            }

        }
    }
}
