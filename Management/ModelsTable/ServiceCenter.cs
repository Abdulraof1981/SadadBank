using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class ServiceCenter
    {
        public int ServiceCenterId { get; set; }
        public string ServiceCenterName { get; set; }
        public TimeSpan? StartOfWorkTime { get; set; }
        public TimeSpan? EndOfWorkTime { get; set; }
        public int? PeriodOfOneCaseInMinutes { get; set; }
    }
}
