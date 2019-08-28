using System;
using System.Collections.Generic;

namespace Management.Models
{
    public partial class BanksysBankActions
    {
        public long BankActionId { get; set; }
        public int? ActionType { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        public long? CashInId { get; set; }
        public int? PersonalInfoId { get; set; }
        public long? BranchId { get; set; }
        
        public int UserType { get; set; }

        public DateTime ActionDate { get; set; }

        public CashIn CashIn { get; set; }
        public PersonalInfo PersonalInfo { get; set; }
        public BanksysUsers User { get; set; }
        public BanksysBranch Branch { get; set; }
    }
}
