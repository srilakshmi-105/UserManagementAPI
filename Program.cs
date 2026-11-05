using UserManagementAPI.Middleware;
using UserManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Register one shared in-memory store for this demo API.
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

// Exception middleware should be first so it can catch errors from later middleware.
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapGet("/", () => Results.Ok(new
{
    message = "User Management API is running",
    endpoints = new[] { "GET /api/users", "GET /api/users/{id}", "POST /api/users", "PUT /api/users/{id}", "DELETE /api/users/{id}" }
}));

var users = app.MapGroup("/api/users");

users.MapGet("/", (UserService service) =>
    Results.Ok(service.GetAll()));

users.MapGet("/{id:int}", (int id, UserService service) =>
{
    var user = service.GetById(id);
    return user is null ? Results.NotFound(new { message = "User not found." }) : Results.Ok(user);
}).WithName("GetUserById");

users.MapPost("/", (CreateUserRequest request, UserService service) =>
{
    var errors = UserValidation.Validate(request);
    if (errors.Count > 0)
        return Results.ValidationProblem(errors);

    if (service.EmailExists(request.Email))
        return Results.Conflict(new { message = "A user with this email already exists." });

    var user = service.Create(request);
    return Results.CreatedAtRoute("GetUserById", new { id = user.Id }, user);
});

users.MapPut("/{id:int}", (int id, UpdateUserRequest request, UserService service) =>
{
    var errors = UserValidation.Validate(request);
    if (errors.Count > 0)
        return Results.ValidationProblem(errors);

    if (service.EmailExists(request.Email, id))
        return Results.Conflict(new { message = "Another user already uses this email." });

    var updatedUser = service.Update(id, request);
    return updatedUser is null
        ? Results.NotFound(new { message = "User not found." })
        : Results.Ok(updatedUser);
});

users.MapDelete("/{id:int}", (int id, UserService service) =>
    service.Delete(id)
        ? Results.Ok(new { message = "User deleted successfully." })
        : Results.NotFound(new { message = "User not found." }));

app.Run();
