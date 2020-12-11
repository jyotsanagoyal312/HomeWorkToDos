using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace HomeWorkToDos.Util.Dtos
{
    [SwaggerSchemaFilter(typeof(RegisterUserDto))]
    public class RegisterUserDto
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
