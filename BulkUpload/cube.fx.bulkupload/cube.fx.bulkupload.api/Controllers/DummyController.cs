using cube.fx.bulkupload.api.Interfaces.Services;
using cube.fx.bulkupload.api.Models;
using Microsoft.AspNetCore.Mvc;


namespace cube.fx.bulkupload.api.Controllers
{
    [RouteAttribute("Dummy")]
    [ServiceName("DummyService")]
    public partial class DummyController : BaseController<DummyInput>
    {
        public DummyController(IEnumerable<IService> services) : base(services)
        {
        }
    }
}
