using cube.fx.bulkupload.api.Interfaces.Services;
using cube.fx.common.contract.model;

namespace cube.fx.bulkupload.api.Services
{
    public abstract class BaseService : IService
    {
        public object Input { get; set; }

        public abstract APIResponse Invoke();

        private APIResponse BuildApiResponse()
        {
            return new APIResponse();
        }

        public virtual APIResponse Insert()
        {
            APIResponse response = BuildApiResponse();
            response.Status = "Success";
            response.Messages = new List<Message>() { new Message() { MessageDesc = "Success", Type = "Status" } };
            return response;
        }

        public virtual APIResponse Update()
        {
            APIResponse response = BuildApiResponse();
            response.Status = "Success";
            response.Messages = new List<Message>() { new Message() { MessageDesc = "Success", Type = "Status" } };
            return response;
        }

        public virtual APIResponse GetById(Guid pk)
        {
            throw new NotImplementedException();
        }

        public virtual APIResponse FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
