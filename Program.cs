// using ApplicationsApi.Controllers;
//
// UserController.CheckPermissions(new[] { "" });
// return;

using ApplicationsApi.ExceptionHandling;
using ApplicationsApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ExceptionHandlerMiddleware>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseWebSockets();

app.MapControllers();


app.Run();