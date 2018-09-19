using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace MicroService.Controllers
{
    [Route("api/administration/[controller]")]
    public class AccountController : Controller
    {

        public AccountController()
        {

        }

        [HttpPost("login")]
        public IActionResult login([FromBody]ViewModel.Administration.Login login)
        {


            var oResult = new ViewModel._System.Result<dynamic>();


            var oLoginTableAdapter = new DataAccess.administration.UserTableAdapters.LoginTableAdapter();

            var oLoginDataTable = oLoginTableAdapter.GetData(login.mobileNo, login.password);



            if (oLoginDataTable.Count == 0)
            {
                oResult.isDone = false;
                oResult.errorCode = Unauthorized().StatusCode;
                oResult.serverMessage = "The mobile no. or password is invalid";

            }
            else
            {


                if (oLoginDataTable[0].isActive == false)
                {
                    oResult.isDone = false;
                    oResult.errorCode = Unauthorized().StatusCode;
                    oResult.serverMessage = "The account is disabled";
                }




                var oPayloadViewModel = new ViewModel.Administration.Payload
                {
                    userId = oLoginDataTable[0].id,
                    expireDate = DateTime.Now.AddMinutes(Utility.Setting.JWT_TIMEOUT_MINUTE).Ticks,
                };

                var strPayload = JsonConvert.SerializeObject(oPayloadViewModel);

                var strEncryptedPayLoad = Utility.Security.Encrypt(strPayload);

                var oJasonWebToken = new ViewModel.Administration.JasonWebToken();

                oJasonWebToken.payLoad = strEncryptedPayLoad;

                oResult.datum = oJasonWebToken;


            }






            return (new JsonResult(oResult));


        }

        [HttpGet("getUserProfile")]
        public IActionResult getUserProfile()
        {

            int intCurrentUserId = Utility.Setting.currentUserID;

            var oGetUserProfileTableAdapter = new DataAccess.administration.UserTableAdapters.GetUserProfileTableAdapter();

            var oGetUserProfileTable = oGetUserProfileTableAdapter.GetData(intCurrentUserId);

            var oResult = new ViewModel._System.Result<dynamic>();


            if (oGetUserProfileTable.Count == 0)
            {
                oResult.isDone = false;
                oResult.errorCode = NotFound().StatusCode;
            }

            else
            {

                var oDatum = oGetUserProfileTable[0].toDynamic();
                oResult.datum = oDatum;


            }

            return (new JsonResult(oResult));
        }



    }

}





