const string unixSocketPath = "/tmp/uds-dotnet-bun.sock";

if (File.Exists(unixSocketPath))
    File.Delete(unixSocketPath);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(options => options.ListenUnixSocket(unixSocketPath));

var todos = new List<Todo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/todos", () => todos);

app.MapGet("/todos/{id:guid}", (Guid id) =>
{
    var todo = todos.Find(todo => todo.Id == id);
    if (todo is null) return Results.NotFound();
    return Results.Ok(todo);
});

app.MapPost("/todos", (TodoDTO dto) =>
{
    var (Id, Title, Done) = dto;
    if (Id is not null)
    {
        var existing = todos.Find(todo => todo.Id == Id);
        if (existing is not null)
            return Results.Conflict("A todo with this Id already exists!");
    }
    var todo = new Todo(Id ?? Guid.NewGuid(), Title, Done);
    todos.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id:guid}", (Guid id, TodoDTO dto) =>
{
    var (Id, Title, Done) = dto;
    var index = todos.FindIndex(todo => todo.Id == id);
    if (index is -1) return Results.NotFound();
    todos[index] = todos[index] with { Title = Title, Done = Done };
    return Results.Ok();
});

app.MapDelete("/todos/{id:guid}", (Guid id) =>
{
    var removed = todos.RemoveAll(todo => todo.Id == id);
    if (removed is not 0) return Results.NoContent();
    return Results.NotFound();
});

app.Run();

record Todo(Guid Id, string Title, bool Done);
record TodoDTO(Guid? Id, string Title, bool Done);
