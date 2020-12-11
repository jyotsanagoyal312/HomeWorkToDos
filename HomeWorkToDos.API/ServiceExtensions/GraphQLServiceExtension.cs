using HomeWorkToDos.API.GraphQL;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace HomeWorkToDos.API.ServiceExtensions
{
    /// <summary>
    /// Extension method for configure IService Collection for adding GraphQl services.
    /// </summary>
    public static class GraphQLServiceExtension
    {
        /// <summary>
        /// Adds the graph ql services.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public static IServiceCollection AddGraphQLServices(this IServiceCollection service)
        {
            return service.AddGraphQL(s => SchemaBuilder.New()
                    .AddServices(s)
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>()
                    .Create());
        }
    }
}
