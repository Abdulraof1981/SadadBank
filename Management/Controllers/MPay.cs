using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
    public class MPay
    {
        public string operation { get; set; }
        public string sender { get; set; }
        public int lang { get; set; }

        public string channelId { get; set; }
        public string requestedId { get; set; }
        public string shopId { get; set; }



        public string senderType { get; set; }
        public string deviceId { get; set; }
        public string msgId { get; set; }
        public string tenant { get; set; }
        public extraData[] extraData { get; set; }
    }

    public class responseMpay
    {
        public int responseCode { get; set; }
        public responseData responsData { get; set; }
    }
    public class responseData
    {
        public response response { get; set; }
        public string token { get; set; }
    }
    public class response
    {
        public string errorCd { get; set; }
        public string desc { get; set; }
        public int reF { get; set; }

        public string statusCode { get; set; }

    }

    public class extraData
    {
        public string key { get; set; }
        public string value { get; set; }

    }
}
