using NanoidDotNet;

const string alphabet = "abcdefghijkmnpqrtwxyz346789ABCDEFGHJKLMNPQRTUVWXY_";

var builder = WebApplication.CreateBuilder(args);

var todos = new List<Todo>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/todos", () => todos);

app.MapGet("/todos/{id:alpha}", (string id) =>
{
    var todo = todos.Find(todo => todo.Id.Equals(id));
    if (todo is null) return Results.NotFound();
    return Results.Ok(todo);
});

app.MapPost("/todos", (TodoDTO dto) =>
{
    var (Id, Title, Deadline, Done) = dto;
    if (Title is null or "")
        return Results.BadRequest("Title cannot be empty.");
    Title = Title.Trim();
    if (Id is not null)
    {
        var existing = todos.Find(todo => todo.Id.Equals(Id));
        if (existing is not null)
            return Results.Conflict("A todo with this Id already exists!");
    }
    else Id = Nanoid.Generate(alphabet, 10);
    var deadline = Deadline ?? DateTime.Now.AddDays(1);
    var todo = new Todo(Id, Title, deadline, Done);
    todos.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id:alpha}", (string id, TodoDTO dto) =>
{
    var (Id, Title, Deadline, Done) = dto;
    var index = todos.FindIndex(todo => todo.Id.Equals(id));
    if (index is -1) return Results.NotFound();
    if (Title is null or "")
        return Results.BadRequest("Title cannot be empty.");
    Title = Title.Trim();
    todos[index] = todos[index] with
    {
        Title = Title,
        Deadline = Deadline ?? todos[index].Deadline,
        Done = Done
    };
    return Results.Ok();
});

app.MapDelete("/todos/{id:alpha}", (string id) =>
{
    var removed = todos.RemoveAll(todo => todo.Id.Equals(id));
    if (removed is not 0) return Results.NoContent();
    return Results.NotFound();
});

app.Run();

record Todo(string Id, string Title, DateTime Deadline, bool Done);
record TodoDTO(string? Id, string? Title, DateTime? Deadline, bool Done);
