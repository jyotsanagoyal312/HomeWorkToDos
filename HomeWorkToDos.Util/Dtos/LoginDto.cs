using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace HomeWorkToDos.Util.Dtos
{
    [SwaggerSchemaFilter(typeof(LoginDto))]
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
