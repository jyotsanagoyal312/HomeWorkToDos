using System;

namespace HomeWorkToDos.Util.Dtos
{
    public class LabelDto
    {
        public int LabelId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }

        public UserDto User { get; set; }
    }
}
