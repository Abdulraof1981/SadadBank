using System;
using System.Collections.Generic;

namespace Management.Models
{
    public partial class UserGroupDetails
    {
        public long UserGroupDetailsId { get; set; }
        public int? Home { get; set; }
        public int? PersonalRegister { get; set; }
        public int? MerchantRegister { get; set; }
        public int? FirstConfirmPersonal { get; set; }
        public int? LastConfirmPersonal { get; set; }
        public int? FirstConfirmMerchant { get; set; }
        public int? LastConfirmMerchant { get; set; }
        public int? MerchantReadyToWork { get; set; }
        public int? CashInFirstConfirm { get; set; }
        public int? CashInLastConfirm { get; set; }
        public int? PermissionSettings { get; set; }
        public int? AssignPermissions { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public int? LastModifiedBy { get; set; }
        public long? UserGroupId { get; set; }
        public int? Status { get; set; }
    }
}
