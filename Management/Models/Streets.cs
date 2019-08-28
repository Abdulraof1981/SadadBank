using System;
using System.Collections.Generic;

namespace Management.Models
{
    public partial class Streets
    {
        public int StreetId { get; set; }
        public string StreetName { get; set; }
        public int? Status { get; set; }
        public int? CityId { get; set; }

        public City City { get; set; }
    }
}
