using System;
using System.Collections.Generic;

namespace Management.Models
{
    public partial class Bank
    {
        public Bank()
        {
            CashIn = new HashSet<CashIn>();
        }

        public int BankId { get; set; }
        public string BankName { get; set; }
        public int? Status { get; set; }

        public ICollection<CashIn> CashIn { get; set; }
    }
}
