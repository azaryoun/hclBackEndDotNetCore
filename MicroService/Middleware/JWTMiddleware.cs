using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace MicroService.Middleware
{

    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {


            if (httpContext.Request.Path.HasValue == false)
                return _next(httpContext); //skip fav icon


            if (httpContext.Request.Path.Value.StartsWith("/api/", StringComparison.OrdinalIgnoreCase) == false)
                return _next(httpContext);  //skip fav icon


            // public action methods (no authentication required)
            ActionsStruct[] skipingActions = new ActionsStruct[]
                {
                    new ActionsStruct { Path = "/api/administration/account/login", HttpMethod = "POST" },
                };


            //removing special character from body
            string bodyAsText = new StreamReader(httpContext.Request.Body).ReadToEnd();
            var injectedRequestStream = new MemoryStream();
            var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText.Replace("'", "").Replace("--", ""));
            injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
            injectedRequestStream.Seek(0, SeekOrigin.Begin);
            httpContext.Request.Body = injectedRequestStream;




            for (int i = 0; i < skipingActions.Length; i++)
            {
                if (httpContext.Request.Path.Value.StartsWith(skipingActions[i].Path, StringComparison.OrdinalIgnoreCase)
              && httpContext.Request.Method.Equals(skipingActions[i].HttpMethod, StringComparison.OrdinalIgnoreCase))
                    return _next(httpContext);
            }



            try
            {
                var strEncyptedPayLoad = httpContext.Request.Headers["authorization"].ToString().Substring(7);

                var strPayload = Utility.Security.Decrypt(strEncyptedPayLoad);

                var oPayloadViewModel = JsonConvert.DeserializeObject<ViewModel.Administration.Payload>(strPayload);

                var dteExpireDate = new DateTime(oPayloadViewModel.expireDate);

                if (dteExpireDate <= DateTime.Now)
                    throw new Exception("The token is already expired.");

                oPayloadViewModel.expireDate = DateTime.Now.AddMinutes(Utility.Setting.JWT_TIMEOUT_MINUTE).Ticks;

                strPayload = JsonConvert.SerializeObject(oPayloadViewModel);

                strEncyptedPayLoad = Utility.Security.Encrypt(strPayload);

                httpContext.Response.Headers.Remove("authorization");

                httpContext.Response.Headers.Add("authorization", strEncyptedPayLoad); //updating expire date

                Utility.Setting.currentUserID = oPayloadViewModel.userId; //Set UserID in a public place;

                return _next(httpContext);


            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = 401; //Unauhorized
                httpContext.Response.WriteAsync("Unauhorized:" + ex.Message);
                return _next(null);
            }

        }


        private struct ActionsStruct
        {
            public string Path;
            public string HttpMethod;
        }

    }


    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JWTMiddlewareExtensions
    {
        public static IApplicationBuilder UseJWTMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JWTMiddleware>();
        }
    }

}