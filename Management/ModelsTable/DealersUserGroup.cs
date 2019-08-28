using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class DealersUserGroup
    {
        public long DealersUserGroupId { get; set; }
        public long? DealersId { get; set; }
        public long? UesrGroupId { get; set; }
        public int? Status { get; set; }
    }
}
