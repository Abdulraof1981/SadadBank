using Management.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Models
{
    public partial class BanksysUsers
    {
        public BanksysUsers()
        {
            BanksysBankActions = new HashSet<BanksysBankActions>();
            BanksysUserBranchs = new HashSet<BanksysUserBranchs>();
        }

        public long UserId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public short UserType { get; set; }
        public long? BranchId { get; set; }
        public string Email { get; set; }
        public short Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public short LoginTryAttempts { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public byte[] Photo { get; set; }
        public short? RegisterMaker { get; set; }
        public short? RegisterChecker { get; set; }
        public short? CashInMaker { get; set; }
        public short? CashInChecker { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public short Status { get; set; }

        [NotMapped]
        public int[] Permissions { get; set; }
        [NotMapped]
        public SavedPermissions[] SavedPermissions { get; set; }
        public BanksysBranch Branch { get; set; }
        public ICollection<BanksysUserBranchs> BanksysUserBranchs { get; set; }

        public ICollection<BanksysBankActions> BanksysBankActions { get; set; }
    }
}
