using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.contract.model
{
    public static class Config
    {
        public static Settings Settings { get; set; }
        public static List<Context> Contexts { get; set; } 
    }
    
    public class BaseSettings
    {
        public BaseAppSettings BaseAppSettings { get; set; }
        public BaseAppSettings BaseConnectionStrings { get; set; }   
        public BaseAppSettings BaseCommonSettings { get; set; }
    }
   
    public class BaseAppSettings
    {       
        public string emailId { get; set; }
    }
    public class BaseCommonSettings
    {      
        public string EaxisApiURL { get; set; }
        public string AuthApiURL { get; set; }
        public string ReportApiURL { get; set; }
        public string Application { get; set; }
        public string AccessToken { get; set; }        
        public TokenClaims TokenClaim { get; set; }
        public string SkipAuth { get; set; }
        public string UserName { get; set; }
        public string UserEmailId { get; set; }
        public string TenantCode { get; set; }
        public Guid? SAP_FK { get; set; }
    }
    public class BaseConnectionStrings
    {
        public string EA { get; set; }
        public string TC { get; set; }
        public string LO { get; set; }       
    }
    public class BaseContext
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class Settings : BaseSettings
    {
        public AppSettings AppSettings { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public CommonSettings CommonSettings { get; set; }
    }
    public class AppSettings : BaseAppSettings
    {

    }
    public class ConnectionStrings : BaseConnectionStrings
    {

    }
    public class CommonSettings : BaseCommonSettings
    {

    }
    public class Context : BaseContext
    {

    }
}
