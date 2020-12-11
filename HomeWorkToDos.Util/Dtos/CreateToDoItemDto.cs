using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HomeWorkToDos.Util.Dtos
{
    [SwaggerSchemaFilter(typeof(CreateToDoItemDto))]
    public class CreateToDoItemDto
    {
        public int? ToDoListId { get; set; }
        public int? LabelId { get; set; }
        [Required]
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public int CreatedBy { get; set; }
    }
}
