using System;
using System.Collections.Generic;

namespace cube.fx.common.contract.model.des
{
    public class DesEventData
    {
        public Guid PK  { get; set; }
        public string AppCode { get; set; }
        public string ClassSource { get; set; }
        public string FieldName { get; set; }
        public string UIField { get; set; }
        public string Actions { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public Guid EntityRefKey { get; set; }
        public string EntitySource { get; set; }
        public string EntityRefCode { get; set; }
        public Guid? ParentEntityRefKey { get; set; }
        public string ParentEntitySource { get; set; }
        public string ParentEntityRefCode { get; set; }
        public Guid? AdditionalEntityRefKey { get; set; }
        public string AdditionalEntitySource { get; set; }
        public string AdditionalEntityRefCode { get; set; }
        public Guid DER_FK { get; set; }
        public DateTime EventDateTime { get; set; }
        public Guid DEM_FK { get; set; }
        public string EventName { get; set; }
        public string EventCode { get; set; }
        public int? DisplayOrder { get; set; }
        public string EventCSS { get; set; }
        public string TimeZone { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public Guid? CMN_SAP_FK { get; set; }
        public string TenantCode { get; set; }
        public DesEventReference DesEventReference { get; set; }
        public DesEventDataObject DesEventDataObject { get; set; }
    }
}
