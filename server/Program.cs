using NanoidDotNet;

const string alphabet = "abcdefghijkmnpqrtwxyz346789ABCDEFGHJKLMNPQRTUVWXY_";

const string unixSocketPath = "/tmp/uds-dotnet-bun.sock";

if (File.Exists(unixSocketPath))
    File.Delete(unixSocketPath);

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.ListenUnixSocket(unixSocketPath));

var todos = new List<Todo>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/todos", () => todos);

app.MapGet("/todos/{id:alpha}", (string id) =>
{
    var todo = todos.Find(todo => todo.Id == id);
    if (todo is null) return Results.NotFound();
    return Results.Ok(todo);
});

app.MapPost("/todos", (TodoDTO dto) =>
{
    var (Id, Title, Done) = dto;
    Title = Title.Trim();
    if (Title == string.Empty)
        return Results.BadRequest("Title cannot be empty.");
    if (Id is not null)
    {
        var existing = todos.Find(todo => todo.Id == Id);
        if (existing is not null)
            return Results.Conflict("A todo with this Id already exists!");
    }
    var todo = new Todo(Id ?? Nanoid.Generate(alphabet, 10), Title, Done);
    todos.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id:alpha}", (string id, TodoDTO dto) =>
{
    var (Id, Title, Done) = dto;
    var index = todos.FindIndex(todo => todo.Id == id);
    if (index is -1) return Results.NotFound();
    todos[index] = todos[index] with { Title = Title, Done = Done };
    return Results.Ok();
});

app.MapDelete("/todos/{id:alpha}", (string id) =>
{
    var removed = todos.RemoveAll(todo => todo.Id == id);
    if (removed is not 0) return Results.NoContent();
    return Results.NotFound();
});

app.Run();

record Todo(string Id, string Title, bool Done);
record TodoDTO(string? Id, string Title, bool Done);
