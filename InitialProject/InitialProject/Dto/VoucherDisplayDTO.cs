using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Dto
{
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
