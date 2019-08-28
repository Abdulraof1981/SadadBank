using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class CashIn
    {
        public CashIn()
        {
            BanksysBankActions = new HashSet<BanksysBankActions>();
        }

        public long CashInId { get; set; }
        public double? Valuedigits { get; set; }
        public string Valueletters { get; set; }
        public string Walletaccount { get; set; }
        public string ShipmentDuration { get; set; }
        public int? DepositType { get; set; }
        public int? CheckNum { get; set; }
        public string NumInvoiceDep { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public int? RefrenceNumber { get; set; }
        public int? PersonalId { get; set; }
        public int? BankId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public int? CardNumber { get; set; }
        public int? Refrence { get; set; }
        public DateTime? CreatedOnDealer { get; set; }
        public DateTime? LastModifiedOnDealer { get; set; }
        public int? CreatedByDealer { get; set; }
        public int? LastModifiedByDealer { get; set; }
        public string BanckAccountNumber { get; set; }
        public string AuthNumber { get; set; }

        public Bank Bank { get; set; }
        public PersonalInfo Personal { get; set; }
        public ICollection<BanksysBankActions> BanksysBankActions { get; set; }
    }
}
