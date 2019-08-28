using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class ClintInfo
    {
        public int Id { get; set; }
        public string NameOrganization { get; set; }
        public string Activity { get; set; }
        public string OrganType { get; set; }
        public string AdressOrgan { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Licensor { get; set; }
        public string RegistrationPlace { get; set; }
        public DateTime? EstablishmentDate { get; set; }
        public string NumberCommerceRoom { get; set; }
        public string NumberInCommerceRecord { get; set; }
        public string LegalForm { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string GrandName { get; set; }
        public string SurName { get; set; }
        public string NameEn { get; set; }
        public string FatherNameEn { get; set; }
        public string GrandNameEn { get; set; }
        public string SurNameEn { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string EmailCommissioner { get; set; }
        public string Nid { get; set; }
        public string FamilyNumber { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportExportDate { get; set; }
        public string PersonalCardNumber { get; set; }
        public DateTime? PersonalCardExportDate { get; set; }
        public string Address { get; set; }
        public string NearByPlace { get; set; }
        public string PhoneCommissioner { get; set; }
        public string Bankaccountnumber { get; set; }
        public string Bankname { get; set; }
        public string Bankbranch { get; set; }
        public string Wallettype { get; set; }
        public string ActivityCommissioner { get; set; }
        public string Valuedigits { get; set; }
        public string Valueletters { get; set; }
        public string Walletaccount { get; set; }
        public string ShipmentDuration { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public string AppointmentDate { get; set; }
        public string ReferenceCode { get; set; }
        public string AppointmentTime { get; set; }
        public string ServiceCenterName { get; set; }
        public int? DepositType { get; set; }
        public int? CheckNum { get; set; }
        public string NumInvoiceDep { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
    }
}
