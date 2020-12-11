using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HomeWorkToDos.Util.Dtos
{
    [SwaggerSchemaFilter(typeof(CreateLabelDto))]
    public class CreateLabelDto
    {
        [Required]
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public int CreatedBy { get; set; }
    }
}
