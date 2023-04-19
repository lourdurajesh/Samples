using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model
{
    class CommonResponse
    {
    }
    public class APIResponse
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public object Response { get; set; }  /// This should be Generic Object
        public string Error { get; set; }
        public List<Message> Messages { get; set; }
        public Exception Exception { get; set; }
        public int Count { get; set; }
        public List<SValidation> Validations { get; set; }
    }
    public class Message
    {
        public string Type { get; set; }
        public string MessageDesc { get; set; }
    }
    public class SValidation
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public bool IsAlert { get; set; }
        public string MetaObject { get; set; }
        public string ParentRef { get; set; }
        public string GParentRef { get; set; }
        public string DisplayName { get; set; }
        public string CtrlKey { get; set; }
        public int RowIndex { get; set; }
        public int ColIndex { get; set; }
    }
}
