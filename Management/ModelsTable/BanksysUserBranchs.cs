using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class BanksysUserBranchs
    {
        public long UserBranchId { get; set; }
        public long? BranchId { get; set; }
        public long? UserId { get; set; }
        public short? RegisterMaker { get; set; }
        public short? RegisterChecker { get; set; }
        public short? CashInMaker { get; set; }
        public short? CashInChecker { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public short? Status { get; set; }

        public BanksysBranch Branch { get; set; }
        public BanksysUsers User { get; set; }
    }
}
