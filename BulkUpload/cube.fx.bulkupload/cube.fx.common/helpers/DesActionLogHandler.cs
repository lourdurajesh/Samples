using cube.fx.common.contract.interfaces;
using cube.fx.common.contract.model;
using cube.fx.common.contract.model.des;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.helper.Helpers
{
   public class DESActionLogHandler : IDesActionLogHandler
    {
        ICRUD _instance;
        public DESActionLogHandler(ICRUD iCRUD)
        {
            _instance = iCRUD;
        }
        public APIResponse Insert(DesActionLog desActionLog)
        {
            _instance.SetInstance("EA");
            APIResponse aPIResponse = new APIResponse();
            var dtoObj = CommonHelpers.UIToDTO<Des_Action_Log>(desActionLog);
            var uiObj = CommonHelpers.DTOToUI<DesActionLog>(dtoObj);
            var res= _instance.Insert<Des_Action_Log>(dtoObj);
           if(res==null)
                aPIResponse.Status = "Success";
           else
                aPIResponse.Status = "Failed";
            aPIResponse.Response = res;
           
            return aPIResponse;
        }
        public APIResponse Update(UpdateEntity updateEntity)
        {
            _instance.SetInstance("EA");
            APIResponse aPIResponse = new APIResponse();
            UpdateEntity uEntity=  _instance.Update<DesActionLog>(updateEntity);
            if(uEntity.Success)
            {
                aPIResponse.Status = "Success";
                aPIResponse.Response = updateEntity;
            }
            else
            {
                aPIResponse.Status = "Failed";
                aPIResponse.Response = null;
            }
            return aPIResponse;
        }


        private Des_Action_Log UIToDTO(DesActionLog desActionLog)
        {
            Des_Action_Log des_Action_Log = new Des_Action_Log();
            Dictionary<string, object> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(des_Action_Log));
            foreach(PropertyInfo propertyInfo in desActionLog.GetType().GetProperties())
            {
                keyValuePairs.Keys.ToList().Where(x=>(x.Split("_")[1]).Equals(propertyInfo.Name)).ToList().ForEach(x=> {
                    
                    keyValuePairs[x]=propertyInfo.GetValue(desActionLog, null);
                });
               
            }
            return des_Action_Log = JsonConvert.DeserializeObject<Des_Action_Log>(JsonConvert.SerializeObject(keyValuePairs));
        }
    }
}
