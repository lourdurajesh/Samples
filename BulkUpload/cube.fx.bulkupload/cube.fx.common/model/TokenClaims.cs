using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model
{
    public class TokenClaims
    {
        public string ApplicationId { get; set; }
        public string SessionId { get; set; }
        public string RemoteIP { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserDisplayName { get; set; }
        public string TenantCode { get; set; }
        public string BaseTenantCode { get; set; }
        public string UserId { get; set; }
        public string TokenType { get; set; }
        public string UserType { get; set; }
        public string SAP_FK { get; set; }
        public string IsFirst { get; set; }
        public string IsSysAdmin { get; set; }
        public string ROLCode { get; set; }
        public string ROLPK { get; set; }
        public string DefaultPartyPK { get; set; }
        public string DefaultPartyCode { get; set; }
        public string DefaultRolePK { get; set; }
        public string DefaultRoleCode { get; set; }
        public string DefaultAccessCode { get; set; }
        public string DefaultMenu { get; set; }
        public string Version { get; set; }
        public string UserDefinedTimezone { get; set; }
        public string DefaultTimeZone { get; set; }
        public string AdminUser { get; set; }
    }
}
