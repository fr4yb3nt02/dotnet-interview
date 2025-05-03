using Microsoft.EntityFrameworkCore;
using TodoApi.BusinessLogic.Classes;
using TodoApi.BusinessLogic.Interfaces;
using TodoApi.DataAccess.Classes;
using TodoApi.DataAccess.Interfaces;
using TodoApi.Middlewares;
using Hangfire;
using Hangfire.MemoryStorage;
using TodoApi.TestData;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services.AddDbContext<TodoContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("TodoContext"))
    )
    .AddEndpointsApiExplorer()
    .AddControllers();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//Dependency Injection
builder.Services.AddScoped<ITodoListsDA, TodoListsDA>();
builder.Services.AddScoped<ITodoItemsDA, TodoItemsDA>();
builder.Services.AddScoped<ITodoListsBL, TodoListsBL>();
builder.Services.AddScoped<ITodoItemsBL, TodoItemsBL>();
builder.Services.AddScoped<ExceptionMiddleware>();

//Para tener Swagger (documentacion)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Todo API",
        Version = "v1",
        Description = "API para gestionar listas de tareas con sus items asociados :D.",
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
    await SeedDatabase.SeedAsync(context);
}


app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
