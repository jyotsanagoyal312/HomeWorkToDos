using AutoMapper;
using HomeWorkToDos.DataAccess.Models;
using HomeWorkToDos.Util.Dtos;

namespace HomeWorkToDos.API.Mapper
{
    /// <summary>
    /// Mapping Profile
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<User, RegisterUserDto>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Label, LabelDto>();
            CreateMap<LabelDto, Label>();
            CreateMap<Label, CreateLabelDto>();
            CreateMap<CreateLabelDto, Label>();

            CreateMap<ToDoItem, ToDoItemDto>();
            CreateMap<ToDoItemDto, ToDoItem>();
            CreateMap<CreateToDoItemDto, ToDoItem>();
            CreateMap<ToDoItem, CreateToDoItemDto>();
            CreateMap<UpdateToDoItemDto, ToDoItem>();
            CreateMap<ToDoItem, UpdateToDoItemDto>();
            CreateMap<UpdateToDoItemDto, ToDoItemDto>();
            CreateMap<ToDoItemDto, UpdateToDoItemDto>();

            CreateMap<ToDoList, ToDoListDto>();
            CreateMap<ToDoListDto, ToDoList>();
            CreateMap<CreateToDoListDto, ToDoList>();
            CreateMap<ToDoList, CreateToDoListDto>();
            CreateMap<UpdateToDoListDto, ToDoList>();
            CreateMap<ToDoList, UpdateToDoListDto>();
            CreateMap<UpdateToDoListDto, ToDoListDto>();
            CreateMap<ToDoListDto, UpdateToDoListDto>();
        }
    }
}
