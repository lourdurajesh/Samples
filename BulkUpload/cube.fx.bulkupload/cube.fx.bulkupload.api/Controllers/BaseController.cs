using cube.fx.bulkupload.api.Interfaces.Services;
using cube.fx.common.contract.model;
using Microsoft.AspNetCore.Mvc;

namespace cube.fx.bulkupload.api.Controllers
{
    
    public abstract class BaseController<T> : ControllerBase where T : class
    {
        private readonly IService? service;
        public BaseController(IEnumerable<IService> services)
        {
            var serviceAttributes = GetType().GetCustomAttributes(typeof(ServiceNameAttribute), true);
                
            if(serviceAttributes != null)
            {
                var name = serviceAttributes.FirstOrDefault();
                if(name != null)
                {
                    this.service = services.SingleOrDefault(p => p.GetType().Name == ((ServiceNameAttribute)name).ServiceName);
                }
            }
            else
            {
                throw new Exception("Service Not Configured Properly");
            }
            if(this.service == null)
            {
                throw new Exception("Service Not Configured Properly");
            }
        }

        [RouteAttribute("Insert")]
        [HttpPost]
        public virtual IActionResult Insert([FromBody] T message)
        {
            APIResponse result = new APIResponse();
            if (message != null && service != null)
            {
                service.Input = message;
                result = service.Insert();

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [RouteAttribute("Update")]
        [HttpPost]
        public virtual IActionResult Update([FromBody] T message)
        {
            APIResponse result = new APIResponse();
            if (message != null && service != null)
            {
                service.Input = message;
                result = service.Update();

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
