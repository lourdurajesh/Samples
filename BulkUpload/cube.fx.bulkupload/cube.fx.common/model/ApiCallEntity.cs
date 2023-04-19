using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model
{
    public class ApiCallEntity
    {
        public string BaseAPIEndPoint { get; set; }
        public string ApiURL { get; set; }
        public dynamic InputJson { get; set; }
        public string AccessToken { get; set; }
        public string HttpMethod { get; set; }

    }
}
