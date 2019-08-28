using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class Applications
    {
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public int? Ttl { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? TokenLength { get; set; }
        public int? State { get; set; }
    }
}
