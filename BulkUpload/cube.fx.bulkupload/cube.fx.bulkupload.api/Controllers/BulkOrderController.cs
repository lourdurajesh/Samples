using cube.fx.bulkupload.api.Interfaces.Services;
using cube.fx.bulkupload.api.models;

using Microsoft.AspNetCore.Mvc;

namespace cube.fx.bulkupload.api.Controllers
{
    [RouteAttribute("BulkUpload")]
    [ServiceName("BulkProcessingService")]
    public class BulkOrderController : BaseController<BulkOrderInput>
    {
        public BulkOrderController(IEnumerable<IService> services) : base(services)
        {
        }

        //[RouteAttribute("Insert")]
        //[HttpPost]
        //public override IActionResult Insert([FromBody] BulkOrderInput message)
        //{

        //    return Ok("Calling from Child");
        //}
    }
}
