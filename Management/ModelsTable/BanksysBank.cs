using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class BanksysBank
    {
        public BanksysBank()
        {
            BanksysBranch = new HashSet<BanksysBranch>();
        }

        public long BankId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public short? Status { get; set; }

        public ICollection<BanksysBranch> BanksysBranch { get; set; }
    }
}
