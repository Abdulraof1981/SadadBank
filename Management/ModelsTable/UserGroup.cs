using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class UserGroup
    {
        public long UserGroupId { get; set; }
        public string Name { get; set; }
        public string Descrption { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
