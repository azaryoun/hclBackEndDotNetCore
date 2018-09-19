using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MicroService.ViewModel._System
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Result<dataType>
    {

        public bool isDone { get; set; }
        public bool hasException { get; set; }
        public int? errorCode { get; set; }
        public string serverMessage { get; set; }
        public int? id { get; set; }
        public string value { get; set; }
        public dataType datum { get; set; } //data is plural. datum is singular
        public Result()
        {
            this.isDone = true;
            this.hasException = false;
            this.errorCode = null;
            this.serverMessage = null;
            this.id = -1;
            this.value = null;
          //  this.datum = null;
        }
    }


}