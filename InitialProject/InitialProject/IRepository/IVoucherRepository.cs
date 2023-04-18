using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IVoucherRepository
    {
        List<Voucher> FindAll();

        void Save(List<Voucher> vouchers);

        int NextId();
    }
}
