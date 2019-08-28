using System;
using System.Collections.Generic;

namespace Management.Models
{
    public partial class Tokens
    {
        public string SecretCode { get; set; }
        public string TokenCode { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int? ApplicationId { get; set; }
        public bool? BlackListed { get; set; }
    }
}
