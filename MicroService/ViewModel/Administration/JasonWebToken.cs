using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MicroService.ViewModel.Administration
{
    [JsonObject(MemberSerialization.OptOut)]
    public class JasonWebToken
    {
        public string payLoad { get; set; }

        public JasonWebToken()
        {
            this.payLoad = null;
        }
    }


    public class Payload
    {
        public int userId { get; set; }
        public long expireDate { get; set; }
    
    }

}