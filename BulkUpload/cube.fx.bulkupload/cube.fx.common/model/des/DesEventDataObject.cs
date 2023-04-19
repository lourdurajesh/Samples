using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model.des
{
   public class DesEventDataObject
    {        
            public Guid PK { get; set; }
            public Guid DED_FK { get; set; }
            public string Event_JSON { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedDateTime { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDateTime { get; set; }
            public string TenantCode { get; set; }        
    }
}
