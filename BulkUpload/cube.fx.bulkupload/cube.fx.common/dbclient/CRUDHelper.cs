using cube.fx.common.contract.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.dbclient
{
   public class CRUDHelper
    {
        public string connectionString;
        public CRUDHelper(string db)
        {
            connectionString = GetDBConnection(db);
        }
        private string GetDBConnection(string db)
        {
            string _connString = string.Empty;
            switch (db)
            {
                case "EA":
                    _connString = Config.Settings.ConnectionStrings.EA;
                    break;
                case "TC":
                    _connString = Config.Settings.ConnectionStrings.TC;
                    break;
                case "LO":
                    _connString = Config.Settings.ConnectionStrings.LO;
                    break;                
                default:
                    _connString = null;
                    break;
            }
            return _connString;

        }
    }
}
