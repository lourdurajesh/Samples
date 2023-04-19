using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.helper.Helpers
{
    public static class CommonHelpers
    {
        public static T UIToDTO<T>(object uiObj) where T : class
        {
            T objTable = (T)Activator.CreateInstance(typeof(T), new object[] { }); ;

            Dictionary<string, object> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(objTable));
            foreach (PropertyInfo propertyInfo in uiObj.GetType().GetProperties())
            {
                keyValuePairs.Keys.ToList().Where(x => (x.Substring(x.IndexOf('_')+1, (x.Length) - (x.IndexOf('_') + 1))).Equals(propertyInfo.Name)).ToList().ForEach(x =>
                {

                    keyValuePairs[x] = propertyInfo.GetValue(uiObj, null);
                });

            }
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(keyValuePairs));
        }

        public static T DTOToUI<T>(object dtoObj) where T : class
        {
            T objTable = (T)Activator.CreateInstance(typeof(T), new object[] { }); ;

            Dictionary<string, object> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(objTable));
            foreach (PropertyInfo propertyInfo in dtoObj.GetType().GetProperties())
            {
                string name = propertyInfo.Name.Substring(propertyInfo.Name.IndexOf('_')+1, propertyInfo.Name.Length - (propertyInfo.Name.IndexOf('_') + 1));
                keyValuePairs.Keys.ToList().Where(x => x.Equals(name)).ToList().ForEach(x =>
                {

                    keyValuePairs[x] = propertyInfo.GetValue(dtoObj, null);
                });

            }
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(keyValuePairs));
        }

        public static List<T> DTOToUIList<T>(List<object> dtoObjLst) where T : class
        {
            T objTable = (T)Activator.CreateInstance(typeof(T), new object[] { }); ;

            List<Dictionary<string, object>> keyValuePairsLst = new List<Dictionary<string, object>>();           

            dtoObjLst.ForEach(dtoObj => {
                Dictionary<string, object> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(objTable));
                foreach (PropertyInfo propertyInfo in dtoObj.GetType().GetProperties())
                {
                    string name = propertyInfo.Name.Substring(propertyInfo.Name.IndexOf('_') + 1, propertyInfo.Name.Length - (propertyInfo.Name.IndexOf('_') + 1));
                    keyValuePairs.Keys.ToList().Where(x => x.Equals(name)).ToList().ForEach(x =>
                    {

                        keyValuePairs[x] = propertyInfo.GetValue(dtoObj, null);
                    });

                }
                keyValuePairsLst.Add(keyValuePairs);
            });

            
            return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(keyValuePairsLst));
        }
    }
}
