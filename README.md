# HomeWorkToDos: ToDo App web API (.Net Core)

A rest api project to do CRUD operations for labels, todoitems and lists via HTTP Verbs (GET, POST, PUT, DELETE, PATCH).
It includes functionality for assigning labels to items and lists.
It also includes authorization via JWT Token.
It also logs each and every request/response or error if any.


DB Setup:

1. Database is configured using Entity Framework's Db first approach. User needs to create database using the script "HomeWorkToDos\DataScript\HomeWorkToDosScript.sql". Now Only the connection string in appsettings.{Env}.json needs to be changed accordingly.
2. The Db contains one user entry.


Techologies:

.Net Core 3.1, EF Core, GraphQL (including playground), Swagger (documentation)


Pre Requisite:

Microsoft dot net core 3.1 sdk package/ dot net core runtime 3.1 version should be installed on machine.


