using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model
{
    public class ErrorLog
    {
        public Guid ErrorId { get; set; }

        public string Application { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        public string Logged_User { get; set; }

        public int StatusCode { get; set; }  
        public string Exception { get; set; }
        public string AdditionalLog { get; set; }
        public string RequestFrom { get; set; }

    }
}
