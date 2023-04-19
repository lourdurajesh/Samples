using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model.des
{
    public class DesEventReference
    {
        public Guid PK { get; set; }
        public Guid DED_FK { get; set; }
        public string ReferenceValue1 { get; set; }
        public string ReferenceValue2 { get; set; }
        public string ReferenceValue3 { get; set; }
        public string ReferenceValue4 { get; set; }
        public string ReferenceValue5 { get; set; }
        public string ReferenceValue6 { get; set; }
        public bool? ReferenceFlag1 { get; set; }
        public bool? ReferenceFlag2 { get; set; }
        public DateTime? ReferenceDate1 { get; set; }
        public DateTime? ReferenceDate2 { get; set; }
    }
}
