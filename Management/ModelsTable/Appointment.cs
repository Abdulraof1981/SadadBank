using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class Appointment
    {
        public long TransactionId { get; set; }
        public string Nid { get; set; }
        public string Msisdn { get; set; }
        public int? ServiceCenterId { get; set; }
        public int? BankId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public byte? TransactionStatus { get; set; }
        public byte? TransactionType { get; set; }
        public long? ReferenceCode { get; set; }
    }
}
