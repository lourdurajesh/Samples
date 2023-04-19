using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model.des
{
   public class DesBaseInput
    {      
        public DesEventData DesEventData { get; set; }
        public DesActions DesActions { get; set; }
        public DesActionLog DesActionLog { get; set; }
    }
}
