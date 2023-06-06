using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public User user { get ; set; }
        public VoucherType voucherType { get; set; }
        public DateTime expirationDate { get; set; }

        public bool IsUsed { get; set; }

        public string Guide { get; set; }
       
        public Voucher()
        { }

        public Voucher(int id, User user, VoucherType voucherType, DateTime expirationDate, bool isUsed, string guide)
        {
            Id = id;
            this.user = user;
            this.voucherType = voucherType;
            this.expirationDate = expirationDate;
            IsUsed = isUsed;
            Guide = guide;
        }   

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), user.Username.ToString(), voucherType.ToString(), expirationDate.ToString(), IsUsed.ToString(), Guide};
            return csvValues;
            
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            user = new User() { Username = values[1] };
            voucherType = Enum.Parse<VoucherType>(values[2]);
            expirationDate = Convert.ToDateTime(values[3]);
            IsUsed = Convert.ToBoolean(values[4]);
            Guide = values[5];
        }
    }
}
