using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.Business.Service;
using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.DataAccess.Data;
using HomeWorkToDos.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HomeWorkToDos.API.DependencyInjection
{
    /// <summary>
    /// DI Configuration
    /// </summary>
    public static class DIConfiguration
    {
        /// <summary>
        /// Registers the DI.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterDI(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<HomeworktodosContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUser, UserService>();
            services.AddScoped<IUserRepo, UserRepository>();
            services.AddScoped<IToDoList, ToDoListService>();
            services.AddScoped<IToDoListRepo, ToDoListRepository>();
            services.AddScoped<IToDoItem, ToDoItemService>();
            services.AddScoped<IToDoItemRepo, ToDoItemRepository>();
            services.AddScoped<ILabel, LabelService>();
            services.AddScoped<ILabelRepo, LabelRepository>();
            return services;
        }
    }
}
