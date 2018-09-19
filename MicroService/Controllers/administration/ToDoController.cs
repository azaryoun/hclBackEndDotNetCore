using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace MicroService.Controllers
{
    [Route("api/administration/[controller]")]
    public class ToDoController : Controller
    {

        public ToDoController()
        { }

        #region "Common Methods and Action Methods"


        [HttpGet("getManagementEntities")]
        public IActionResult getManagementEntities()
        {


            var oResult = new ViewModel._System.Result<List<ViewModel.Administration.ToDoManagement>>();


            var oGetManagementToDosTableAdapter = new DataAccess.administration.ToDoTableAdapters.GetManagementToDosTableAdapter();
            var oGetManagementToDosTable = oGetManagementToDosTableAdapter.GetData();
            var oToDoManagements = new List<ViewModel.Administration.ToDoManagement>();

            foreach (var oGetManagementToDos in oGetManagementToDosTable)
            {
                var oToDoManagement = new ViewModel.Administration.ToDoManagement();

                oToDoManagement.assigneeTitle = oGetManagementToDos.assigneeTitle;
                oToDoManagement.dueDateTitle = oGetManagementToDos.dueDate.ToString("yyyy/MM/dd");
                oToDoManagement.id = oGetManagementToDos.id;
                oToDoManagement.priority = oGetManagementToDos.priority;
                oToDoManagement.statusTitle = oGetManagementToDos.toDoStatusTitle;
                oToDoManagement.task = oGetManagementToDos.task;

                oToDoManagements.Add(oToDoManagement);
            }

            oResult.datum = oToDoManagements;



            return (new JsonResult(oResult));

        }


        [HttpGet("getEntity/{id}")]
        public IActionResult getEntity(int id)
        {


            var oResult = new ViewModel._System.Result<ViewModel.Administration.ToDo>();


            var oGetEntityTableAdapter = new DataAccess.administration.ToDoTableAdapters.GetToDoTableAdapter();
            var oGetEntityTable = oGetEntityTableAdapter.GetData(id);

            var oEntity = new ViewModel.Administration.ToDo();



            if (oGetEntityTable.Count > 0)
            {
                oEntity.assigneeId = oGetEntityTable[0].fk_assigneeId;
                oEntity.dueDate = oGetEntityTable[0].dueDate;
                oEntity.notes = oGetEntityTable[0].notes;
                oEntity.priority = oGetEntityTable[0].priority;
                oEntity.statusId = oGetEntityTable[0].fk_assigneeId;
                oEntity.task = oGetEntityTable[0].task;

                oResult.datum = oEntity;

            }
            else
            {
                oResult.isDone = false;
                oResult.errorCode = NotFound().StatusCode;
            }

            return (new JsonResult(oResult));

        }

        [HttpPost("insertEntity")]
        public IActionResult insertEntity([FromBody]ViewModel.Administration.ToDo entity)
        {


            var oResult = new ViewModel._System.Result<dynamic>();


            try
            {

                var oQueriesTableAdapter = new DataAccess.administration.ToDoTableAdapters.QueriesTableAdapter();
                oQueriesTableAdapter.InsertToDo(entity.task, entity.statusId, entity.priority, entity.dueDate, entity.assigneeId, entity.notes);


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

        [HttpPut("updateEntity/{id}")]
        public IActionResult updateEntity(int id, [FromBody]ViewModel.Administration.ToDo entity)
        {

            var oResult = new ViewModel._System.Result<dynamic>();


            try
            {

                var oQueriesTableAdapter = new DataAccess.administration.ToDoTableAdapters.QueriesTableAdapter();
                oQueriesTableAdapter.UpdateToDo(id, entity.task, entity.statusId, entity.priority, entity.dueDate, entity.assigneeId, entity.notes);





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

        [HttpDelete("deleteEntity/{id}")]
        public IActionResult deleteEntity(int id)
        {
            var oResult = new ViewModel._System.Result<dynamic>();

            try
            {

                var oQueriesTableAdapter = new DataAccess.administration.ToDoTableAdapters.QueriesTableAdapter();
                oQueriesTableAdapter.DeleteToDo(id);



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







