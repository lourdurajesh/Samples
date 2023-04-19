using cube.fx.common.contract.interfaces;
using cube.fx.common.contract.model;
using cube.fx.common.contract.model.des;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace cube.fx.common.helper
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ICRUD _instance;
        private string _IpAddress;
        public ExceptionHandler(RequestDelegate next, ICRUD iCRUD)
        {
            _instance = iCRUD;
            _next = next;
        }

        public ExceptionHandler(ICRUD iCRUD)
        {
            _instance = iCRUD;            
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                _IpAddress = context.Connection.RemoteIpAddress.ToString();
                await _next(context);
            }
            catch (Exception error)
            {
                var res = context.Response;
                var response = new APIResponse();
                res.ContentType = "application/json";
                res.StatusCode = error switch
                {
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,// not found error
                    _ => (int)HttpStatusCode.InternalServerError,// unhandled error
                };
                response.Status = "Failed";
                response.Exception = error?.GetBaseException();
                response.Error = error?.Message;
                if (error != null)
                {
                    LogException(error, context);
                }
                var errorResponse = new
                {
                    message = error?.GetBaseException()?.Message,
                    statusCode = res.StatusCode
                };
                var errorJson = JsonConvert.SerializeObject(errorResponse);
                await res.WriteAsync(errorJson);
            }
        }

        private void LogException(Exception error, HttpContext context)
        {
            try
            {
                _instance.SetInstance("LO");
                string AdditionalLog = string.Empty;
                if (context.Request.Body.CanRead)
                {
                    JObject keyValuePairs = new JObject();
                    DesBaseInput desBaseInput = new DesBaseInput();

                    using var reader = new StreamReader(context.Request.Body);

                    // You shouldn't need this line anymore.
                    // reader.BaseStream.Seek(0, SeekOrigin.Begin);

                    // You now have the body string raw
                    var body =  reader.ReadToEndAsync().Result;

                    try
                    {
                        // As well as a bound model
                        //desBaseInput = JsonConvert.DeserializeObject<DesBaseInput>(body);
                        //keyValuePairs.Add("DED_PK", desBaseInput.DesEventData.PK);
                        //keyValuePairs.Add("DAL_PK", desBaseInput.DesActionLog.PK);
                        //keyValuePairs.Add("EntityRefKey", desBaseInput.DesEventData.EntityRefKey);
                        //keyValuePairs.Add("EntityRefCode", desBaseInput.DesEventData.EntityRefCode);
                    }
                    catch
                    {
                        keyValuePairs.Add("InputObj", body);
                    }
                    AdditionalLog = JsonConvert.SerializeObject(keyValuePairs);
                }

                ErrorLog errorLog = new ErrorLog
                {
                    ErrorId = Guid.NewGuid(),
                    Application = Config.Settings.CommonSettings.Application,
                    Type = error.GetType().ToString(),
                    Exception = error.ToString(),
                    Message = error.Message,
                    StatusCode = context.Response.StatusCode,
                    Logged_User = Config.Settings.CommonSettings.UserName,
                    RequestFrom = _IpAddress,
                    AdditionalLog = AdditionalLog
                };
                _instance.Insert<ErrorLog>(errorLog);
                //Save the error data to Elmah_Error in Logging table or MongoDB
            }
            catch
            {
            }
        }

        public void LogException(Exception error, DesBaseInput desBaseInput)
        {
            try
            {
                _instance.SetInstance("LO");
                string AdditionalLog = string.Empty;
                JObject keyValuePairs = new JObject();
                keyValuePairs.Add("DED_PK", desBaseInput.DesEventData.PK);
                keyValuePairs.Add("DAL_PK", desBaseInput.DesActionLog.PK);
                keyValuePairs.Add("EntityRefKey", desBaseInput.DesEventData.EntityRefKey);
                keyValuePairs.Add("EntityRefCode", desBaseInput.DesEventData.EntityRefCode);                    
                AdditionalLog = JsonConvert.SerializeObject(keyValuePairs);

                ErrorLog errorLog = new ErrorLog
                {
                    ErrorId = Guid.NewGuid(),
                    Application = Config.Settings.CommonSettings.Application,
                    Type = error.GetType().ToString(),
                    Exception = error.ToString(),
                    Message = error.Message,
                    StatusCode = 500,
                    Logged_User = Config.Settings.CommonSettings.UserName,
                    RequestFrom = _IpAddress,
                    AdditionalLog = AdditionalLog
                };
                _instance.Insert<ErrorLog>(errorLog);
                //Save the error data to Elmah_Error in Logging table or MongoDB
            }
            catch
            {
            }
        }
    }
}
