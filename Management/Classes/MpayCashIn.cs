using Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Models
{
    public class MpayCashIn
    {
        public string operation { get; set; }
        public string sender { get; set; }
        public int lang { get; set; }


        public string senderType { get; set; }
     
        public string msgId { get; set; }
        public string tenant { get; set; }


        public string channelId { get; set; }
        public string requestedId { get; set; }
        public string shopId { get; set; }

        public extraData[] extraData { get; set; }




    }
}
