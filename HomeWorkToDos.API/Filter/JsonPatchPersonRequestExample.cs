using Microsoft.AspNetCore.JsonPatch.Operations;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace HomeWorkToDos.API.Filter
{
    /// <summary>
    /// JsonPatchPersonRequestExample
    /// </summary>
    public class JsonPatchPersonRequestExample : IExamplesProvider<List<Operation>>
    {
        /// <summary>
        /// GetExamples
        /// </summary>
        /// <returns></returns>
        public List<Operation> GetExamples()
        {
            return new List<Operation>(){ new Operation
                {
                    op = "replace",
                    path = "/PropertyName",
                    value = "NewValue"
                }
            };
        }
    }
}
