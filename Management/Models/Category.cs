using System;
using System.Collections.Generic;

namespace Management.Models
{
    public partial class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Icon { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }
        public string SmallIcon { get; set; }
    }
}
