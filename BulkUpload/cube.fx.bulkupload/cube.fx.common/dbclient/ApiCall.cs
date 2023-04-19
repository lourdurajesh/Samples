using cube.fx.common.contract.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace cube.fx.common.dbclient
{
    public class ApiCall
    {
        public APIResponse GetAPI(ApiCallEntity apiCallEntity)
        {
            try
            {
                DataTable dt = new DataTable();
                APIResponse objResponse = new APIResponse();
                string Url = apiCallEntity.BaseAPIEndPoint + apiCallEntity.ApiURL;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", apiCallEntity.AccessToken);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("API call failed. Input : " + JsonConvert.SerializeObject(apiCallEntity)); //failed handle
                Stream s = response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                objResponse.Response = dataString;
                return objResponse;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public dynamic PostCallAPIWithResponse(ApiCallEntity apiCallEntity)
        {
            try
            {
                DataTable dtDSR = new DataTable();
                DataTable dt = new DataTable();
                HttpClient client = new HttpClient();
                APIResponse objResponse = new APIResponse();                
                HttpResponseMessage response = new HttpResponseMessage();
                using (client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiCallEntity.BaseAPIEndPoint);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    if(apiCallEntity.AccessToken!=null)
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", apiCallEntity.AccessToken);
                    client.Timeout = TimeSpan.FromMinutes(6);
                    response = client.PostAsync(apiCallEntity.BaseAPIEndPoint+ apiCallEntity.ApiURL,new StringContent(JsonConvert.SerializeObject(apiCallEntity.InputJson), Encoding.UTF8, "application/json")).Result;
                    objResponse.Response = response;
                    var responseStr = response.Content.ReadAsStringAsync().Result;
                    objResponse.Response = responseStr;
                    objResponse.Status = response.StatusCode.ToString();
                    if (response.IsSuccessStatusCode)
                    {
                        Uri gizmoUrl = response.Headers.Location;
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return objResponse;
                    }
                    else
                        throw new Exception("API call failed. Input : " + JsonConvert.SerializeObject(apiCallEntity)); //failed handle
                }
            }
            catch 
            {
                throw;
            }
        }
        public APIResponse ExternalAPICall(ApiCallEntity apiCallEntity)
        {
            APIResponse Response = new APIResponse();
            if (apiCallEntity.HttpMethod?.ToUpper() == "POST")
            {
                var result = PostCallAPIWithResponse(apiCallEntity);
                if (result?.Response != null)
                {
                    if (result?.GetType() == typeof(APIResponse) && result.GetType().GetProperty("Response")?.PropertyType == typeof(Object))
                    {
                        try
                        {
                            Response.Response = JsonConvert.DeserializeObject<APIResponse>(result.Response);
                            if (Response.Response.GetType() == typeof(APIResponse))
                            {
                                if (((APIResponse)Response.Response)?.Response == null)
                                    Response.Response = result.Response;
                            }
                        }
                        catch (Exception ex)
                        {
                            
                            throw new Exception("API response is not valid. Received Response : "+ JsonConvert.SerializeObject(result.Response)); //failed handle
                        }
                    }
                    else
                        throw new Exception("API response is not valid. Received Response : " + JsonConvert.SerializeObject(result.Response)); //failed handle
                }
                else 
                    throw new Exception("API call failed. Input : " + JsonConvert.SerializeObject(apiCallEntity)); //failed handle
            }
            else
            {
                var result = GetAPI(apiCallEntity);
                Response = result;
            }
            return Response;
        }
    }
}
