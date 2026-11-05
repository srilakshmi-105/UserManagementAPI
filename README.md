# User Management API

A simple ASP.NET Core 8 Web API demonstrating CRUD operations, dependency injection,
input validation, request logging, and centralized exception handling.

## Requirements
- .NET 8 SDK
- Visual Studio, Visual Studio Code, or another C# editor

## Run the project
1. Open a terminal in this folder.
2. Run `dotnet restore`.
3. Run `dotnet run`.
4. Open the URL printed in the terminal. The root route (`/`) lists the available endpoints.

The sample uses an in-memory store. Data resets when the application restarts; this keeps
the assignment easy to run without a database setup.

## API endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/users` | Get all users |
| GET | `/api/users/{id}` | Get one user |
| POST | `/api/users` | Create a user |
| PUT | `/api/users/{id}` | Update a user |
| DELETE | `/api/users/{id}` | Delete a user |

### Example POST/PUT JSON
```json
{
  "name": "Jordan Lee",
  "email": "jordan@example.com",
  "age": 20
}
```

Validation rules: name is required and must be 2–60 characters; email must be valid;
age must be between 1 and 120. Duplicate email addresses are rejected.

## Design decisions
- `UserService` is registered as a **singleton** because this demo keeps one shared in-memory
  collection for the lifetime of the application. `ConcurrentDictionary` supports concurrent
  access. In a database-backed application, a scoped service/DbContext would usually be more
  appropriate.
- Middleware is ordered with exception handling first, so it can catch failures from later
  middleware and endpoint execution. Request logging records method, path, status, and duration.
- JSON is used because it is readable and widely supported by web clients.

## Copilot and debugging evidence — complete this honestly
Use Microsoft Copilot while debugging your own project. For example, ask it to review the
validation logic, middleware order, or endpoint behavior. Test its suggestions, keep only
changes you understand, and record:
- The prompt you used:
- The issue Copilot identified:
- The change you accepted or rejected:
- How you tested the result:

Do not claim Copilot was used unless you actually used it.

## GitHub submission
Create a **public** GitHub repository named `UserManagementAPI`, upload the project files,
commit them, and submit the repository URL to Coursera.
