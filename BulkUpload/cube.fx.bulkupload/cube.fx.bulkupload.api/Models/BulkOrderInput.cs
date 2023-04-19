using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cube.fx.bulkupload.api.models
{
    public class BulkOrderInput
    {
        public string Filename { get; set; }
    }

    public class FileType
    {
        public string Filetype { get; set; }
    }

    public class OrgDetail
    {
        public string OrgCode { get; set; }
    }
    public class UserPass
    {
        public string username {get; set;}

        public string Password { get; set; }
    }
}
