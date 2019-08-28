using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class RefugeeCashIn
    {
        public long RefugeeCashInId { get; set; }
        public long? RefugeeId { get; set; }
        public double? Valuedigits { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public long? Reference { get; set; }
        public int? OrganizationType { get; set; }

        public Refugees Refugee { get; set; }
    }
}
