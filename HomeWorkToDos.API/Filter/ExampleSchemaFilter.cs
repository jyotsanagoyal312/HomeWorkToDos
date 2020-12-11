using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace HomeWorkToDos.API.Filter
{
    /// <summary>
    /// Example Schema Filter
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter" />
    public class ExampleSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Applies the specified schema.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = GetExampleOrNullFor(context.Type);
        }

        /// <summary>
        /// Gets the example or null for.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private IOpenApiAny GetExampleOrNullFor(Type type)
        {
            return type.Name switch
            {
                "CreateLabelDto" => new OpenApiObject
                {
                    ["Description"] = new OpenApiString("Car"),
                    ["IsActive"] = new OpenApiBoolean(true)
                },

                "CreateToDoItemDto" => new OpenApiObject
                {
                    ["ToDoListId"] = new OpenApiLong(2),
                    ["Description"] = new OpenApiString("XUV feature"),
                    ["LabelId"] = new OpenApiInteger(2),
                    ["IsActive"] = new OpenApiBoolean(true)
                },
                "UpdateToDoItemDto" => new OpenApiObject
                {
                    ["ToDoItemId"] = new OpenApiLong(2),
                    ["ToDoListId"] = new OpenApiLong(2),
                    ["Description"] = new OpenApiString("Sedan feature"),
                    ["LabelId"] = new OpenApiInteger(2),
                    ["IsActive"] = new OpenApiBoolean(true)
                },

                "CreateToDoListDto" => new OpenApiObject
                {
                    ["Description"] = new OpenApiString("List of XUV"),
                    ["LabelId"] = new OpenApiInteger(1),
                    ["IsActive"] = new OpenApiBoolean(true)
                },
                "UpdateToDoListDto" => new OpenApiObject
                {
                    ["ToDoListId"] = new OpenApiLong(2),
                    ["Description"] = new OpenApiString("List of Sedan"),
                    ["LabelId"] = new OpenApiInteger(2),
                    ["IsActive"] = new OpenApiBoolean(true)
                },
                
                "RegisterUserDto" => new OpenApiObject
                {
                    ["FirstName"] = new OpenApiString("Jyotsana"),
                    ["LastName"] = new OpenApiString("Goyal"),
                    ["Email"] = new OpenApiString("Jyotsana@mail.com"),
                    ["Contact"] = new OpenApiString("2233445566"),
                    ["UserName"] = new OpenApiString("Jyotsana"),
                    ["Password"] = new OpenApiString("123"),
                },
                "LoginDto" => new OpenApiObject
                {
                    ["UserName"] = new OpenApiString("Jyotsana"),
                    ["Password"] = new OpenApiString("123"),
                },
                _ => null,
            };
        }
    }
}
