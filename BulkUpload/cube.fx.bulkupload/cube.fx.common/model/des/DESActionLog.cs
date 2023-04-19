using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model.des
{
    
    public class DesActionLog
    {       
        public Guid PK { get; set; }
        public Guid DED_FK { get; set; }
        public Guid DEA_FK { get; set; }
        public string DEA_ActionType { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string TenantCode { get; set; }
        public bool IsFailureAcknowledged { get; set; }
        public string FailureAcknowledgementComment { get; set; }
    }

    public class Des_Action_Log
    {
        public Guid DAL_PK { get; set; }
        public Guid DAL_DED_FK { get; set; }
        public Guid DAL_DEA_FK { get; set; }
        public string DAL_DEA_ActionType { get; set; }
        public string DAL_Status { get; set; }
        public string DAL_Remarks { get; set; }
        public DateTime? DAL_StartTime { get; set; }
        public DateTime? DAL_EndTime { get; set; }
        public DateTime? DAL_CreatedDateTime { get; set; }
        public DateTime? DAL_ModifiedDateTime { get; set; }
        public string DAL_CreatedBy { get; set; }
        public string DAL_ModifiedBy { get; set; }
        public string CMN_TenantCode { get; set; }
        public bool DAL_IsFailureAcknowledged { get; set; }
        public string DAL_FailureAcknowledgementComment { get; set; }
    }
}
