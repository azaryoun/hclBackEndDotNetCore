using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MicroService.ViewModel._System;

namespace MicroService.Controllers
{
    [Route("api/lookUp/[controller]")]
    public class AssigneeController : Controller
    {

        public AssigneeController()
        {

        }

     

        #region "Common Methods and Action Methods"


    
 
        [HttpGet("getLookUps")]
        public IActionResult getLookUps()
        {

            var oResult = new ViewModel._System.Result<dynamic>();

            try
            {
                var oGetEntitiesTableAdapter = new DataAccess.lookUp.AssigneeTableAdapters.GetAssigneesTableAdapter();
                var oGetEntitiesTable = oGetEntitiesTableAdapter.GetData();

                oResult.datum = oGetEntitiesTable.toDynamic();

            }
            catch (Exception ex)
            {
                oResult.isDone = false;
                oResult.hasException = true;
                oResult.serverMessage = ex.Message;
                oResult.errorCode = BadRequest().StatusCode;
            }

            return (new JsonResult(oResult));


        }




        #endregion






    }



}







