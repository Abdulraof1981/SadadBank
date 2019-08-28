using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class Refugees
    {
        public Refugees()
        {
            RefugeeCashIn = new HashSet<RefugeeCashIn>();
        }

        public long RefugeeId { get; set; }
        public string Rid { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string GrandName { get; set; }
        public string SurName { get; set; }
        public string NameEn { get; set; }
        public string FatherNameEn { get; set; }
        public string GrandNameEn { get; set; }
        public string SurNameEn { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Gender { get; set; }
        public string Nationality { get; set; }
        public string Email { get; set; }
        public string Nid { get; set; }
        public string FamilyNumber { get; set; }
        public string PassportNumber { get; set; }
        public string PassportExportDate { get; set; }
        public string PersonalCardNumber { get; set; }
        public string PersonalCardExportDate { get; set; }
        public string Address { get; set; }
        public string NearByPlace { get; set; }
        public string Phone { get; set; }
        public string Bankaccountnumber { get; set; }
        public string Bankname { get; set; }
        public string Bankbranch { get; set; }
        public int? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public int? PersonalId { get; set; }
        public string Reason { get; set; }
        public long? Reference { get; set; }
        public int? OrganizationType { get; set; }

        public ICollection<RefugeeCashIn> RefugeeCashIn { get; set; }
    }
}
