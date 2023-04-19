using cube.fx.bulkupload.api.models;
using cube.fx.common.contract.model;

namespace cube.fx.bulkupload.api.Services
{
    public class BulkProcessingService : BaseService
    {
        public override APIResponse Invoke()
        {
            APIResponse result = new APIResponse();
            try
            {
                //if (Input != null)
                //{
                //    if (Duplicate(Constants.TMS.Consignment, Input.Filename))
                //    {
                //        return BuildApiResponse();
                //    }

                //    Global.Claims = BasicDetail.GetBasicDetails();
                //    Global.Token = Request.Headers.Authorization.ToString();
                //    var APService = new OracleAPBL();
                //    result = APService.PostAP(message.Filename);

                //    utility.RemoveFromCache(Constants.TMS.Consignment, Input.Filename);

                return result;
                //}
                //else
                //{
                //    return BadRequest();
                //}
            }
            catch (Exception ex)
            {
                //utility.RemoveFromCache(Constants.TMS.Consignment, message.Filename);

                //result.Status = "Failed";
                //result.Error = ex.Message;
                //result.Exception = ex;
                return result;
            }
        }

        public override APIResponse Insert()
        {
            var res = new APIResponse();
            res.Status = "Sucess";
            var msg = new Message();
            msg.MessageDesc = "Calling from Child";
            res.Messages = new List<Message>() { msg };
            return res;
        }

        private bool Duplicate(string messageName, string fileName)
        {
            return false;
        }
    }
}
