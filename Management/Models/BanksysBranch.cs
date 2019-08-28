using System;
using System.Collections.Generic;

namespace Management.Models
{
    public partial class BanksysBranch
    {
        public BanksysBranch()
        {
            BanksysUserBranchs = new HashSet<BanksysUserBranchs>();
            BanksysUsers = new HashSet<BanksysUsers>();
            BanksysBankActions = new HashSet<BanksysBankActions>();
        }

        public long BranchId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? BankId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public short? Status { get; set; }

        public BanksysBank Bank { get; set; }
        public ICollection<BanksysUserBranchs> BanksysUserBranchs { get; set; }
        public ICollection<BanksysUsers> BanksysUsers { get; set; }
        public ICollection<BanksysBankActions> BanksysBankActions { get; set; }

    }
}
