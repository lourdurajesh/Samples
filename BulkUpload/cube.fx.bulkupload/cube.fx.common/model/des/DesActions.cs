using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model.des
{
    public class DesActions
    {
        public Guid PK { get; set; }
        public Guid DEM_FK { get; set; }
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
        public string ExpressionType { get; set; }
        public string Expression { get; set; }
        public string ActionType { get; set; }
        public string Action { get; set; }
        public int? ActionSequence { get; set; }
        public bool? WaitForAction { get; set; }

        public Dictionary<string, object> GetReferences()
        {
            Dictionary<string, object> result = new ();
            foreach(PropertyInfo pi in this.GetType().GetProperties())
            {
                if (pi.Name.Substring(0, 9).Equals("Reference", StringComparison.InvariantCultureIgnoreCase))
                {
                    var referenceValue = pi.GetValue(pi);
                    if (referenceValue != null)
                    {
                        result.Add(pi.Name, referenceValue);
                    }
                }
            }
            return result;
        }
    }
}
