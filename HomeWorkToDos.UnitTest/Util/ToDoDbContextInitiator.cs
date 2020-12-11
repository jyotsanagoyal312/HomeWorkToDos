using HomeWorkToDos.DataAccess.Data;
using HomeWorkToDos.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace HomeWorkToDos.UnitTest.Util
{
    /// <summary>
    /// ToDoDbContextInitiator
    /// </summary>
    /// <seealso cref="HomeWorkToDos.UnitTest.Util.MapperInitiator" />
    public class ToDoDbContextInitiator : MapperInitiator
    {
        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        public HomeworktodosContext DBContext { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoDbContextInitiator"/> class.
        /// </summary>
        public ToDoDbContextInitiator()
        {
            DbContextOptionsBuilder<HomeworktodosContext> builder = new DbContextOptionsBuilder<HomeworktodosContext>()
                .UseInMemoryDatabase("ToDoDB").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            HomeworktodosContext _toDoDbContext = new HomeworktodosContext(builder.Options);
            DBContext = _toDoDbContext;

            SeedData(DBContext);
        }

        /// <summary>
        /// Seeds the data.
        /// </summary>
        /// <param name="DBContext">The database context.</param>
        public void SeedData(HomeworktodosContext DBContext)
        {
            DBContext.User.Add(new User
            {
                FirstName = "Jyotsana",
                LastName = "Goyal",
                UserName = "Jyotsana",
                Password = "123",
                Email = "test@mail.com",
                Contact = "1111111111",
                CreatedOn = DateTime.UtcNow,
            });

            Label label = new Label
            {
                Description = "something",
                UserId = 1,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = 1
            };
            DBContext.Label.Add(label);

            List<ToDoList> toDoLists = new List<ToDoList>()
            {
                new ToDoList
                {
                    Description = "something",
                    LabelId = 1,
                    IsActive = true,
                    UserId = 1,
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now
                },
                new ToDoList
                {
                    Description = "something test",
                    LabelId = 1,
                    IsActive = true,
                    UserId = 1,
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now
                }
            };
            DBContext.ToDoList.AddRange(toDoLists);

            List<ToDoItem> toDoItems = new List<ToDoItem>()
            {
                new ToDoItem
                {
                    Description = "something",
                    ToDoListId = 1,
                    LabelId = 1,
                    UserId = 1,
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now
                },
                new ToDoItem
                {
                    Description = "something test",
                    ToDoListId = 1,
                    LabelId = 1,
                    UserId = 1,
                    IsActive = true,
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now
                }
            };
            DBContext.ToDoItem.AddRange(toDoItems);

            DBContext.SaveChanges();

            // detach entities for in memoryDB
            DBContext.Entry(label).State = EntityState.Detached;

            foreach (var item in toDoLists)
            {
                DBContext.Entry(item).State = EntityState.Detached;
            }

            foreach (var item in toDoItems)
            {
                DBContext.Entry(item).State = EntityState.Detached;
            }
        }
    }
}
