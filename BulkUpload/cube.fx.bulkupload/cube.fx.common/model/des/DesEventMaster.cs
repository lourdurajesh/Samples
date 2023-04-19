using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model.des
{
   public class Des_Event_Master
    {
         public Guid DEM_PK { get; set; }
        public string DEM_Code { get; set; }
        public string DEM_Description { get; set; }
        public string DEM_Details { get; set; }
        public string DEM_Module { get; set; }
        public string DEM_Type { get; set; }
        public int DEM_Sequence { get; set; }
        public string DEM_OtherConfig { get; set; }
        public string DEM_CreatedBy { get; set; }
        public DateTime? DEM_CreatedDateTime { get; set; }
        public string DEM_ModifiedBy { get; set; }
        public DateTime? DEM_ModifiedDateTime { get; set; }
        public string CMN_TenantCode { get; set; }
        public string DEM_CSS { get; set; }
        public Guid? DEM_PartyType_FK { get; set; }
        public string DEM_PartyType_Code { get; set; }
        public bool DEM_IsSystemEvent { get; set; }
    }
}
