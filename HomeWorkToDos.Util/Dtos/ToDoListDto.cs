using System;
using System.Collections.Generic;

namespace HomeWorkToDos.Util.Dtos
{
    public class ToDoListDto
    {
        public int ToDoListId { get; set; }
        public int? LabelId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }

        public LabelDto Label { get; set; }
        public UserDto User { get; set; }
        public ICollection<ToDoItemDto> ToDoItem { get; set; }
    }
}
