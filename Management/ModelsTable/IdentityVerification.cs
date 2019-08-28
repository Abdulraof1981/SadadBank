using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class IdentityVerification
    {
        public int Id { get; set; }
        public string Msisdn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreationOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Code { get; set; }
        public int? Status { get; set; }
    }
}
