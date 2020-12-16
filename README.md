# HomeWorkToDos: ToDo App web API (.Net Core)

A rest api project to do CRUD operations for labels, todoitems and lists via HTTP Verbs (GET, POST, PUT, DELETE, PATCH).
It includes functionality for assigning labels to items and lists.
It also includes authorization via JWT Token.
It also logs each and every request/response or error if any.

#### Notes
- A user has to create label first to assign it to todo list or todo item.
- After that create todo list in order to add todo item.
- Label can be assigned to todo list or todo item at the time of creation of list or item or while updating.
- Duplicate Usernames are not allowed.
- Base64 password encoding algorithm is used for storing password in database.


### Technologies:

.Net Core 3.1, EF Core, GraphQL (including playground), Swagger (documentation)


### Pre Requisite:

Microsoft dot net core 3.1 sdk package/ dot net core runtime 3.1 version should be installed on machine.

### Logging
Logs are stored in HomeWorkToDos-{date}.txt file in the Logs folder in the executing directory.

All the request, response and error details are logged in this file.


### Database Setup

Database is configured using Entity Framework's Db first approach. User needs to create database using the script "HomeWorkToDos\DataScript\HomeWorkToDosScript.sql". 

Update the connection string in appsettings.{Env}.json accordingly.

    "ConnectionStrings": 
		{
			"Default": <update-connectionString>
		}

The database contains one user already registered; username=jyotsana, password=123


### Running the Project
Go to the solution folder in `cmd` and run `dotnet restore`.

Go to the HomeWorkToDos.API folder in `cmd` and run `dotnet run`.

The APIs are accessible only by an authenticated user and required an authentication token generated by validating user login.

##### Swagger
Go to `http://localhost:5000` in browser to access Swagger documentation.

Add authorization token in swagger by using user login call `/api/v1/todo/User/Login` with the sample request body with the username and password.
This call will result in Jwt token in response.

Use this token to as Authentication token in the `Authorize` section of the swagger UI and enter the token in `Bearer <copied token>` format.

##### GraphQL
Go to `http://localhost:5000/playground` in browser to access GraphQL playground.
Use the below mutation to get the JWT.

    mutation {
    	AuthenticateUser(userName:"jyotsana",password: "123")
    }
Copy the token and add an Authorization header in the Headers section for all the requests.

    {
    	"Authorization": <copied token>
    }
