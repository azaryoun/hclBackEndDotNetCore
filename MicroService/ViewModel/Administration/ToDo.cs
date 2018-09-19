using System;


namespace MicroService.ViewModel.Administration
{

    public class ToDo
    {
        public string task { get; set; }
        public int statusId { get; set; }
        public string priority { get; set; }
        public DateTime dueDate { get; set; }
        public int assigneeId { get; set; }
        public string notes { get; set; }

    }

    public class ToDoManagement
    {
        public int id { get; set; }
        public string task { get; set; }
        public string statusTitle { get; set; }
        public string priority { get; set; }
        public string dueDateTitle { get; set; }
        public string assigneeTitle { get; set; }

    }


}
