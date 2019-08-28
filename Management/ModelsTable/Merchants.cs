using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class Merchants
    {
        public int MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string MerchantAdress { get; set; }
        public string PhoneNumber { get; set; }
        public int? Status { get; set; }
        public int? CityId { get; set; }
        public int? CategoryId { get; set; }
        public string Logo { get; set; }
        public string FacebookLink { get; set; }
        public string WorkHours { get; set; }
        public string SadadWalletId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public int? LastModifiedBy { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int? StreetId { get; set; }
        public string CloseReason { get; set; }

        public Merchants Merchant { get; set; }
        public Merchants InverseMerchant { get; set; }
    }
}
