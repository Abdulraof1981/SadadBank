using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class City
    {
        public City()
        {
            Streets = new HashSet<Streets>();
        }

        public int CityId { get; set; }
        public string CityName { get; set; }
        public int? Status { get; set; }
        public int? CityMpayId { get; set; }

        public City CityNavigation { get; set; }
        public City InverseCityNavigation { get; set; }
        public ICollection<Streets> Streets { get; set; }
    }
}
